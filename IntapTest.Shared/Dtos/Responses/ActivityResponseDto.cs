namespace IntapTest.Shared.Dtos.Responses
{
    public class ActivityResponseDto
    {
        public Guid Id { get; set; }
        public string? Descripcion { get; set; }
        public List<TimeActivityDto>? TimeActivities { get; set; }
    }
}
