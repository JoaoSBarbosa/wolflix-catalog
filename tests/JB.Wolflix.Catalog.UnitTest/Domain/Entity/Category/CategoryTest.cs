using FluentAssertions;
using JB.Wolflix.Catalog.Domain.Exceptions;
using JB.Wolflix.Catalog.Domain.Utils;
using JB.Wolflix.Catalog.UnitTest.Common;
using Xunit;

// Alias usado para referenciar a entidade real do domínio
using DomainEntity = JB.Wolflix.Catalog.Domain.Entity;

namespace JB.Wolflix.Catalog.UnitTest.Domain.Entity.Category
{

    [Collection(nameof(CategoryTestFixture))] // Aplicando Fixture
    public class CategoryTest
    {
        // Aplicando Fixture
        private readonly CategoryTestFixture _fixture;

        public CategoryTest(CategoryTestFixture fixture)
        {
            _fixture = fixture;
        }


        // IEnumeable é uma interface que permite retornar uma coleção de objetos
        // Ou seja, uma lista de arrays que podem ser percorridos
        // Ela pode gerar valores sob demanda, economizando memória

        public static IEnumerable<object[]> GetInvalidadeNameWithThan3Caharacters(int numerOfTest = 6)
        {
            var fix = new CategoryTestFixture();

            for (int i = 0; i < numerOfTest; i++)
            {
                var isOdd = i % 2 == 1;
                yield return new object[] { fix.GetValidCategoryName()[..(isOdd ? 1 : 2)] };

            }
        }

