using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace RSoft.Framework.Web.Options
{

    /// <summary>
    /// Jwt token configuration
    /// </summary>
    public class JwtTokenConfig
    {

        /// <summary>
        /// JWT token issuer
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Token recipient, represents the application that will use it.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Token expiration date and time
        /// </summary>
        public DateTime Expiration => IssuedAt.Add(ValidFor);

        /// <summary>
        /// Token validity start date and time
        /// </summary>
        public DateTime NotBefore => DateTime.UtcNow;

        /// <summary>
        /// Token validity start date and time
        /// </summary>
        public DateTime IssuedAt => DateTime.UtcNow;

        /// <summary>
        /// Defines the length of time the token will be valid
        /// </summary>
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromHours(4);

        /// <summary>
        /// Jti id token generator
        /// </summary>
        public Func<Task<string>> JtiGenerator => () => Task.FromResult(Guid.NewGuid().ToString());

        /// <summary>
        /// A chave de assinatura a ser usada ao gerar tokens.
        /// </summary>
        public SigningCredentials Credentials { get; set; }
    }

}
