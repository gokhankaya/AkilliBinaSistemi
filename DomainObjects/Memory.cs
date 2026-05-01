using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainObjects
{
    public class Memory : EntityBase
    {
        public int? AreaID { get; set; }

        public int? ItemID { get; set; }

        public string Definition { get; set; }

        public string ActionName { get; set; }

        public string ActionValue { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey("AreaID")]

        public virtual Area Area { get; set; }

        [ForeignKey("ItemID")]
        public virtual Item Item { get; set; }
    }
}
