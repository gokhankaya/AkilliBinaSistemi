using DataAccess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace DomainObjects
{
    public class Area : EntityBase, INamedEntity
    {
        [StringLength(450)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public int? AreaID { get; set; }

        public int? AreaTypeID { get; set; }

        [ForeignKey("AreaTypeID")]
        public virtual AreaType AreaType { get; set; }

        [ForeignKey("AreaID")]
        public virtual Area RootArea { get; set; }

        public virtual ICollection<Item> Items { get; private set; }

        public virtual ICollection<Area> SubAreas { get; set; }

        public virtual ICollection<Memory> MemoriesOfArea { get; set; }
    }
}
