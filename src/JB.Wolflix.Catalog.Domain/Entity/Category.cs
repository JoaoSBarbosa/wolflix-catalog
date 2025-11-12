
using JB.Wolflix.Catalog.Domain.Exceptions;
using JB.Wolflix.Catalog.Domain.SeedWork;
using JB.Wolflix.Catalog.Domain.Utils;
using JB.Wolflix.Catalog.Domain.Validation;
using System.ComponentModel.DataAnnotations;

namespace JB.Wolflix.Catalog.Domain.Entity
{
    public class Category: AggregateRoot
    {
        public Category(string name, string description):base()
        {
            Name = name;
            Description = description;

            CreatedAt = DateTime.Now;
            IsActive = true;
            validate();
        }

        public Category( string name, string description, bool isActive)
        {
           
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            IsActive = isActive;
            Description = description;
            Name = name;
            validate();
        }

        public string Name { get; private set; }
        public string Description { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public bool IsActive { get; private set; }

        public void Activate()
        {
            IsActive = true;
            validate();
        }
        public void Inactive()
        {
            IsActive = false;
            validate();
        }
        public void Update(string? name = null, string? description = null)
        {
            if (!string.IsNullOrWhiteSpace(name))
                Name = name;

            if (!string.IsNullOrWhiteSpace(description))
                Description = description;

            validate();
        }

        private void validate()
        {
            DomainValidation.NotNullOrEmpty(Name, "Nome");
            DomainValidation.NotNullOrEmpty(Description, "Descrição");
            DomainValidation.MinLength(Name, "Nome", 3);
            DomainValidation.MaxLength(Name, "Nome");
            DomainValidation.MaxLengthDescription(Description, "Descrição");
            //if (String.IsNullOrWhiteSpace(Name)) throw new EntityValidationException(CategoryExceptionMessage.NameNullExceptionMessage);
            //if ( Description == null) throw new EntityValidationException(CategoryExceptionMessage.DescriptionNullExceptionMessage);
            //if (Name.Length < 3) throw new EntityValidationException(CategoryExceptionMessage.NameMinLengthExceptionMessage);
            //if (Name.Length > 255) throw new EntityValidationException(CategoryExceptionMessage.NameMaxLengthExceptionMessage);
            //if (Description.Length > 10000) throw new EntityValidationException(CategoryExceptionMessage.DescriptionMaxLengthExceptionMessage);
        }

    }
}
