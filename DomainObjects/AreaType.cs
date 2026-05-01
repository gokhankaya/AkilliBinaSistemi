using DataAccess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainObjects
{
    public class AreaType : EntityBase, INamedEntity
    {
        [StringLength(450)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public string Definition { get; set; }

        public ICollection<Area> Areas { get; set; }
    }
}