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
                searchString = searchString.Replace(' ', '+');
                Console.WriteLine(searchString);
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://ws.spotify.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("search/1/track.json?q=" + searchString);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Hola");
                }
            }
            return View();
        }
    }
}