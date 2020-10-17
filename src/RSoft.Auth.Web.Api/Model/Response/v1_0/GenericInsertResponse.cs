using System;

namespace RSoft.Auth.Web.Api.Model.Response.v1_0
{

    /// <summary>
    /// Generic data addition response model
    /// </summary>
    public class GenericInsertResponse
    {

        #region Constructors

        /// <summary>
        /// Creates a new object instance
        /// </summary>
        /// <param name="id">Entity id</param>
        public GenericInsertResponse(Guid id)
        {
            Id = id;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Entity id
        /// </summary>
        public Guid Id { get; set; }

        #endregion

    }

}
