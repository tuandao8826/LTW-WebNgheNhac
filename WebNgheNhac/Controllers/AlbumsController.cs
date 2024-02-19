using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebNgheNhac.Models;
using System.IO;

namespace WebNgheNhac.Controllers
{
    public class AlbumsController : Controller
    {
        private QLWebNgheNhacEntities db = new QLWebNgheNhacEntities();

        // GET: Albums
        public ActionResult Index()
        {
            var albums = db.Albums.Include(a => a.CaSi);
            return View(albums.ToList());
        }

        // GET: Albums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // GET: Albums/Create
        public ActionResult Create()
        {
            ViewBag.IdCaSi = new SelectList(db.CaSis, "IdCaSi", "HoTen");
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "IdAlbum,TenAlbum,HinhAlbum,IdCaSi")] Album album,
            HttpPostedFileBase HinhAlbum)
        {
            if (ModelState.IsValid)
            {
                if (HinhAlbum != null)
                {
                    //Lấy tên file của hình được up lên
                    var fileName = Path.GetFileName(HinhAlbum.FileName);

                    //Tạo đường dẫn tới file
                    var path = Path.Combine(Server.MapPath("~/Assets/Img"), fileName);

                    //Lưu tên
                    album.HinhAlbum = fileName;

                    //Save vào Images Folder
                    HinhAlbum.SaveAs(path);
                }
                db.Albums.Add(album);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCaSi = new SelectList(db.CaSis, "IdCaSi", "HoTen", album.IdCaSi);
            return View(album);
        }

        // GET: Albums/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCaSi = new SelectList(db.CaSis, "IdCaSi", "HoTen", album.IdCaSi);
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "IdAlbum,TenAlbum,HinhAlbum,IdCaSi")] Album album,
            HttpPostedFileBase HinhAlbum)
        {
            if (ModelState.IsValid)
            {
                if (HinhAlbum != null)
                {
                    //Lấy tên file của hình được up lên
                    var fileName = Path.GetFileName(HinhAlbum.FileName);

                    //Tạo đường dẫn tới file
                    var path = Path.Combine(Server.MapPath("~/Assets/Img"), fileName);

                    //Lưu tên
                    album.HinhAlbum = fileName;

                    //Save vào Images Folder
                    HinhAlbum.SaveAs(path);
                }
                db.Entry(album).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCaSi = new SelectList(db.CaSis, "IdCaSi", "HoTen", album.IdCaSi);
            return View(album);
        }

        // GET: Albums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Album album = db.Albums.Find(id);
            db.Albums.Remove(album);
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
