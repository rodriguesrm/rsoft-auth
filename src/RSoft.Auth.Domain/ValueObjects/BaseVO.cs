using FluentValidator;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSoft.Auth.Domain.ValueObjects
{

    /// <summary>
    /// Value object model bases
    /// </summary>
    public abstract class BaseVO : Notifiable
    {

        #region Public methods

        /// <summary>
        /// Execute validate
        /// </summary>
        protected abstract void Validate();

        #endregion

    }
}
