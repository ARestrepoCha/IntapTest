using FluentValidation;
using FluentValidation.AspNetCore;
using IntapTest.Data;
using IntapTest.Data.Entities;
using IntapTest.Data.Seeds;
using IntapTest.Domain;
using IntapTest.Domain.Validator;
using IntapTest.Shared.AppConfigurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace IntapTest.Api
{
    public class StartUp
    {
        public IConfiguration Configuration { get; }

        public StartUp(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public async Task ConfigureServices(IServiceCollection services)
        {
            var appConfiguration = Configuration.GetApplicationConfig();

            services.AddValidatorsFromAssemblyContaining<Program>();
            services.AddValidatorsFromAssemblyContaining<StartupDomainConfiguration>();
            services.AddFluentValidationAutoValidation();

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateAttribute));
            })
            .AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<StartUp>());

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // Appsettings
            services.AddSingleton<IAppConfiguration>(appConfiguration);
            services.AddDbContext<IntapTestDbContext>(options =>
            {                
                options.UseSqlServer(appConfiguration.ConnectionStrings.DefaultConnection);
            });

            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<IntapTestDbContext>()
            .AddDefaultTokenProviders();

            services.AddCors(options =>
            {
                options.AddPolicy("corspolicy", (policy) =>
                {
                    policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
                });
            });

            //Add DI for data services
            var data = new StartupDataConfiguration();
            data.DataConfigureServices(services);

            //Add DI for domain services
            var domain = new StartupDomainConfiguration();
            domain.DomainConfigureServices(services, appConfiguration);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", (policy) =>
                {
                    policy.RequireRole("Administrator");
                });

                options.AddPolicy("Employee", (policy) =>
                {
                    policy.RequireRole("Employee");
                });
            });

            // set the Authentication Scheme
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                // Validate the token bt receivig the token from the Authorization Request Header
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKeys = new SymmetricSecurityKey[]
                    {
                        new SymmetricSecurityKey(
                            DeriveKeyBytes(appConfiguration.JWTConfiguration.TokenSecretKey)),
                        new SymmetricSecurityKey(
                            DeriveKeyBytes(appConfiguration.JWTConfiguration.AccessRefreshToken))
                    },
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
                x.Events = new JwtBearerEvents()
                {
                    // If the Token is expired the respond
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Authentication-Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            }).AddCookie(options =>
            {
                options.Events.OnRedirectToAccessDenied =
                options.Events.OnRedirectToLogin = c =>
                {
                    c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.FromResult<object>(null);
                };
            });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JSON Web Token Authorization header using the Bearer scheme.  \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                        },
                        new List<string>()
                    }
                });
            });

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            await SeedRoles.CreateApplicationRoles(serviceProvider);
        }

        private static byte[] DeriveKeyBytes(string secret)
        {
            byte[] keyBytes = Convert.FromBase64String(secret);
            int desiredLength = 256 / 8;

            using Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(keyBytes, new byte[] { 0x01, 0x02, 0x03, 0x04 }, 10000);
            // Deriva bytes adicionales hasta alcanzar la longitud deseada
            while (deriveBytes.GetBytes(desiredLength).Length < desiredLength) { }

            return deriveBytes.GetBytes(desiredLength);
        }

        public async Task Configure(WebApplication app, IWebHostEnvironment env, IServiceCollection services)
        {
            if (!env.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Assure For Life API V1");
                });
            }

            //app.UseMiddleware<FixedTokenMiddleware>();
            //app.UseMiddleware<ErrorHandlingMiddleware>();
            using (IServiceScope serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                IntapTestDbContext context = serviceScope.ServiceProvider.GetRequiredService<IntapTestDbContext>();
                context.Database.Migrate();
            }

            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    await context.Response.WriteAsync("Access Denied");
                }
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("corspolicy");
            app.MapControllers();

            app.Run();
        }
    }
}
