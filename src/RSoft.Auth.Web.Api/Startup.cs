using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using RSoft.Auth.Cross.IoC;
using RSoft.Auth.Infra.Data.Extensions;
using RSoft.Framework.Web.Api.Identity;
using RSoft.Framework.Web.Filters;
using RSoft.Framework.Web.Options;
using RSoft.Logs.Extensions;
using RSoft.Logs.Middleware;
using System;
using System.Text;

namespace RSoft.Auth.Web.Api
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services
                .AddControllers(opt => GlobalFilters.Configure(opt))
                .AddJsonOptions(opt => opt.JsonSerializerOptions.IgnoreNullValues = true)
                .ConfigureApiBehaviorOptions(opt => opt.SuppressModelStateInvalidFilter = true);
            services.AddHttpContextAccessor();
            services.AddCors();
            services.AddResponseCaching();

            #region Jwt

            JwtOptions jwtOptions = null;
            Configuration.GetSection("Jwt").Bind(jwtOptions);

            byte[] jwtHash = Encoding.ASCII.GetBytes(jwtOptions.Hash);
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(jwtHash);

            services.Configure<JwtTokenConfig>(o =>
            {
                o.Issuer = jwtOptions.Issuer;
                o.Audience = jwtOptions.Audience;
                o.Credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(bearerOptions =>
                {

                    bearerOptions.RequireHttpsMetadata = false;
                    bearerOptions.SaveToken = true;

                    TokenValidationParameters pv = bearerOptions.TokenValidationParameters;

                    pv.ValidateIssuer = true;
                    pv.ValidIssuer = jwtOptions.Issuer;

                    pv.ValidateAudience = true;
                    pv.ValidAudience = jwtOptions.Audience;

                    pv.ValidateIssuerSigningKey = true;
                    pv.IssuerSigningKey = signingKey;

                    pv.RequireExpirationTime = true;
                    pv.ValidateLifetime = true;

                    pv.ClockSkew = TimeSpan.Zero;

                });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(JwtBearerDefaults.AuthenticationScheme, new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());
            });

            #endregion

            services.AddAuthRegister(Configuration);
            services.AddMiddlewareLoggingOption(Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseStaticFiles();
            app.UseResponseCaching();

            app.UseMiddleware<RequestResponseLogging<Startup>>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.MigrateDatabase();

        }
    }
}
