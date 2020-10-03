using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSoft.Framework.Application.Dto;
using RSoft.Framework.Web.Model.Response;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Framework.Web.Api
{

    /// <summary>
    /// CRUD API controller base
    /// </summary>
    [ApiController]
    public abstract class ApiCrudBaseController<TKey, TDto, TRequest, TResponse> : ApiBaseController
        where TKey : struct
        where TDto : AppDtoIdBase<TKey>
    {

        #region Constructors

        /// <summary>
        /// Create a new controller instance
        /// </summary>
        public ApiCrudBaseController() : base() { }

        #endregion

        #region Local abstracts methods

        /// <summary>
        /// Maps a request object to a Dto object
        /// </summary>
        /// <param name="request">Request object instance</param>
        protected abstract TDto Map(TRequest request);

        /// <summary>
        /// Maps a dto object to a response object
        /// </summary>
        /// <param name="dto">Dto object instance</param>
        protected abstract TResponse Map(TDto dto);

        /// <summary>
        /// Add a new entity
        /// </summary>
        /// <param name="dto">Dto object instance</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected abstract Task<TDto> AddAsync(TDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Find entity by key
        /// </summary>
        /// <param name="key">Entity key value</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected abstract Task<TDto> GetByIdAsync(TKey key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected abstract Task<IEnumerable<TDto>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Update an existing entity
        /// </summary>
        /// <param name="dto">Dto object instance</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected abstract Task<TDto> SaveUpdateAsync(TDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an entity
        /// </summary>
        /// <param name="key">Entity key value</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected abstract Task RemoveAsync(TKey key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Prepares the response object from an entity insert operation
        /// </summary>
        /// <param name="dto">Dto object instance</param>
        protected abstract object PrepareInsertResponse(TDto dto);

        #endregion

        #region Métodos Locais

        /// <summary>
        /// Maps a dto list object to a response list object
        /// </summary>
        /// <param name="dtos">Dto object list</param>
        protected virtual IEnumerable<TResponse> Map(IEnumerable<TDto> dtos)
        {
            IEnumerable<TResponse> result = dtos.Select(s => Map(s)).ToList();
            return result;
        }

        /// <summary>
        /// Perform the Insert operation
        /// </summary>
        /// <param name="request">Request data</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected async Task<IActionResult> RunInsertAsync(TRequest request, CancellationToken cancellationToken = default)
        {
            TDto dto = Map(request);

            dto = await AddAsync(dto, cancellationToken);

            if (dto.Invalid)
            {
                IEnumerable<GenericNotificationResponse> msg = GetNotificationsErrors(dto.Notifications);
                return BadRequest(msg);
            }

            return StatusCode(StatusCodes.Status201Created, PrepareInsertResponse(dto));
        }

        /// <summary>
        /// Find record by key
        /// </summary>
        /// <param name="key">Entity key value</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected async Task<IActionResult> RunGetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            TDto dto = await GetByIdAsync(key, cancellationToken);
            if (dto == null)
                return NotFound("Data not found");
            TResponse resp = Map(dto);
            return Ok(resp);
        }

        /// <summary>
        /// List all entity
        /// </summary>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected async Task<IActionResult> RunListAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<TDto> result = await GetAllAsync(cancellationToken);
            IEnumerable<TResponse> resp = Map(result);
            return Ok(resp);
        }

        /// <summary>
        /// Update an existing entity
        /// </summary>
        /// <param name="key">Entity key value</param>
        /// <param name="request">Request data</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected async Task<IActionResult> RunUpdateAsync(TKey key, TRequest request, CancellationToken cancellationToken = default)
        {

            TDto dto = await GetByIdAsync(key, cancellationToken);
            if (dto == null)
                return NotFound("Data not found");

            dto = Map(request);
            dto.Id = key;
            dto = await SaveUpdateAsync(dto, cancellationToken);

            if (dto.Invalid)
            {
                IEnumerable<GenericNotificationResponse> msg = GetNotificationsErrors(dto.Notifications);
                return BadRequest(msg);
            }

            return NoContent();

        }

        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="key">Entity key value</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected async Task<IActionResult> RunDeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            TDto dto = await GetByIdAsync(key, cancellationToken);
            if (dto == null)
                return NotFound("Data not found");
            await RemoveAsync(key, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Add a new entity
        /// </summary>
        /// <param name="request">Request data</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected virtual async Task<IActionResult> InsertAsync(TRequest request, CancellationToken cancellationToken = default)
            => await RunActionAsync(RunInsertAsync(request, cancellationToken), cancellationToken);

        /// <summary>
        /// Find entity by key
        /// </summary>
        /// <param name="key">Entity key value</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected virtual async Task<IActionResult> GetAsync(TKey key, CancellationToken cancellationToken = default)
            => await RunActionAsync(RunGetAsync(key, cancellationToken), cancellationToken);

        /// <summary>
        /// List all entities
        /// </summary>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected virtual async Task<IActionResult> ListAsync(CancellationToken cancellationToken = default)
            => await RunActionAsync(RunListAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Update an existing entity
        /// </summary>
        /// <param name="key">Entity key value</param>
        /// <param name="request">Request data</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected virtual async Task<IActionResult> UpdateAsync(TKey key, TRequest request, CancellationToken cancellationToken = default)
            => await RunActionAsync(RunUpdateAsync(key, request, cancellationToken), cancellationToken);

        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="key">Entity key value</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected virtual async Task<IActionResult> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
            => await RunActionAsync(RunDeleteAsync(key, cancellationToken), cancellationToken);

        #endregion

    }
}
