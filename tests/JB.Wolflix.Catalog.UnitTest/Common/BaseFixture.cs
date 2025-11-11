using Bogus;

namespace JB.Wolflix.Catalog.UnitTest.Common
{
    public abstract class BaseFixture
    {
        protected BaseFixture() => Faker = new Faker("pt_BR");

        public Faker Faker { get; set; }
    }
}
