using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpotifyDataClient.Models
{
    public class Artist
    {
        public int artistID { get; set; }
        public string name { get; set; }

        public virtual ICollection<Album> albums { get; set; }
    }
}