using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddresBookEL.Entities
{
    [Table("Neighborhoods")]
    public class Neighborhood:BaseEntity<int>
    {
        [Required]
        [StringLength(50,MinimumLength =2,ErrorMessage ="Mahalle adı min 2 mak 50 karakter olmalıdır!")]
        public string Name { get; set; }
        public short DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }

    }
}
