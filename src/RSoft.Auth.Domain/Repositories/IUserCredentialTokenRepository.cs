﻿using RSoft.Auth.Domain.Entities;
using RSoft.Lib.Design.Infra.Data;
using System;

namespace RSoft.Auth.Domain.Repositories
{

    /// <summary>
    /// User Credential Token repository contract interface
    /// </summary>
    public interface IUserCredentialTokenRepository : IRepositoryBase<UserCredentialToken, Guid> 
    { 
    }

}
