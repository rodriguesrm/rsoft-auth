﻿using RSoft.Auth.Domain.Entities;
using System;
using RSoft.Lib.DDD.Domain.Services;

namespace RSoft.Auth.Domain.Services
{

    /// <summary>
    /// Scope domain service interface
    /// </summary>
    public interface IScopeDomainService : IDomainServiceBase<Scope, Guid>
    {
    }

}