        #region Tests of instantiation
        [Fact(DisplayName = "Deve instanciar uma categoria válida")]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)] // Agrupa/classifica o teste por domínio
        public void Should_Instantiate_Validate_Category()
        {
            var validCategory = _fixture.GetValidCategory();

            var dateTimeBefore = DateTime.Now;
            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);

            var dateTimeAfter = DateTime.Now.AddSeconds(1);



            category.Should().NotBeNull();
            category.Name.Should().Be(validCategory.Name);
            category.Description.Should().Be(validCategory.Description);
            category.Id.Should().NotBeEmpty();
            category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
            (category.CreatedAt > dateTimeBefore).Should().BeTrue();
            (category.CreatedAt < dateTimeAfter).Should().BeTrue();
            category.IsActive.Should().BeTrue();

        }

        [Theory(DisplayName = "Deve instanciar uma categoria com status de ativo ou inativo")]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        [InlineData(true)]   // Caso 1 → Categoria ativa
        [InlineData(false)]  // Caso 2 → Categoria inativa
        public void Should_Instantiate_Category_With_IsActiveStatus(bool isActive)
        {

            var validateCategory = _fixture.GetValidCategory();
            var dateTimeBefore = DateTime.Now;
            var category = new DomainEntity.Category(validateCategory.Name, validateCategory.Description, isActive);
            var dateTimeAfter = DateTime.Now.AddSeconds(1);


            category.Should().NotBeNull();
            category.Name.Should().BeEquivalentTo(validateCategory.Name);
            category.Description.Should().BeEquivalentTo(validateCategory.Description);
            category.Id.Should().NotBeEmpty();
            category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
            (category.CreatedAt >= dateTimeBefore).Should().BeTrue();
            (category.CreatedAt <= dateTimeAfter).Should().BeTrue();
            category.IsActive.Should().Be(isActive);
        }
        #endregion

        #region Tests of instantiation Empty or null exceptions
        [Theory(DisplayName = "Deve lançar uma exceção ao instancia com um nome vazio")]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("      ")]
        public void Should_Throw_When_Instantiete_With_NameIsEmpty(string? value)
        {

            Action action = () => new DomainEntity.Category(value!, _fixture.GetValidCategoryDescription());
            action.Should().Throw<EntityValidationException>().WithMessage(CategoryExceptionMessage.NameNullExceptionMessage);
        }

        [Fact(DisplayName = "Deve lançar uma exceção ao instanciar com descrição nula")]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        public void Should_Throw_When_Instantiate_With_DescriptionIsNull()
        {
            Action action = () => new DomainEntity.Category(_fixture.GetValidCategoryName(), null!);
            action.Should().Throw<EntityValidationException>().WithMessage(CategoryExceptionMessage.DescriptionNullExceptionMessage);

        }
        #endregion
        #region Validation length rules
        [Theory(DisplayName = "Deve lançar exceção quando o nome tiver menos de 3 caracteres")]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        [MemberData(nameof(GetInvalidadeNameWithThan3Caharacters), parameters: 5)]
        public void Should_Throw_When_NameIsLessThanThreeCharacters(string invalidName)
        {

            Action action = () => _fixture.GetValidCategoryByParam(invalidName);
            action.Should().Throw<EntityValidationException>().WithMessage(CategoryExceptionMessage.NameMinLengthExceptionMessageParam("Nome", 3));

        }


        [Fact(DisplayName = "Deve lançar exceção quando o nome ultrapassar 255 caracteres")]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        public void Should_Throw_When_NameIsGreaterThan255Characters()
        {
            var longName = new string('A', 256);
            var validDescription = _fixture.GetValidCategoryDescription(); // aplicando melhorias com dados randômicos da lib Bogus
            Action action = () => new DomainEntity.Category(longName, validDescription);
            action.Should().Throw<EntityValidationException>().WithMessage(CategoryExceptionMessage.NameMaxLengthExceptionMessage);
        }


        [Fact(DisplayName = "Deve lançar exceção quando a descrição ultrapassar 10.000 caracteres")]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        public void Should_Throw_When_DescriptionIsGreaterThan10000Characters()
        {
            var longDescription = new string('D', 10_001);
            var validName = _fixture.GetValidCategoryName(); // Ddados randômicos com Bogus
            Action ACTION = () => new DomainEntity.Category(validName, longDescription);
            ACTION.Should().Throw<EntityValidationException>().WithMessage(CategoryExceptionMessage.DescriptionMaxLengthExceptionMessage);

        }
        #endregion

        #region Activation / Inactivation
        [Fact(DisplayName = "Deve ativar a categoria corretamente")]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        public void Should_Activate_Category()
        {
            var validCategory = _fixture.GetValidCategory();
            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);
            category.Activate();
            category.IsActive.Should().BeTrue();

        }

        [Fact(DisplayName = "Deve inativar a categoria corretamente")]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        public void Should_Inactivate_Category()
        {
            var validCategory = _fixture.GetValidCategory();
            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);
            category.Inactive();
            category.IsActive.Should().BeFalse();

        }
        #endregion

        #region Update tests
        [Fact(DisplayName = "Deve atualizar nome e descrição da categoria")]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        public void Should_Update_Name_And_Description()
        {
            var category = _fixture.GetValidCategory();
            var newData = new
            {
                Name = _fixture.GetValidCategoryName(),
                Description = _fixture.GetValidCategoryDescription()
            };

            category.Update(newData.Name, newData.Description);
            category.Name.Should().BeEquivalentTo(newData.Name);
            category.Description.Should().BeEquivalentTo(newData.Description);
        }

        [Fact(DisplayName = "Deve atualizar apenas o nome da categoria")]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        public void Should_Update_Only_Name()
        {
            var category = _fixture.GetValidCategory();
            var newName = _fixture.GetValidCategoryDescription();
            var currentDescription = category.Description;
            category.Update(newName);
            category.Name.Should().BeEquivalentTo(newName);
            category.Description.Should().BeEquivalentTo(currentDescription);

        }


        [Fact(DisplayName = "Deve atualizar apenas a descrição da categoria")]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        public void Should_Update_Only_Description()
        {
            var categoryNew = _fixture.GetValidCategory();
            var newDescription = _fixture.GetValidCategoryDescription();
            var currentName = categoryNew.Name;
            categoryNew.Update(null, newDescription);
            categoryNew.Description.Should().BeEquivalentTo(newDescription);
            categoryNew.Name.Should().BeEquivalentTo(currentName);


        }

        [Theory(DisplayName = "Não deve atualizar o nome quando for vazio ou nulo")]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void Should_Not_Update_Name_When_EmptyOrNull(string? name)
        {
            var category = _fixture.GetValidCategory();
            var oldName = category.Name;
            var currentDescriptio = category.Description;
            category.Update(name);
            category.Description.Should().BeEquivalentTo(currentDescriptio);
            category.Name.Should().NotBeEquivalentTo(name);
            category.Name.Should().BeEquivalentTo(oldName);



        }

        [Theory(DisplayName = "Não deve atualizar a descrição quando for vazia ou nula")]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void Should_Not_Update_Description_When_EmptyOrNull(string? description)
        {
            var category = _fixture.GetValidCategory();
            var oldDescription = category.Description;
            category.Update(null, description);
            category.Description.Should().BeEquivalentTo(oldDescription);
            category.Description.Should().NotBeEquivalentTo(description);
        }

        [Theory(DisplayName = "Deve lançar exceção ao atualizar com nome menor que 3 caracteres")]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        [MemberData(nameof(GetInvalidadeNameWithThan3Caharacters), parameters: 10)]
        public void Should_Throw_When_Update_With_Name_LessThanThreeCharacters(string? invalidName)
        {
            var category = _fixture.GetValidCategory();
            Action action = () => category.Update(invalidName);
            action.Should().Throw<EntityValidationException>().WithMessage(CategoryExceptionMessage.NameMinLengthExceptionMessageParam("Nome", 3));
        }

        [Fact(DisplayName = "Deve lançar exceção ao atualizar com nome maior que 255 caracteres")]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        public void Should_Throw_When_Update_With_Name_GreaterThan255Characters()
        {
            var category = _fixture.GetValidCategory();
            var longName = _fixture.Faker.Lorem.Letter(256);
            Action action = () => category.Update(longName);
            action.Should().Throw<EntityValidationException>().WithMessage(CategoryExceptionMessage.NameMaxLengthExceptionMessage);
        }

        [Fact(DisplayName = "Deve lançar exceção ao atualizar com descrição maior que 10.000 caracteres")]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        public void Should_Throw_When_Update_With_Description_GreaterThan10000Characters()
        {
            var category = _fixture.GetValidCategory();
            var longDescription = _fixture.Faker.Lorem.Letter(10_004);
            Action action = () => category.Update(null, longDescription);
            action.Should().Throw<EntityValidationException>().WithMessage(CategoryExceptionMessage.DescriptionMaxLengthExceptionMessage);

        }
        #endregion
    }
}
