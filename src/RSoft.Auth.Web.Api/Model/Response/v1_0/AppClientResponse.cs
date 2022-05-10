using RSoft.Lib.Common.Web.Models.Response;
using System;

namespace RSoft.Auth.Web.Api.Model.Response.v1_0
{

    /// <summary>
    /// Application-Client response model
    /// </summary>
    public class AppClientResponse : EntityIdBaseResponse<Guid>
    {

        #region Constructors

        /// <summary>
        /// Create a new AppClientResponse instance
        /// </summary>
        /// <param name="id">Client id key value</param>
        public AppClientResponse(Guid id) : base(id)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Application-Client name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Application-Client access-key application
        /// </summary>
        public Guid AccessKey { get; set; }

        /// <summary>
        /// Indicates whether the application-client can log in as a service/application
        /// </summary>
        public bool AllowLogin { get; set; }

        /// <summary>
        /// Acope active status
        /// </summary>
        public bool? IsActive { get; set; } = true;

        #endregion

    }
}
