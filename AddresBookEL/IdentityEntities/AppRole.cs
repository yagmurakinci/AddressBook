using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddresBookEL.IdentityEntities
{
    public class AppRole:IdentityRole
    {
        public DateTime CreatedDate { get; set; }
        [StringLength(300,ErrorMessage ="Role adı mak 300 karakter olmalıdır!")]
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}
