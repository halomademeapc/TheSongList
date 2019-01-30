using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSongList.Models.Entities
{
    public class Season
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Display(Name = "Season")]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, Display(Name = "Sort Order")]
        public int SortOrder { get; set; }

        public virtual ICollection<Episode> Episodes { get; set; }
    }
}
