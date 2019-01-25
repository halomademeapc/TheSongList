using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSongList.Models.Entities
{
    public class Song
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int ArtistId { get; set; }
        public int? EraId { get; set; }

        [ForeignKey("ArtistId"), Display(Name = "Artist")]
        public virtual Artist Artist { get; set; }
        [ForeignKey("EraId"), Display(Name = "Era")]
        public virtual Era Era { get; set; }
    }
}
