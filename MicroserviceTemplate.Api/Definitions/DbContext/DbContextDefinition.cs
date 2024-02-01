using Calabonga.AspNetCore.AppDefinitions;
using MicroserviceTemplate.Api.DataBase.EF;

namespace MicroserviceTemplate.Api.Definitions.DbContext;

public class DbContextDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
        => builder.Services.AddDbContext<ApplicationDbContext>();
}