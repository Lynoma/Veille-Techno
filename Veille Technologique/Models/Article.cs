using System.ComponentModel.DataAnnotations;

namespace Veille_Technologique.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Url { get; set; }
        public string? Image_Url { get; set; }
        public ArticleState State { get; set; }
    }

    public enum ArticleState { ToRead, Discarded, Saved }
}
