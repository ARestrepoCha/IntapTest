using IntapTest.Data.Base;

namespace IntapTest.Data.Entities
{
    public class Activity : BaseEntity
    {
        public string Descripcion { get; set; } = string.Empty;

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public ICollection<TimeActivity> TimeActivities { get; set; } = new HashSet<TimeActivity>();
    }
}
