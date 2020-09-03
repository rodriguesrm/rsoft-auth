namespace RSoft.Framework.Domain.Entities
{

    /// <summary>
    /// Address data interface
    /// </summary>
    public interface IAddress
    {

        /// <summary>
        /// Street name
        /// </summary>
        string StreetName { get; set; }

        /// <summary>
        /// Address number
        /// </summary>
        string AddressNumber { get; set; }

        /// <summary>
        /// A secondary address like 'Apt. 2' or 'Suite 321'.
        /// </summary>
        string SecondaryAddress { get; set; }

        /// <summary>
        /// District name
        /// </summary>
        string District { get; set; }

        /// <summary>
        /// City data
        /// </summary>
        string City { get; set; }

        /// <summary>
        /// State data
        /// </summary>
        string State { get; set; }

        /// <summary>
        /// Zip Code
        /// </summary>
        string ZipCode { get; set; }

    }

}