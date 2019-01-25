using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSongList.Models.Entities
{
    public class Era
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Display(Name = "Era")]
        public int Id { get; set; }
        [Required]
        public string Label { get; set; }
        [Required, Display(Name = "Sort Order")]
        public int SortOrder { get; set; }
        [Display(Name = "Color")]
        public string Color { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
    }
}
