using IntapTest.Data.Base;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IntapTest.Data.Entities
{
    public class User : IdentityUser<Guid>, IBase
    {
        [MaxLength(150)]
        public string? FullName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Activity> Activities { get; set; } = new List<Activity>();

    }
}
