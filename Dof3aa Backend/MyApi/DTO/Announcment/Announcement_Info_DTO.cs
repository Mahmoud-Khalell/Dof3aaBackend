namespace PresentationLayer.DTO.Announcment
{
    public class Announcement_Info_DTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CourceId { get; set; }
        public DateTime CreationDate { get; set; }
        public int Id { get; set; }
        public string PublisherUserName { get; set; }

        public Announcement_Info_DTO()
        {

        }
    }
}
