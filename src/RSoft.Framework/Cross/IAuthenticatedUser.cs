using System;

namespace RSoft.Framework.Cross
{

    /// <summary>
    /// /// Http logged application user interface
    /// </summary>
    public interface IAuthenticatedUser : IHttpLoggedUser<Guid>
    {
    }

}
