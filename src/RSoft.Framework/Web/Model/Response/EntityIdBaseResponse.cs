using System;

namespace RSoft.Framework.Web.Model.Response
{

    /// <summary>
    /// Abstract object model-response with id
    /// </summary>
    public abstract class EntityIdBaseResponse<TKey> : EntityBaseResponse
        where TKey : struct
    {

        #region Construtores

        /// <summary>
        /// Create a new response instance
        /// </summary>
        /// <param name="id">Key value</param>
        public EntityIdBaseResponse(TKey id)
        {
            Id = id;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Entity id value
        /// </summary>
        public TKey Id { get; set; }

        #endregion

    }

}
