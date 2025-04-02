using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using MudanzaApp.Data.Models;
using System.Globalization;

public static class LocalizationExtensions
{
    public static IServiceCollection AddAppLocalization(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("es-MX")
            };

            options.DefaultRequestCulture = new RequestCulture("en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;

            // Agregar proveedores de cultura personalizados si es necesario
            options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
            {
                // Intentar obtener la cultura preferida del usuario autenticado
                var userManager = context.RequestServices.GetService<UserManager<ApplicationUser>>();
                var signInManager = context.RequestServices.GetService<SignInManager<ApplicationUser>>();

                if (signInManager.IsSignedIn(context.User))
                {
                    var user = await userManager.GetUserAsync(context.User);
                    if (user != null && !string.IsNullOrEmpty(user.PreferredLanguage))
                    {
                        return new ProviderCultureResult(user.PreferredLanguage);
                    }
                }

                // Si no hay usuario autenticado o no tiene preferencia, usar los proveedores predeterminados
                return null;
            }));
        });

        return services;
    }
}