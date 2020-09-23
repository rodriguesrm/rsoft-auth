using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RSoft.Framework.Web.Options;
using System;
using System.Text;

namespace RSoft.Framework.Web.Extensions
{

    /// <summary>
    /// Provides extension methods for Jwt Token
    /// </summary>
    public static class TokenExtension
    {

        /// <summary>
        /// Provides resources for generating JWT authentication token
        /// </summary>
        /// <param name="services">Service collection object instance</param>
        /// <param name="configuration">Configuration object instance</param>
        public static IServiceCollection AddJwtToken(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<JwtOptions>(options => configuration.GetSection("Jwt").Bind(options));

            JwtOptions jwtOptions = new JwtOptions();
            configuration.GetSection("Jwt").Bind(jwtOptions);

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

            return services;

        }

    }

}
