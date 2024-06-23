using System.ComponentModel.DataAnnotations;

namespace KalumManagement.Helpers
{
    public class PrefixPhoneAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            string prefix = value.ToString().Substring(0,3);
            if(!prefix.Equals("502"))
            {
                return new ValidationResult("Es necesario agregar el prefijo del pais en el nùmero de telefono");
            }
            return ValidationResult.Success;
        }
    }
}