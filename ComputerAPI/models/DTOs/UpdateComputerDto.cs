namespace ComputerAPI.models.DTOs
{
    public class UpdateComputerDto
    {
        public string? Brand { get; set; }
        public string? Type { get; set; }
        public float Display { get; set; }
        public int Memory { get; set; }
        public Guid OsysId { get; set; }
    }
}
