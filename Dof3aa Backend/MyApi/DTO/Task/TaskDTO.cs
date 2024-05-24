namespace PresentationLayer.DTO.Task
{
    public class TaskDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DeadLine { get; set; }
        public IFormFile? Saurce { get; set; }
        public int CourceId { get; set; }

    }
}
