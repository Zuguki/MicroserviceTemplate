﻿using Calabonga.AspNetCore.AppDefinitions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceTemplate.Api.Definitions.Common;

public class CommonDefinition : AppDefinition
{
    public override void ConfigureApplication(WebApplication app)
        => app.UseHttpsRedirection();

    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddLocalization();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddResponseCaching();
        builder.Services.AddMemoryCache();
    }
}