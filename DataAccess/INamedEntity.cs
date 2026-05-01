using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess
{
    public interface INamedEntity
    {
        [StringLength(450)]
        [Index(IsUnique = true)]
        string Name { get; set; }
    }
}
