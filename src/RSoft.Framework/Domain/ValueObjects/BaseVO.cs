using FluentValidator;

namespace RSoft.Framework.Domain.ValueObjects
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
