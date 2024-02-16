using Calabonga.AspNetCore.AppDefinitions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace MicroserviceTemplate.Api.Definitions.Authorizations;

public class AuthorizationDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        var url = builder.Configuration.GetSection("AuthServer").GetValue<string>("Url");
    }

    public override void ConfigureApplication(WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    } 
}