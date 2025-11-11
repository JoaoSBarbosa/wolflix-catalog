using JB.Wolflix.Catalog.Domain.Exceptions;
using JB.Wolflix.Catalog.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Wolflix.Catalog.Domain.Validation
{
    public class DomainValidation
    {

        public static void NotNull(object? target, string fieldName)
        {
                    
            if (target is null) throw new EntityValidationException(CategoryExceptionMessage.NameNullExceptionMessageParam(fieldName));

        }
        public static void NotNullOrEmpty(string? target, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(target)) throw new EntityValidationException(CategoryExceptionMessage.NameNullExceptionMessageParam(fieldName));
        }
        public static void MinLength(string target, string fieldName)
        {
            if (target.Length < 3) throw new EntityValidationException(CategoryExceptionMessage.NameMinLengthExceptionMessageParam(fieldName));
        }

        public static void MaxLength(string target, string fieldName)
        {
            if ( target.Length > 255) throw new EntityValidationException(CategoryExceptionMessage.NameMaxLengthExceptionMessageParam(fieldName));
        }
        public static void MaxLengthDescription(string target, string fieldName)
        {
            if (target.Length > 10000) throw new EntityValidationException(CategoryExceptionMessage.DescriptionMaxLengthExceptionMessageParam(fieldName));
        }
    }
}
