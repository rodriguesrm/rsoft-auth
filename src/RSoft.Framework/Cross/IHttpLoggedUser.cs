using System;
using System.Collections.Generic;

namespace RSoft.Framework.Cross
{

    /// <summary>
    /// Http logged application user interface
    /// </summary>
    public interface IHttpLoggedUser<TKey> : IDisposable
        where TKey : struct
    {

        /// <summary>
        /// User id
        /// </summary>
        TKey Id { get; }

        /// <summary>
        /// User name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// user login
        /// </summary>
        string Login { get; }

        /// <summary>
        /// User roles
        /// </summary>
        IEnumerable<string> Roles { get; }

    }

}
