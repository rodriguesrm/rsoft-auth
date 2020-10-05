using System.Collections.Generic;

namespace RSoft.Framework.Cross
{

    /// <summary>
    /// Http logged application user interface
    /// </summary>
    public interface IHttpLoggedUser<TKey>
        where TKey : struct
    {

        /// <summary>
        /// User id
        /// </summary>
        TKey? Id { get; }

        /// <summary>
        /// User first name
        /// </summary>
        string FirstName { get; }

        /// <summary>
        /// User last name
        /// </summary>
        string LastName { get; }

        /// <summary>
        /// User login
        /// </summary>
        string Login { get; }

        /// <summary>
        /// User e-mail
        /// </summary>
        string Email { get; }

        /// <summary>
        /// User scopes
        /// </summary>
        IEnumerable<string> Scopes { get; }

        /// <summary>
        /// User roles
        /// </summary>
        IEnumerable<string> Roles { get; }

    }

}
