using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Text;
using WebApi.Swagger;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureService
    {
        public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration config)
        {

            //authentication and authorization
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["Jwt:Issuer"], //appsettings.json JWT ISSUER 
                        ValidAudience = config["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
                    };
                });

            //add swaggerGen to project
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Useful sources and tags API",
                    Description = "An ASP.NET Core Web API for useful sources with tags",
                    Contact = new OpenApiContact()
                    {
                        Name = "Contact me",
                        Url = new Uri(@"https://docs.microsoft.com/ru-ru/aspnet/web-api/overview/getting-started-with-aspnet-web-api/tutorial-your-first-web-api")
                    },
                });

                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                opt.OperationFilter<SecurityRequirementsOperationFilter>();

                //add possibility to add comments to endpoints
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            services.AddControllers()
                //for the PATCH request
                .AddNewtonsoftJson(s =>
                {
                    s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //adds for control of user who makes changes
            //services.AddSingleton<ICurrentUserService, CurrentUserService>();
            return services;
        }
    }
}
