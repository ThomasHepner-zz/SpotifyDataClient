using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpotifyDataClient.Models
{
    public class Song
    {
        public int SongID { get; set; }
        public string Name { get; set; }
        public float Popularity { get; set; }
        public float Length { get; set; }

        public virtual Album Album { get; set; }
    }
}