using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWTTest.DataAccess;
using JWTTest.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PartnerApp.Common;

namespace JWTTest
{
    public class Startup
    { 
        public static IConfigurationSection ConnectionStrings { get; private set; }
        public static IEnumerable<Claim> UserClaims;
        private TokenValidationParameters tokenValidationParameters;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionStrings = configuration.GetSection("ConnectionStrings");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = ConnectionStrings.GetSection("Issuer").Value,
                ValidAudience = ConnectionStrings.GetSection("Audience").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConnectionStrings.GetSection("SigningKey").Value)),
                TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConnectionStrings.GetSection("EncryptionKey").Value))
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWTTest", Version = "v1" });
            });
            services.AddScoped<IUtility, PartnerApp.Common.Utility>();
            services.AddScoped<ILoginServices, LoginServices>();
            services.AddScoped<ILoginRepo, LoginRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JWTTest v1"));
            }

            app.UseHttpsRedirection();

            app.Use(async (context, next) =>
            {
                if (context.Request.Headers.ContainsKey("Authorization"))
                {
                    string token = context.Request.Headers["Authorization"];
                    token = token.Substring("Bearer ".Length);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                    UserClaims = claimsPrincipal.Claims;
                }
                await next();
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}