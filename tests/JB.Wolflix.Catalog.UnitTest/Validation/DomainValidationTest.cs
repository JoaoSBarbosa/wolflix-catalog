using Bogus;
using FluentAssertions;
using JB.Wolflix.Catalog.Domain.Exceptions;
using JB.Wolflix.Catalog.Domain.Utils;
using JB.Wolflix.Catalog.Domain.Validation;
using JB.Wolflix.Catalog.UnitTest.Domain.Entity.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JB.Wolflix.Catalog.UnitTest.Validation
{
    [Collection(nameof(CategoryTestFixture))]
    public class DomainValidationTest
    {
        private Faker _Faker = new Faker();

        private readonly CategoryTestFixture _Fixture;
        public DomainValidationTest(CategoryTestFixture categoryTestFixture)
        {
            _Fixture = categoryTestFixture;
        }

        public static IEnumerable<object[]> GetInvalidNames(int numerOfTest)
        {
            var fix = new CategoryTestFixture();

            for (int i = 0; i < numerOfTest; i++)
            {
                var isOdd = i % 2 == 1;
                yield return new object[] { fix.GetValidCategoryName()[..(isOdd ? 1 : 2)] };
            }
        }


        [Fact(DisplayName = nameof(NotThrowExceptionIfFieldNotEmpty))]
        [Trait("Domain", "Validation - DomainValidation")]
        public void NotThrowExceptionIfFieldNotEmpty()
        {
            var name = _Fixture.GetValidCategoryName();

            Action action = () => DomainValidation.NotNull(name, "Nome");
            action.Should().NotThrow();
        }

        [Fact(DisplayName = nameof(ShouldThrowExceptionIfFieldIsNull))]
        [Trait("Domain", "Validation - DomainValidation")]
        public void ShouldThrowExceptionIfFieldIsNull()
        {
            string? name = null;
            Action action = () => DomainValidation.NotNull(name, "Nome");
            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage(CategoryExceptionMessage.NameNullExceptionMessage);

        }

        [Theory(DisplayName = nameof(ShouldThrowExceptionIfFieldIsNullOrEmpty))]
        [Trait("Domain", "Validation - DomainValidation")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ShouldThrowExceptionIfFieldIsNullOrEmpty(string? value)
        {
            Action action = () => DomainValidation.NotNullOrEmpty(value, "Nome");
            action.Should().Throw<EntityValidationException>().WithMessage(CategoryExceptionMessage.NameNullExceptionMessage);
        }

        [Fact(DisplayName = nameof(NotExceptionShouldNotBeThrownIfTheFieldIsNotEmptyOrNull))]
        [Trait("Domain", "Validation - DomainValidation")]
        public void NotExceptionShouldNotBeThrownIfTheFieldIsNotEmptyOrNull()
        {
            string name = _Fixture.GetValidCategoryName();
            Action action = () => DomainValidation.NotNullOrEmpty(name, "Nome");
            action.Should().NotThrow();
        }

        [Fact(DisplayName = nameof(ShouldNotThrowExceptioWhenNameHasNoMoreThanThreeCharacters))]
        [Trait("Domain", "Validation - DomainValidation")]
        public void ShouldNotThrowExceptioWhenNameHasNoMoreThanThreeCharacters()
        {
            var name = _Fixture.GetValidCategoryName();
            Action action = () => DomainValidation.NotNullOrEmpty(name, "Nome");
            action.Should().NotThrow();
        }

        [Theory(DisplayName = nameof(ShouldThrowExceptioWhenNameIsLessThreeCharacters))]
        [Trait("Domain", "Validation - DomainValidation")]
        [MemberData(nameof(GetInvalidNames), parameters: 5)]
        public void ShouldThrowExceptioWhenNameIsLessThreeCharacters(string name)
        {
            Action action = () => DomainValidation.MinLength(name, "Nome");
            action.Should().Throw<EntityValidationException>().WithMessage(CategoryExceptionMessage.NameMinLengthExceptionMessage);
        }

        [Fact(DisplayName = nameof(ShouldExceptionThrowWhenNameIsLogerThan255Characters))]
        [Trait("Domain", "Validation - DomainValidation")]
        public void ShouldExceptionThrowWhenNameIsLogerThan255Characters()
        {
            var invalidName = new String('a', 256);

            Action action = () => DomainValidation.MaxLength(invalidName, "Nome");
            action.Should().Throw<EntityValidationException>().WithMessage(CategoryExceptionMessage.NameMaxLengthExceptionMessage);
        }

        [Fact(DisplayName = nameof(DoNotShouldExceptionThrowWhenNameIsLogerThan255Characters))]
        [Trait("Domain", "Validation - DomainValidation")]
        public void DoNotShouldExceptionThrowWhenNameIsLogerThan255Characters()
        {
            var validName = _Fixture.GetValidCategoryName();

            Action action = () => DomainValidation.MaxLength(validName, "Nome");
            action.Should().NotThrow();
        }

        [Fact(DisplayName = nameof(ShouldErrorWhenDescriptionIsGreaterThan10000Characters))]
        [Trait("Domain", "Validation - DomainValidation")]
        public void ShouldErrorWhenDescriptionIsGreaterThan10000Characters()
        {
            var invalidDescription = new String('a', 10001);
            Action action = () => DomainValidation.MaxLengthDescription(invalidDescription, "Descrição");
            action.Should().Throw<EntityValidationException>().WithMessage(CategoryExceptionMessage.DescriptionMaxLengthExceptionMessage);
        }

        [Fact(DisplayName = nameof(DoNotShouldErrorWhenDescriptionIsGreaterThan10000Characters))]
        [Trait("Domain", "Validation - DomainValidation")]
        public void DoNotShouldErrorWhenDescriptionIsGreaterThan10000Characters()
        {
            var validateDescriptio = _Fixture.GetValidCategoryDescription();
            Action action = () => DomainValidation.MaxLengthDescription(validateDescriptio, "Descrição");
            action.Should().NotThrow();
        }
    }
}
