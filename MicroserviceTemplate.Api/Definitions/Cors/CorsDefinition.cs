using Calabonga.AspNetCore.AppDefinitions;

namespace MicroserviceTemplate.Api.Definitions.Cors;

public class CorsDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        var origins = builder.Configuration.GetSection("Cors")?.GetSection("Origins")?.Value?.Split(',');
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(AppData.PolicyName, builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                if (origins is not {Length: > 0})
                {
                    return;
                }

                if (origins.Contains("*"))
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.SetIsOriginAllowed(host => true);
                    builder.AllowCredentials();
                }
                else
                {
                    foreach (var origin in origins)
                    {
                        builder.WithOrigins(origin);
                    }
                }
            });
        });
    }
}