namespace Presentation.Extensions;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Presentation.Endpoints;
using Serilog;

[ExcludeFromCodeCoverage]
public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        #region Logging

        _ = app.UseSerilogRequestLogging();

        #endregion Logging

        #region Security

        _ = app.UseHsts();

        #endregion Security

        #region API Configuration

        _ = app.UseHttpsRedirection();

        #endregion API Configuration

        #region Swagger

        var ti = CultureInfo.CurrentCulture.TextInfo;

        _ = app.UseSwagger();
        _ = app.UseSwaggerUI(c =>
            c.SwaggerEndpoint(
                "/swagger/v1/swagger.json",
                $"CleanMinimalApi - {ti.ToTitleCase(app.Environment.EnvironmentName)} - V1"));

        #endregion Swagger

        #region MinimalApi

        _ = app.MapUserEndpoints();
        _ = app.MapPresentationEndpoints();
        _ = app.MapSlideEndpoints();

        #endregion MinimalApi

        return app;
    }
}
