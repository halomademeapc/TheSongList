using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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

        [ForeignKey("ArtistId")]
        public virtual Artist Artist { get; set; }
        [ForeignKey("EraId")]
        public virtual Era Era { get; set; }
    }
}
