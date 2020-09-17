using RSoft.Framework.Cross.Entities;

namespace RSoft.Framework.Domain.Contracts
{

    /// <summary>
    /// Address validate contract, where:
    /// Street name is required and has 2 and 80 characters.
    /// Address number is required and has 20 maximum characters.
    /// Secondary address is optional, but has 40 maxium characters.
    /// District/Neighborhood is required and 2 and 50 characters.
    /// City is required and 2 and 80 characters.
    /// State is required and has 2 characters.
    /// Zip code is required and has 8 digits (only numbers)
    /// </summary>
    public class AddressValidationContract : BaseValidationContract
    {

        #region Constructors

        /// <summary>
        /// Create a new instance of object
        /// </summary>
        /// <param name="address">Address object instance</param>
        public AddressValidationContract(IAddress address)
        {

            //TODO: Globalization
            //(names and messages)
            Contract
                .Requires()

                .IsNotNullOrEmpty(address.StreetName, "StreetName", "Street name is required")
                .HasMinLen(address.StreetName ?? string.Empty, 2, "StreetName", "Street name must contain at least 2 characters")
                .HasMaxLen(address.StreetName ?? string.Empty, 80, "StreetName", "Street name must contain a maximum of 80 characters")

                .IsNotNullOrEmpty(address.AddressNumber, "AddressNumber", "Address number is required")
                .HasMaxLen(address.StreetName ?? string.Empty, 20, "AddressNumber", "Address number must contain a maximum of 20 characters")

                .HasMaxLen(address.SecondaryAddress ?? string.Empty, 40, "SecondaryAddress", "Secondary address must contain a maximum of 40 characters")

                .IsNotNullOrEmpty(address.District, "District", "District/Neighborhood is required")
                .HasMinLen(address.District ?? string.Empty, 2, "District", "District/Neighborhood must contain at least 2 characters")
                .HasMaxLen(address.District ?? string.Empty, 50, "District", "District/Neighborhood must contain a maximum of 50 characters")

                .IsNotNullOrEmpty(address.City, "City", "City is required")
                .HasMinLen(address.City ?? string.Empty, 2, "City", "City must contain at least 2 characters")
                .HasMaxLen(address.City ?? string.Empty, 80, "City", "City must contain a maximum of 80 characters")

                .IsNotNullOrEmpty(address.State, "State", "State is required")
                .HasLen(address.State ?? string.Empty, 2, "State", "State must contain exactly 2 positions")

                .IsNotNullOrEmpty(address.ZipCode, "ZipCode", "Zip code is required")
                .HasLen(address.ZipCode, 8, "ZipCode", "The zip code must contain exactly 8 positions")
                .Matchs(address.ZipCode, "[0-9]{8}", "ZipCode", "Zip code is in an invalid format (enter numbers only)")
            ;

        }

        #endregion

    }
}