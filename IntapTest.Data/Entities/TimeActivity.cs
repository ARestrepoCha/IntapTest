using IntapTest.Data.Base;

namespace IntapTest.Data.Entities
{
    public class TimeActivity : BaseEntity
    {
        public DateTime Date { get; set; }
        public double Hours { get; set; }

        public Guid AcitvityId { get; set; }
        public Activity? Activity { get; set; }
    }
}
