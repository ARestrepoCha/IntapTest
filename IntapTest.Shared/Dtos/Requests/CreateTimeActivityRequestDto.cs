namespace IntapTest.Shared.Dtos.Requests
{
    public class CreateTimeActivityRequestDto
    {
        public Guid ActivityId { get; set; }
        public DateTime Date { get; set; }
        public double Hours { get; set; }
    }
}
