using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotifyDataClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SpotifyDataClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async System.Threading.Tasks.Task<ActionResult> SearchResults(string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.TrimEnd(' ').Replace(' ', '+');
            }
            JObject jsonResponse = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://ws.spotify.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage spotifyResponse = await client.GetAsync("search/1/track.json?q=" + searchString);
                if (spotifyResponse.IsSuccessStatusCode)
                {
                    var jsonString = await spotifyResponse.Content.ReadAsStringAsync();
                    jsonResponse = JObject.Parse(jsonString);
                }
            }

            if (jsonResponse == null || (int)jsonResponse["info"]["num_results"] == 0)
            {
                ViewBag.Message = "No results found";
                return View();
            }

            var artistTracks = jsonResponse["tracks"];
            int numberOfTracks = (int)jsonResponse["info"]["num_results"] > (int)jsonResponse["info"]["limit"] ? (int)jsonResponse["info"]["limit"] : (int)jsonResponse["info"]["num_results"];
            string albumName, albumReleaseDate, trackName;
            float trackPopularity, trackLength;
            for (int i = 0; i < numberOfTracks; i++)
            {
                albumName = (string)artistTracks[i]["album"]["name"];
                albumReleaseDate = (string)artistTracks[i]["album"]["released"];
                //TODO: save the album as a EF record, if it hasn't been saved yet.
                trackName = (string)artistTracks[i]["name"];
                trackPopularity = (float)artistTracks[i]["popularity"];
                trackLength = (float)artistTracks[i]["length"];
                //TODO: save the song as a EF record, it it hasn't been saved yet.
            }
            return View();
        }
    }
}