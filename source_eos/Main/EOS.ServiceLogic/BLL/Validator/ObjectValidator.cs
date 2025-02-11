using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EOS.ServiceLogic.BLL.Validator
{
    public static class ObjectValidator<T>
    {
        /// <summary>
        /// Validates the obj properties depending on The aspect described in the data annotations used to decorate the properties
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>A list of validations errors separated by ";"</returns>
        public static List<string> ValidateObject(T obj)
        {
            var validationContext = new ValidationContext(obj, null, null);
            
            var errorsList = new List<ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(obj, validationContext, errorsList, true);
            return errorsList.Select(x => x.ErrorMessage).ToList();
        }
    }
}
