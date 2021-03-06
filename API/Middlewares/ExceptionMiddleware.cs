using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middlewares
{
    public class ExceptionMiddleware
    {
        public RequestDelegate Next { get; }
        public ILogger<ExceptionMiddleware> Logger { get; }
        public IHostEnvironment Env { get; }
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            this.Env = env;
            this.Logger = logger;
            this.Next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
          
          try
          {
                  await this.Next(context);
          }
          catch (Exception ex)
          {
              Logger.LogError(ex,ex.Message);
              context.Response.ContentType = "application/json";
              context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

              var response = Env.IsDevelopment()
              ? new ApiException(context.Response.StatusCode,ex.Message,ex.StackTrace?.ToString())
              : new ApiException(context.Response.StatusCode, "Internal Server Error");

              var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
              
              var json = JsonSerializer.Serialize(response,options);

              await context.Response.WriteAsync(json);
          }

        }
    }
}