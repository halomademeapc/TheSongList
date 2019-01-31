using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheSongList.Models.Entities
{
    public class Episode
    {
        [Key, Display(Name = "Episode Number")]
        public int Id { get; set; }
        [Required, Display(Name = "Number in Season")]
        public int EpisodeNumber { get; set; }
        public string Name { get; set; }
        [Required, Display(Name = "Season")]
        public int SeasonId { get; set; }
        [Display(Name="Directed By")]
        public string Director { get; set; }
        [Display(Name="Written By")]
        public string Writer { get; set; }
        [Display(Name="Original Air Date")]
        public string AirDate { get; set; }
        [Display(Name="Production Code")]
        public string ProdCode { get; set; }

        public virtual ICollection<Appearance> Appearances { get; set; }
        public virtual Season Season { get; set; }
    }
}
