﻿using Calabonga.AspNetCore.AppDefinitions;
using MassTransit;
using Microsoft.AspNetCore.Builder;

namespace MicroserviceTemplate.Api.Definitions.MassTransit;

public class MassTransitDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ClearSerialization();
                cfg.UseRawJsonSerializer();
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}