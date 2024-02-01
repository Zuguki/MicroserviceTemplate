﻿using System.Net;
using System.Security.Authentication;
using Calabonga.AspNetCore.AppDefinitions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;

namespace MicroserviceTemplate.Api.Definitions.ErrorHandling;

public class ErrorHandlingDefinition : AppDefinition
{
    public override bool Enabled => true;

    public override void ConfigureApplication(WebApplication app) =>
        app.UseExceptionHandler(error => error.Run(async context =>
        {
            context.Response.ContentType = "application/json";
            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature is not null)
            {
                Log.Error($"Something went wrong in the {contextFeature.Error}");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                if (app.Environment.IsDevelopment())
                    await context.Response.WriteAsync($"INTERNAL SERVER ERROR: {contextFeature.Error}");
                else
                    await context.Response.WriteAsync("INTERNAL SERVER ERROR. PLEASE TRY AGAIN LATER");
            }
        }));

    private static HttpStatusCode GetErrorCode(Exception e)
        => e switch
        {
            ValidationException _ => HttpStatusCode.BadRequest,
            AuthenticationException _ => HttpStatusCode.Forbidden,
            NotImplementedException _ => HttpStatusCode.NotImplemented,
            _ => HttpStatusCode.InternalServerError
        };
}