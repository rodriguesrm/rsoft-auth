using sys = System;

namespace RSoft.Framework.Exception
{

    /// <summary>
    /// Exception thrown when the entity to be recorded is not valid for the operation, check the entity's notifications.
    /// </summary>
    public class InvalidEntityException : sys.Exception
    {

        /// <summary>
        /// Initializes a new instance of the RSoft.Framework.Exception.InvalidEntityException class with a specified error message.
        /// </summary>
        /// <param name="entityName">Entity name</param>
        public InvalidEntityException(string entityName) : base($"The entity {entityName} is invalid to insert or update in database context")
        {
        }

    }
}
