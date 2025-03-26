using IceCreamCompany.Domain.Interfaces.Abstract;

namespace IceCreamCompany.Domain.Entities.Abstract
{
    public abstract class Entity : IEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
