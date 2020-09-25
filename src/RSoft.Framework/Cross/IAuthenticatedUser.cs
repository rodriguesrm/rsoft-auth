using System;

namespace RSoft.Framework.Cross
{

    /// <summary>
    /// logged user/service interface
    /// </summary>
    public interface IAuthenticatedUser : IHttpLoggedUser<Guid>
    {
    }

}
