using System.ComponentModel.DataAnnotations;

namespace IntapTest.Data.Base
{
    public interface IBase
    {
        [Key]
        Guid Id { get; set; }
        DateTime CreatedOn { get; set; }
        DateTime? LastModifiedOn { get; set; }
        bool IsDeleted { get; set; }
        Guid CreatedBy { get; set; }
        Guid? LastModifiedBy { get; set; }
        bool IsActive { get; set; }
    }
}
