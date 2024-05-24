namespace PresentationLayer.DTO.Task
{
    public class TaskInfo
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DeadLine { get; set; }
        public string? SaurceUrl { get; set; }
        public int CourceId { get; set; }
        public string PublisherUserName { get; set; }
        public int Id { get; set; }
        public DateTime PublishDate { get; set; }

    }
}
