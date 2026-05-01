using DataAccess;
using System;

namespace DomainObjects
{
    public class EntityBase : IEntityBase
    {
        public int ID { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
