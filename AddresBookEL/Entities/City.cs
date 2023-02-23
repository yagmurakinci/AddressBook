using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddresBookEL.Entities
{
    [Table("Cities")]
    public class City:BaseEntity<sbyte>
    {
        [Required]
        [StringLength(50,MinimumLength =2,ErrorMessage ="İl adı min 2 mak 50 karakter olmalıdır!")]
        public string Name { get; set; }
    }
}
