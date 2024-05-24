using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.DTO
{
    public class CourceDTO
    {
        [Required]
        [Range(1e5,1e6-1, ErrorMessage = "Id must be 6 digits")]
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        
        public IFormFile Image { get; set; }
        public IFormFile Logo { get; set; }
        public int type { get; set; }

    }
    

}
