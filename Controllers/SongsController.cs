using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SpotifyDataClient.DAL;
using SpotifyDataClient.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SpotifyDataClient.Controllers
{
    public class SongsController : Controller
    {
        private SpotifyContext db = new SpotifyContext();


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

            List<Album> albums = new List<Album>();
            List<Song> songs = new List<Song>();

            Artist artist = new Artist() { name = (string)artistTracks[0]["artists"][0]["name"] };

            for (int i = 0; i < numberOfTracks; i++)
            {
                try
                {
                    string albumName = (string)artistTracks[i]["album"]["name"];
                    var albumRecords = from a in db.Albums
                                       where a.name == albumName
                                       select a;
                    if (albumRecords.ToList().Count() == 0)
                    {
                        albums.Add(new Album() { name = albumName, releaseYear = (int)artistTracks[i]["album"]["released"], artist = artist });
                    } 
                    songs.Add(new Song() { name = (string)artistTracks[i]["name"], popularity = (float)artistTracks[i]["popularity"], length = (float)artistTracks[i]["length"], album = albums.Find(a => a.name == albumName) });
                }
                catch (Exception e)
                {
                    Console.Write(e.ToString());
                }
            }
            // Doing the change saving separately for better error finding.
            var artistRecords = from a in db.Artists
                                where a.name == artist.name
                                select a;
            if (artistRecords.ToList().Count() == 0)
            {
                db.Artists.Add(artist);
                db.SaveChanges();
            }
            albums = albums.Distinct().ToList();
            albums.ForEach(a => db.Albums.Add(a));
            db.SaveChanges();

            songs.ForEach(s => db.Songs.Add(s));
            db.SaveChanges();

            return View(songs);
        }
        // GET: Songs
        public ActionResult Index()
        {
            return View(db.Songs.ToList());
        }

        // GET: Songs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Song song = db.Songs.Find(id);
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song);
        }

        // GET: Songs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Songs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,name,popularity,length")] Song song)
        {
            if (ModelState.IsValid)
            {
                db.Songs.Add(song);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(song);
        }

        // GET: Songs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Song song = db.Songs.Find(id);
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song);
        }

        // POST: Songs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,name,popularity,length")] Song song)
        {
            if (ModelState.IsValid)
            {
                db.Entry(song).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(song);
        }

        // GET: Songs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Song song = db.Songs.Find(id);
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song);
        }

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Song song = db.Songs.Find(id);
            db.Songs.Remove(song);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
