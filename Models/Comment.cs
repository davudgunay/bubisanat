using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace bubisanat.Models
{
    public class Comment
    {
        public long Id { get; set; }

        [Column(TypeName = "nchar(256)")]
        [Required(ErrorMessage = "Bu alan gerekli")]
        [StringLength(256, ErrorMessage = "En fazla 256 karakter")]
        [DisplayName("İçerik")]
        public string Content { get; set; }

        [DisplayName("Post")]
        public long PostId { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime TimeStamp { get; set; }

        [DisplayName("Post")]
        [ForeignKey("PostId")]
        public Post? Post { get; set; }
    }
}
