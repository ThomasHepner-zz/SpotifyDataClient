using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpotifyDataClient.Models
{
    public class Song
    {
        public int ID { get; set; }
        public string name { get; set; }
        public float popularity { get; set; }
        public float length { get; set; }

        public virtual Album album { get; set; }
    }
}