﻿using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MsfServer.Domain.Shared.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MsfServer.HttpApi.Host.Middlewares
{
    public class CustomExceptionMiddleware(RequestDelegate next, IOptions<ApiBehaviorOptions> options)
    {
        private readonly RequestDelegate _next = next;
        private readonly ApiBehaviorOptions _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomException ex)
            {
                string result = CreateProblemDetails(httpContext: context, statusCode: ex.ErrorCode, error: ex.ErrorMessage);
                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                string result = CreateProblemDetails(httpContext: context, statusCode: StatusCodes.Status500InternalServerError, error: ex.Message);
                await context.Response.WriteAsync(result);
            }
        }

        public string CreateProblemDetails(
            HttpContext httpContext,
            int? statusCode = null,
            string? error = null)
        {
            statusCode ??= 500;

            var customErrorDetails = new CustomErrorDetails
            {
                Status = statusCode,
                Error = error,
            };

            ApplyProblemDetailsDefaults(httpContext, customErrorDetails, statusCode.Value);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode.Value;

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy() // chuyển đổi các tên thuộc tính thành dạng camelCase
                }
            };

            var result = JsonConvert.SerializeObject(customErrorDetails, settings);

            return result;
        }

        private static void ApplyProblemDetailsDefaults(HttpContext httpContext, CustomErrorDetails customErrorDetails, int statusCode)
        {
            ArgumentNullException.ThrowIfNull(httpContext);
            customErrorDetails.Instance = httpContext.Request.GetEncodedPathAndQuery();
            customErrorDetails.Status ??= statusCode;
        }
    }

    public class CustomErrorDetails
    {
        public int? Status { get; set; }
        public string? Error { get; set; }
        public string? Instance { get; set; }
    }
}
