using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSongList.Models.Entities
{
    public class Episode
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Display(Name = "Episode")]
        public int Id { get; set; }
        [Required, Display(Name = "Number")]
        public int EpisodeNumber { get; set; }
        public string Name { get; set; }
        [Required, Display(Name = "Season")]
        public int SeasonId { get; set; }

        public virtual ICollection<Appearance> Appearances { get; set; }
        public virtual Season Season { get; set; }
    }
}
