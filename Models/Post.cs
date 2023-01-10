using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace bubisanat.Models
{
    public class Post
    {
        public long Id { get; set; }

        [Column(TypeName = "nchar(50)")]
        [Required(ErrorMessage = "Bu alan gerekli")]
        [DisplayName("Başlık")]
        [StringLength(50, ErrorMessage = "En fazla 50 karakter")]
        public string Title { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Kategori")]
        public short CategoryId { get; set; }

        [DisplayName("Önceki içerik")]
        public long? PreviousPostId { get; set; }

        [DisplayName("Sonraki içerik")]
        public long? NextPostId { get; set; }

        public long Likes { get; set; }

        public long DisplayCount { get; set; }

        [Column(TypeName = "nchar(100)")]
        [DisplayName("Etiketler")]
        [StringLength(100, ErrorMessage = "En fazla 100 karakter")]
        public string Tags { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Bu alan gerekli")]
        [DisplayName("İçerik")]
        public string Content { get; set; }

        public bool Deleted { get; set; }

        [DisplayName("Yazar")] 
        public string AuthorId { get; set; }

        [NotMapped]
        [DisplayName("Görsel")]
        public IFormFile? FormImage { get; set; }

        [Column(TypeName = "image")]
        public byte[]? Image { get; set; }

        [DisplayName("Kategori")]
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        [DisplayName("Önceki içerik")]
        [ForeignKey("PreviousPostId")]
        public Post? PreviousPost { get; set; }

        [DisplayName("Sonraki içerik")]
        [ForeignKey("NextPostId")]
        public Post? NextPost { get; set; }

        [DisplayName("Yazar")]
        [ForeignKey("AuthorId")]
        public ApplicationUser? Author { get; set; }

    }
}
