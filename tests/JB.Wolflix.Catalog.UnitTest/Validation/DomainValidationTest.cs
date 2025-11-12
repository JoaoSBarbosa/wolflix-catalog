using Bogus;
using FluentAssertions;
using JB.Wolflix.Catalog.Domain.Exceptions;
using JB.Wolflix.Catalog.Domain.Utils;
using JB.Wolflix.Catalog.Domain.Validation;
using JB.Wolflix.Catalog.UnitTest.Domain.Entity.Category;
using Xunit;

namespace JB.Wolflix.Catalog.UnitTest.Validation
{
    [Collection(nameof(CategoryTestFixture))]
    public class DomainValidationTest
    {
        private readonly Faker _faker = new();
        private readonly CategoryTestFixture _fixture;

        public DomainValidationTest(CategoryTestFixture categoryTestFixture)
        {
            _fixture = categoryTestFixture;
        }

        #region Data Providers

        public static IEnumerable<object[]> GetInvalidNames(int numberOfTests = 5)
        {
            var fixture = new CategoryTestFixture();

            for (int i = 0; i < numberOfTests; i++)
            {
                var isOdd = i % 2 == 1;
                yield return new object[] { fixture.GetValidCategoryName()[..(isOdd ? 1 : 2)] };
            }
        }

        public static IEnumerable<object[]> GetValuesSmallerThanMin(int numberOfTests = 5)
        {
            yield return new object[] { "123456", 10 };
            Faker faker = new Faker();

            for (int i = 0; i < numberOfTests; i++)
            {
                var example = faker.Commerce.ProductName();
                var minLength = example.Length + (new Random()).Next(1, 20);

                yield return new object[] { example, minLength };
            }
        }

        #endregion

        #region NotNull

        [Fact(DisplayName = "Não deve lançar exceção quando o campo não for nulo")]
        [Trait("Domain", "Validation - DomainValidation")]
        public void Should_NotThrow_When_FieldIsNotNull()
        {
            var name = _fixture.GetValidCategoryName();

            Action action = () => DomainValidation.NotNull(name, "Nome");

            action.Should().NotThrow();
        }

        [Fact(DisplayName = "Deve lançar exceção quando o campo for nulo")]
        [Trait("Domain", "Validation - DomainValidation")]
        public void Should_Throw_When_FieldIsNull()
        {
            string? name = null;

            Action action = () => DomainValidation.NotNull(name, "Nome");

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage(CategoryExceptionMessage.NameNullExceptionMessage);
        }

        #endregion

        #region NotNullOrEmpty

        [Theory(DisplayName = "Deve lançar exceção quando o campo for nulo ou vazio")]
        [Trait("Domain", "Validation - DomainValidation")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Should_Throw_When_FieldIsNullOrEmpty(string? value)
        {
            Action action = () => DomainValidation.NotNullOrEmpty(value, "Nome");

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage(CategoryExceptionMessage.NameNullExceptionMessage);
        }

        [Fact(DisplayName = "Não deve lançar exceção quando o campo tiver valor válido")]
        [Trait("Domain", "Validation - DomainValidation")]
        public void Should_NotThrow_When_FieldIsValid()
        {
            string name = _fixture.GetValidCategoryName();

            Action action = () => DomainValidation.NotNullOrEmpty(name, "Nome");

            action.Should().NotThrow();
        }

        #endregion

        #region MinLength

        [Theory(DisplayName = "Deve lançar exceção quando o valor for menor que o comprimento mínimo permitido")]
        [Trait("Domain", "Validation - DomainValidation")]
        [MemberData(nameof(GetValuesSmallerThanMin), parameters: 10)]
        public void Should_Throw_When_ValueIsSmallerThanMinLength(string value, int minLength)
        {
            Action action = () => DomainValidation.MinLength(value, "Nome", minLength);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage(CategoryExceptionMessage.NameMinLengthExceptionMessageParam("Nome", minLength));
        }

        [Fact(DisplayName = "Não deve lançar exceção quando o valor atender ao comprimento mínimo")]
        [Trait("Domain", "Validation - DomainValidation")]
        public void Should_NotThrow_When_ValueMeetsMinLength()
        {
            var name = _fixture.GetValidCategoryName();

            Action action = () => DomainValidation.MinLength(name, "Nome", 3);

            action.Should().NotThrow();
        }

        #endregion

        #region MaxLength

        [Fact(DisplayName = "Deve lançar exceção quando o nome ultrapassar 255 caracteres")]
        [Trait("Domain", "Validation - DomainValidation")]
        public void Should_Throw_When_NameExceeds255Characters()
        {
            var invalidName = new string('a', 256);

            Action action = () => DomainValidation.MaxLength(invalidName, "Nome");

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage(CategoryExceptionMessage.NameMaxLengthExceptionMessage);
        }

        [Fact(DisplayName = "Não deve lançar exceção quando o nome tiver até 255 caracteres")]
        [Trait("Domain", "Validation - DomainValidation")]
        public void Should_NotThrow_When_NameIsWithin255Characters()
        {
            var validName = _fixture.GetValidCategoryName();

            Action action = () => DomainValidation.MaxLength(validName, "Nome");

            action.Should().NotThrow();
        }

        #endregion

        #region MaxLengthDescription

        [Fact(DisplayName = "Deve lançar exceção quando a descrição ultrapassar 10.000 caracteres")]
        [Trait("Domain", "Validation - DomainValidation")]
        public void Should_Throw_When_DescriptionExceeds10000Characters()
        {
            var invalidDescription = new string('a', 10001);

            Action action = () => DomainValidation.MaxLengthDescription(invalidDescription, "Descrição");

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage(CategoryExceptionMessage.DescriptionMaxLengthExceptionMessage);
        }

        [Fact(DisplayName = "Não deve lançar exceção quando a descrição tiver até 10.000 caracteres")]
        [Trait("Domain", "Validation - DomainValidation")]
        public void Should_NotThrow_When_DescriptionIsWithinLimit()
        {
            var validDescription = _fixture.GetValidCategoryDescription();

            Action action = () => DomainValidation.MaxLengthDescription(validDescription, "Descrição");

            action.Should().NotThrow();
        }

        #endregion
    }
}
