using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.DTO.Material
{
    public class EditMaterialDTO
    {
        [Required]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? Saurce { get; set; }
        public int? Type { get; set; }
    }
}
