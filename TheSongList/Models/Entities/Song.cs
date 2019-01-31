using System.Collections.Generic;
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
        [Required, Display(Name = "Artist")]
        public int ArtistId { get; set; }
        [Display(Name = "Era")]
        public int? EraId { get; set; }

        [ForeignKey("ArtistId")]
        public virtual Artist Artist { get; set; }
        [ForeignKey("EraId")]
        public virtual Era Era { get; set; }
        public virtual ICollection<Appearance> Appearances { get; set; }
    }
}
