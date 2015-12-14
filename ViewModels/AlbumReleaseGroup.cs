using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SpotifyDataClient.ViewModels
{
    public class AlbumReleaseGroup
    {
        public string albumName { get; set; }
        public int releaseYear { get; set; }
        public string longestTrackName { get; set; }
        public float longestTrackLength { get; set; }
        public float averageTrackPopularity { get; set; }
    }
}