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
using SpotifyDataClient.ViewModels;

namespace SpotifyDataClient.Controllers
{
    public class ArtistsController : Controller
    {
        private SpotifyContext db = new SpotifyContext();

        // GET: Artists
        public ActionResult Index()
        {
            return View(db.Artists.ToList());
        }

        // GET: Artists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArtistName = artist.name;
            List<AlbumReleaseGroup> albumsViewModel = new List<AlbumReleaseGroup>();
            var albums = from album in db.Albums
                         where album.artist.name == artist.name
                         orderby album.releaseYear
                         select album;

            foreach (var album in albums.ToList())
            {
                float popularitySum = 0;
                float highestLength = 0;
                string longestTrackName = null;
                int albumSongsCount = 0;

                var songs = from song in db.Songs
                            where song.album.name == album.name
                            select song;

                foreach (var song in songs)
                {
                    albumSongsCount++;
                    popularitySum += song.popularity;
                    if (song.length > highestLength)
                    {
                        highestLength = song.length;
                        longestTrackName = song.name;
                    }
                }

                albumsViewModel.Add(new AlbumReleaseGroup()
                    {
                        albumName = album.name,
                        releaseYear = album.releaseYear,
                        longestTrackName = longestTrackName,
                        longestTrackLength = highestLength,
                        averageTrackPopularity = popularitySum / albumSongsCount
                    }
                );
            }

            return View(albumsViewModel);
        }

        // GET: Artists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "artistID,name")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                db.Artists.Add(artist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(artist);
        }

        // GET: Artists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "artistID,name")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artist);
        }

        // GET: Artists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Artist artist = db.Artists.Find(id);
            db.Artists.Remove(artist);
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
