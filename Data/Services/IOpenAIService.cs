namespace MudanzaApp.Data.Services
{
    public interface IOpenAIService
    {
        Task<MudanzaItemAnalysisResult> AnalyzeItemImageAsync(byte[] imageData);
    }

    public class MudanzaItemAnalysisResult
    {
        public bool Success { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public int? EstimatedQuantity { get; set; }
        public string Category { get; set; }
        public string ErrorMessage { get; set; }
    }
}
