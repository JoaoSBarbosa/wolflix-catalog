using FluentAssertions;
using JB.Wolflix.Catalog.Domain.Exceptions;
using JB.Wolflix.Catalog.Domain.Utils;
using JB.Wolflix.Catalog.UnitTest.Common;
using System;
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

            for (int i =0; i < numerOfTest; i++)
            {
                var isOdd = i % 2 == 1;
                yield return new object[] { fix.GetValidCategoryName()[..(isOdd ? 1 : 2)] };

            }
            // A palavra-chave 'yield' permite retornar cada valor um de cada vez
            //yield return new object[] { "12" };
            //yield return new object[] { "1" };
            //yield return new object[] { "a" };
            //yield return new object[] { "b" };
            //yield return new object[] { "ca" };
            //yield return new object[] { "Gu" };
            //yield return new object[] { "XP" };
        }
        // =====================================================================
        // 🧩 SOBRE O [Fact]
        // ---------------------------------------------------------------------
        // - É um atributo do framework xUnit que marca o método como teste unitário.
        // - Não recebe parâmetros de entrada (valores para o método), 
        //   apenas propriedades de configuração:
        //      • DisplayName → nome exibido no relatório
        //      • Skip        → ignora o teste (ex: ainda não implementado)
        //      • Timeout     → tempo máximo (em milissegundos)
        // =====================================================================

        [Fact(DisplayName = nameof(Instantiate))]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)] // Agrupa/classifica o teste por domínio
        public void Instantiate()
        {
            // ================================================================
            // 🔹 ARRANGE → Prepara o cenário do teste
            // ------------------------------------------------
            // Cria um objeto anônimo contendo dados válidos para a categoria.
            // ================================================================
            var validCategory = _fixture.GetValidCategory();

            // Captura o tempo antes e depois da criação, para validar CreatedAt
            var dateTimeBefore = DateTime.Now;

            // ================================================================
            // 🔹 ACT → Executa a ação a ser testada
            // ------------------------------------------------
            // Instancia uma nova categoria com nome e descrição válidos.
            // ================================================================
            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);

            var dateTimeAfter = DateTime.Now.AddSeconds(1);

            // ================================================================
            // 🔹 ASSERT → Verifica se o resultado é o esperado
            // ------------------------------------------------
            // Aqui conferimos se o objeto foi criado corretamente e se as
            // propriedades automáticas (Id, CreatedAt, IsActive) estão válidas.
            // ================================================================


            /* ---- Validações com Fluent Assertions */

            category.Should().NotBeNull();
            category.Name.Should().Be(validCategory.Name);
            category.Description.Should().Be(validCategory.Description);
            category.Id.Should().NotBeEmpty();
            category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
            (category.CreatedAt > dateTimeBefore).Should().BeTrue();
            (category.CreatedAt < dateTimeAfter).Should().BeTrue();
            category.IsActive.Should().BeTrue();

            /* ---- Validações com Assert */
            //Assert.NotNull(category);
            //Assert.Equal(validData.Name, category.Name);
            //Assert.Equal(validData.Description, category.Description);
            //Assert.NotEqual(default(Guid), category.Id);
            //Assert.NotEqual(default(DateTime), category.CreatedAt);

            // --- Verifica se CreatedAt está dentro do intervalo de tempo correto ---
            //Assert.True(category.CreatedAt > dateTimeBefore);
            //Assert.True(category.CreatedAt < dateTimeAfter);
            //Assert.True(category.IsActive);
        }



        [Theory(DisplayName = nameof(InstantiateWithIsActiveStatus))]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        [InlineData(true)]   // Caso 1 → Categoria ativa
        [InlineData(false)]  // Caso 2 → Categoria inativa
        public void InstantiateWithIsActiveStatus(bool isActive)
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

            //Assert.NotNull(category);
            //Assert.Equal(validData.Name, category.Name);
            //Assert.Equal(validData.Description, category.Description);
            //Assert.NotEqual(default(Guid), category.Id);
            //Assert.NotEqual(default(DateTime), category.CreatedAt);
            //Assert.True(category.CreatedAt > dateTimeBefore);
            //Assert.True(category.CreatedAt < dateTimeAfter);

            // --- Verifica se o status de IsActive foi atribuído corretamente ---
            //Assert.Equal(isActive, category.IsActive);
        }



        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("      ")]
        public void InstantiateErrorWhenNameIsEmpty(string? value)
        {

            Action action = () => new DomainEntity.Category(value!, _fixture.GetValidCategoryDescription());

            /*  Teste usando Xunit Assertion */
            /* var exception = Assert.Throws<EntityValidationException>(action);
             * Assert.Equal("Nome não deve ser vazio ou nulo", exception.Message); */
            //Assert->Verifica se a ação lança a exceção esperada
            action.Should().Throw<EntityValidationException>().WithMessage(CategoryExceptionMessage.NameNullExceptionMessage);
        }

        [Fact(DisplayName = nameof(InsertiateErrorWhenDescriptionIsNull))]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        public void InsertiateErrorWhenDescriptionIsNull()
        {
            Action action = () => new DomainEntity.Category(_fixture.GetValidCategoryName(), null!);
            // Testes utilizando Assertion Xunit
            /*var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Descrição não deve ser vazio ou nulo", exception.Message);*/

            // Teste utilizando FluentAssertion
            action.Should().Throw<EntityValidationException>().WithMessage(CategoryExceptionMessage.DescriptionNullExceptionMessage);

        }


        [Theory(DisplayName = nameof(InsertationErrorWhenNameIsLessThanThreeCharacters))]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        [MemberData(nameof(GetInvalidadeNameWithThan3Caharacters), parameters: 5)]
        public void InsertationErrorWhenNameIsLessThanThreeCharacters(string invalidName)
        {

            Action action = () => _fixture.GetValidCategoryByParam(invalidName);
            // TESTE PADRÃO
            //var exception = Assert.Throws<EntityValidationException>(action);
            //Assert.Equal("Nome deve ter um tamanho mínimo de três caracteres.", exception.Message);

            // Teste usando FluentAssertion
            action.Should().Throw<EntityValidationException>().WithMessage(CategoryExceptionMessage.NameMinLengthExceptionMessage);

        }


        [Fact(DisplayName = nameof(InsertationErrorWhenNameIsGreaterThan255Characters))]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        public void InsertationErrorWhenNameIsGreaterThan255Characters()
        {
            var longName = new string('A', 256);
            var validDescription = _fixture.GetValidCategoryDescription(); // aplicando melhorias com dados randômicos da lib Bogus
            Action action = () => new DomainEntity.Category(longName, validDescription);

            //var exception = Assert.Throws<EntityValidationException>(action);
            //Assert.Equal("Nome deve ter um tamanho máximo de 255 caracteres.", exception.Message);

            action.Should().Throw<EntityValidationException>().WithMessage(CategoryExceptionMessage.NameMaxLengthExceptionMessage);
        }


        [Fact(DisplayName = nameof(InsertationErrorWhenDescriptionIsGreaterThan10000Characters))]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        public void InsertationErrorWhenDescriptionIsGreaterThan10000Characters()
        {
            var longDescription = new string('D', 10_001);
            var validName = _fixture.GetValidCategoryName(); // Ddados randômicos com Bogus
            Action ACTION = () => new DomainEntity.Category(validName, longDescription);
            //var exception = Assert.Throws<EntityValidationException>(ACTION);
            //Assert.Equal("Descrição deve ter um tamanho máximo de 10.000 caracteres.", exception.Message);
            ACTION.Should().Throw<EntityValidationException>().WithMessage(CategoryExceptionMessage.DescriptionMaxLengthExceptionMessage);

        }


        [Fact(DisplayName = nameof(Activate))]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        public void Activate()
        {
            var validCategory = _fixture.GetValidCategory();


            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);
            category.Activate();
            //Assert.True(category.IsActive);
            category.IsActive.Should().BeTrue();

        }

        [Fact(DisplayName = nameof(Inactive))]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        public void Inactive()
        {


            var validCategory = _fixture.GetValidCategory();

            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);

            category.Inactive();

            //Assert.False(category.IsActive);
            category.IsActive.Should().BeFalse();

        }


        [Fact(DisplayName = nameof(Update))]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        public void Update()
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

            //Assert.Equal(newData.Name, category.Name);
            //Assert.Equal(newData.Description, category.Description);


        }

        [Fact(DisplayName = nameof(UpdateOnlyName))]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        public void UpdateOnlyName()
        {

            var category = _fixture.GetValidCategory();
            var newName = _fixture.GetValidCategoryDescription();

            var currentDescription = category.Description;

            category.Update(newName);

            //Assert.Equal(newName, category.Name);
            //Assert.Equal(currentDescription, category.Description);

            category.Name.Should().BeEquivalentTo(newName);
            category.Description.Should().BeEquivalentTo(currentDescription);

        }


        [Fact(DisplayName = nameof(UpdateOnlyDescription))]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        public void UpdateOnlyDescription()
        {
            var categoryNew = _fixture.GetValidCategory();
            var newDescription = _fixture.GetValidCategoryDescription();
            var currentName = categoryNew.Name;

            categoryNew.Update(null, newDescription);
            //Assert.Equal(newDescription, categoryNew.Description);
            //Assert.Equal(currentName, categoryNew.Name);

            categoryNew.Description.Should().BeEquivalentTo(newDescription);
            categoryNew.Name.Should().BeEquivalentTo(currentName);


        }

        [Theory(DisplayName = nameof(UpdateShouldNotSaveNameIEmpty))]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void UpdateShouldNotSaveNameIEmpty(string? name)
        {

            var category = _fixture.GetValidCategory();
            var oldName = category.Name;
            var currentDescriptio = category.Description;
            category.Update(name);

            //Assert.Equal(currentDescriptio, category.Description);
            //Assert.NotEqual(name, category.Name);
            //Assert.Equal(oldName, category.Name);


            category.Description.Should().BeEquivalentTo(currentDescriptio);
            category.Name.Should().NotBeEquivalentTo(name);
            category.Name.Should().BeEquivalentTo(oldName);



        }

        [Theory(DisplayName = nameof(UpdateShouldNotSaveDescriptionIEmpty))]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void UpdateShouldNotSaveDescriptionIEmpty(string? description)
        {
            var category = _fixture.GetValidCategory();
            var oldDescription = category.Description;
            category.Update(null, description);

            //Assert.Equal(oldDescription, category.Description);
            //Assert.NotEqual(description, category.Description);
            //Assert.Equal("Teste 1", category.Name);

            category.Description.Should().BeEquivalentTo(oldDescription);
            category.Description.Should().NotBeEquivalentTo(description);
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        [MemberData(nameof(GetInvalidadeNameWithThan3Caharacters), parameters:10)]
        public void UpdateErrorWhenNameIsLessThan3Characters(string? invalidName)
        {
            var category = _fixture.GetValidCategory();
            Action action = () => category.Update(invalidName);

            //var exception = Assert.Throws<EntityValidationException>(action);
            //Assert.Equal("Nome deve ter um tamanho mínimo de três caracteres.", exception.Message);

            action.Should().Throw<EntityValidationException>().WithMessage(CategoryTestFixture.NameMinLengthExceptionMessage);
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        public void UpdateErrorWhenNameIsGreaterThan255Characters()
        {
            var category = _fixture.GetValidCategory();
            var longName = _fixture.Faker.Lorem.Letter(256);
            Action action = () => category.Update(longName);
            //var exception = Assert.Throws<EntityValidationException>(action);
            //Assert.Equal("Nome deve ter um tamanho máximo de 255 caracteres.", exception.Message);

            action.Should().Throw<EntityValidationException>().WithMessage(CategoryTestFixture.NameMaxLengthExceptionMessage);
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10000Characters))]
        [Trait(TestCategories.Domain, TestCategories.CategoryAggregate)]
        public void UpdateErrorWhenDescriptionIsGreaterThan10000Characters()
        {
            var category = _fixture.GetValidCategory();
            var longDescription = _fixture.Faker.Lorem.Letter(10_004);
            Action action = () => category.Update(null, longDescription);
            //var message = Assert.Throws<EntityValidationException>(action);
            //Assert.Equal("Descrição deve ter um tamanho máximo de 10.000 caracteres.", message.Message);

            action.Should().Throw<EntityValidationException>().WithMessage(CategoryTestFixture.DescriptionMaxLengthExceptionMessage);

        }

    }
}
