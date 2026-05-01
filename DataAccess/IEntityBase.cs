using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IEntityBase
    {
        [Key]
        int ID { get; set; }
        string CreatedBy { get; set; }
        DateTime? CreatedDate { get; set; }
        string DeletedBy { get; set; }
        DateTime? DeletedDate { get; set; }
        string ModifiedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
    }
}
