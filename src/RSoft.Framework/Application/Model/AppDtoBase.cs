using FluentValidator;
using RSoft.Framework.Application.Dto;
using System.Linq;

namespace RSoft.Framework.Application.Model
{

    /// <summary>
    /// Abstract dto model base object
    /// </summary>
    public abstract class AppDtoBase : Notifiable, IAppDto
    {

        #region Public Methods

        ///<inheritdoc/>
        public string GetName()
            => GetType().Name.Split(".").ToList().Last();

        #endregion

    }

}
