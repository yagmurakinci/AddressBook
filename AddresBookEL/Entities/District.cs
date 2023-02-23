using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddresBookEL.Entities
{
    [Table("Districts")]
    public class District:BaseEntity<short>
    {
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "İlçe adı min 2 mak 50 karakter olmalıdır!")]
        public string Name { get; set; }

        //ilçe ile bağlı olduğu için ilişki kuralım
        public sbyte CityId { get; set; }
        [ForeignKey("CityId")]
        public virtual City City { get; set; }

    }
}
