
namespace JB.Wolflix.Catalog.Domain.Utils
{
    public class CategoryExceptionMessage
    {
        public static string NameNullExceptionMessage = "Nome não deve ser vazio ou nulo";
        public static string DescriptionNullExceptionMessage = "Descrição não deve ser vazio ou nulo";
        public static string NameMaxLengthExceptionMessage = "Nome deve ter um tamanho máximo de 255 caracteres.";
        public static string NameMinLengthExceptionMessage = "Nome deve ter um tamanho mínimo de três caracteres.";
        public static string DescriptionMaxLengthExceptionMessage = "Descrição deve ter um tamanho máximo de 10.000 caracteres.";


        public static readonly Func<string, string> NameNullExceptionMessageParam = (string fieldName) => $"{fieldName} não deve ser vazio ou nulo";
        public static readonly Func<string, string> DescriptionNullExceptionMessageParam = (string fieldName) => $"{fieldName} não deve ser vazio ou nulo";
        public static readonly Func<string, string> NameMaxLengthExceptionMessageParam = (string fieldName) => $"{fieldName} deve ter um tamanho máximo de 255 caracteres.";
        public static readonly Func<string, int, string> NameMinLengthExceptionMessageParam = (string fieldName, int minLenght) => $"{fieldName} deve ter um tamanho mínimo de {minLenght} caracteres.";
        public static readonly Func<string, string> DescriptionMaxLengthExceptionMessageParam = (string fieldName) => $"{fieldName} deve ter um tamanho máximo de 10.000 caracteres.";
    }
}


