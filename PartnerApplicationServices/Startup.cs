using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PartnerApplicationServices.Common;
using PartnerApplicationServices.DataAccess;
using PartnerApplicationServices.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PartnerApplicationServices
{
    public class Startup
    {
        public static IEnumerable<Claim> UserClaims;
        public static IConfigurationSection ConnectionStrings { get; private set; }
        public static TokenValidationParameters tokenValidationParameters;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionStrings = Configuration.GetSection("ConnectionStrings");
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
            services.AddScoped<IUtility, Common.Utility>();
            services.AddScoped<IFriendRepo, FriendRepo>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<ILoginServices, LoginServices>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PartnerApplicationServices", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PartnerApplicationServices v1"));
            }

            app.Use(async (context, next) =>
            {
                if (context.Request.Headers.ContainsKey("Authorization"))
                {
                    string token = context.Request.Headers["Authorization"];
                    token = token.Substring("Bearer ".Length);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    try
                    {
                        var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                        UserClaims = claimsPrincipal.Claims;
                    }
                    catch(Exception e)
                    {

                    }
                    
                }
                await next();
            });

            app.UseHttpsRedirection();

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
