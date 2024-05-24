namespace PresentationLayer.DTO
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string ImageUrl { get; set; }
        public dynamic Groups { get; set; }
    }
}
