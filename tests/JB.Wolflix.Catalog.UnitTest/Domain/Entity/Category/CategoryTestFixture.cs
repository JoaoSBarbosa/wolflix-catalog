using JB.Wolflix.Catalog.UnitTest.Common;
using JB.Wolflix.Catalog.UnitTest.Domain.Entity.Category;
using Xunit;
using CategoryEntity = JB.Wolflix.Catalog.Domain.Entity.Category;


namespace JB.Wolflix.Catalog.UnitTest.Domain.Entity.Category
{
    public class CategoryTestFixture: BaseFixture 
    {
        // Refatoração nos testes para utilizar o BaseFixture, esse que implementa o Bogus Faker(lib) para gerar dados aleatórios nos testes
        public CategoryTestFixture():base()
        {

        }

        public const string DescriptionNullExceptionMessage = "Descrição não deve ser vazio ou nulo";
        public const string NameNullExceptionMessage = "Nome não deve ser vazio ou nulo";
        public const string NameMinLengthExceptionMessage = "Nome deve ter um tamanho mínimo de três caracteres.";
        public const string NameMaxLengthExceptionMessage = "Nome deve ter um tamanho máximo de 255 caracteres.";
        public const string DescriptionMaxLengthExceptionMessage = "Descrição deve ter um tamanho máximo de 10.000 caracteres.";
        // Faker vem da classe abstract BaseFixture no qual usa a lib Bogus.
        // Gera uma categoria válida com nome entre 3 e 255 caracteres e descrição até 10.000 caracteres.
        public string GetValidCategoryName()
        {
            string categoryName = "";

            while ( categoryName.Length < 3) categoryName = Faker.Commerce.Categories(1)[0];
            return categoryName.Length > 255 ? categoryName[..255]: categoryName;
        }
        public string GetValidCategoryDescription()
        {
            var desc = Faker.Commerce.ProductDescription();

            return desc.Length > 10_000 ? desc[..10_000] : desc;
        }

        public CategoryEntity GetValidCategory() => new(GetValidCategoryName(), GetValidCategoryDescription());
        public CategoryEntity GetValidCategoryByParam(string? name = "Terror", string? description = "Filmes de Terror") => new CategoryEntity(name, description);
    }
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixture> { }