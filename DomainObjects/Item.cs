using DataAccess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainObjects
{
    public class Item : EntityBase, INamedEntity
    {
        [StringLength(450)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public bool Availablity { get; set; }

        public string IpV4 { get; set; }

        public string IpV6 { get; set; }

        public string ItemType { get; set; }

        public int AreaOfItemID { get; set; }

        public virtual Area AreaOfItem { get; set; }

        public virtual List<Memory> MemoriesOfItem { get; set; }
    }
}