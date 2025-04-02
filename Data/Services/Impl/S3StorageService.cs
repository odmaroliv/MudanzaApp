using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace MudanzaApp.Data.Services.Impl
{
    public class S3StorageService : IStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly IConfiguration _configuration;
        private readonly ILogger<S3StorageService> _logger;
        private readonly string _bucketName;

        public S3StorageService(IConfiguration configuration, ILogger<S3StorageService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            _bucketName = _configuration["AWS:S3:BucketName"] ?? "";

            var awsOptions = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(_configuration["AWS:S3:Region"])
            };

            _s3Client = new AmazonS3Client(
                _configuration["AWS:S3:AccessKey"],
                _configuration["AWS:S3:SecretKey"],
                awsOptions
            );
        }

        public async Task<string> UploadFileAsync(byte[] fileBytes, string fileName, string contentType)
        {
            try
            {
                // Generar un nombre único para el archivo
                var extension = Path.GetExtension(fileName);
                var uniqueFileName = $"{Guid.NewGuid()}{extension}";

                // Determinar la carpeta en S3 según el tipo de contenido
                var folder = "images";
                if (contentType.StartsWith("image/"))
                {
                    folder = "images";
                }
                else if (contentType.StartsWith("application/"))
                {
                    folder = "documents";
                }

                var key = $"{folder}/{uniqueFileName}";

                using (var memoryStream = new MemoryStream(fileBytes))
                {
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = memoryStream,
                        Key = key,
                        BucketName = _bucketName,
                        ContentType = contentType,
                        CannedACL = S3CannedACL.PublicRead
                    };

                    var transferUtility = new TransferUtility(_s3Client);
                    await transferUtility.UploadAsync(uploadRequest);
                }

                // Construir la URL del archivo
                var url = $"https://{_bucketName}.s3.amazonaws.com/{key}";

                _logger.LogInformation($"File uploaded to S3: {url}");
                return url;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading file to S3: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            try
            {
                // Extraer el nombre del bucket y la clave del archivo de la URL
                var uri = new Uri(fileUrl);
                var path = uri.PathAndQuery.TrimStart('/');

                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = path
                };

                await _s3Client.DeleteObjectAsync(deleteObjectRequest);

                _logger.LogInformation($"File deleted from S3: {fileUrl}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting file from S3: {ex.Message}");
                return false;
            }
        }
    }
}

