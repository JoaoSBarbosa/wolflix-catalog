
namespace JB.Wolflix.Catalog.Domain.SeedWork
{
    public abstract class Entity
    {


        public Entity() => Id = Guid.NewGuid();
        public Guid Id { get; protected set; }
    }
}
