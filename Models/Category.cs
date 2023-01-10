using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bubisanat.Models
{
    public class Category
    {
        public short id { get; set; }

        [Column(TypeName="nchar(50)")]
        [Required(ErrorMessage ="Bu alan gerekli")]
        [DisplayName("İsim")]
        [StringLength(50, ErrorMessage ="En fazla 50 karakter")]
        public string Name { get; set; }

        [Column(TypeName = "nchar(100)")]
        [DisplayName("Bilgi")]
        [StringLength(100, ErrorMessage = "En fazla 100 karakter")]
        public string? Info { get; set; }

        [DisplayName("Ana kategori (opsiyonel)")]
        public short? TopCategoryId { get; set; }

        [DisplayName("Ana kategori")]
        [ForeignKey("TopCategoryId")]
        public TopCategory? TopCategory { get; set; }
    }
}
