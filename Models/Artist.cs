using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpotifyDataClient.Models
{
    public class Artist
    {
        public int AlbumID { get; set; }
        public string Name { get; set; }
        public int ReleaseYear { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
    }
}