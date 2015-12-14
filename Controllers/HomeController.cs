using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotifyDataClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
            if (!String.IsNullOrEmpty(searchString)) {
                searchString = searchString.TrimEnd(' ').Replace(' ', '+');
            }
            JObject jsonResponse = null;
            spotifyCall(jsonResponse, searchString);

            if (jsonResponse == null || (int)jsonResponse["info"]["num_results"] == 0)
            {
                ViewBag.Message = "No results found";
                return View();
            }

            var artistTracks = jsonResponse["tracks"];
            int numberOfTracks = (int) jsonResponse["info"]["num_results"] > (int) jsonResponse["info"]["limit"] ? (int) jsonResponse["info"]["limit"] : (int)jsonResponse["info"]["num_results"];
            for (int i = 0; i < numberOfTracks; i++)
            {
                string albumName = (string) artistTracks[i]["album"]["name"];
                //TODO: save the album as a EF record.
            }
            return View();
        }

        private async void spotifyCall(JObject jsonResponse, string searchString)
        {
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
        }
    }
}