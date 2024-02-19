using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebNgheNhac.Models;

namespace WebNgheNhac.Controllers
{
    public class SongsController : Controller
    {
        private QLWebNgheNhacEntities db = new QLWebNgheNhacEntities();

        // GET: Songs
        public ActionResult Index()
        {
            var baiHats = db.BaiHats.Include(b => b.Album).Include(b => b.CaSi).Include(b => b.TheLoai);
            return View(baiHats.ToList());
        }

        // GET: Songs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiHat baiHat = db.BaiHats.Find(id);
            if (baiHat == null)
            {
                return HttpNotFound();
            }
            return View(baiHat);
        }

        // GET: Songs/Create
        public ActionResult Create()
        {
            ViewBag.IdAlbum = new SelectList(db.Albums, "IdAlbum", "TenAlbum");
            ViewBag.IdCaSi = new SelectList(db.CaSis, "IdCaSi", "HoTen");
            ViewBag.IdTheLoai = new SelectList(db.TheLoais, "IdTheLoai", "TenTheLoai");
            return View();
        }

        // POST: Songs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "IdBaiHat,TenBaiHat,LinkBaiHat,HinhBaiHat,SoLanNghe,IdCaSi,IdTheLoai,IdAlbum")] BaiHat baiHat, 
            HttpPostedFileBase LinkBaiHat, 
            HttpPostedFileBase HinhBaiHat)
        {
            if (ModelState.IsValid)
            {
                if (LinkBaiHat != null)
                {
                    //Lấy tên file của hình được up lên
                    var fileName = Path.GetFileName(LinkBaiHat.FileName);

                    //Tạo đường dẫn tới file
                    var path = Path.Combine(Server.MapPath("~/Assets/Musics"), fileName);

                    //Lưu tên
                    baiHat.LinkBaiHat = fileName;

                    //Save vào Images Folder
                    LinkBaiHat.SaveAs(path);
                }
                if (HinhBaiHat != null)
                {
                    //Lấy tên file của hình được up lên
                    var fileName = Path.GetFileName(HinhBaiHat.FileName);

                    //Tạo đường dẫn tới file
                    var path = Path.Combine(Server.MapPath("~/Assets/Img"), fileName);

                    //Lưu tên
                    baiHat.HinhBaiHat = fileName;

                    //Save vào Images Folder
                    HinhBaiHat.SaveAs(path);
                }
                db.BaiHats.Add(baiHat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdAlbum = new SelectList(db.Albums, "IdAlbum", "TenAlbum", baiHat.IdAlbum);
            ViewBag.IdCaSi = new SelectList(db.CaSis, "IdCaSi", "HoTen", baiHat.IdCaSi);
            ViewBag.IdTheLoai = new SelectList(db.TheLoais, "IdTheLoai", "TenTheLoai", baiHat.IdTheLoai);
            return View(baiHat);
        }

        // GET: Songs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiHat baiHat = db.BaiHats.Find(id);
            if (baiHat == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdAlbum = new SelectList(db.Albums, "IdAlbum", "TenAlbum", baiHat.IdAlbum);
            ViewBag.IdCaSi = new SelectList(db.CaSis, "IdCaSi", "HoTen", baiHat.IdCaSi);
            ViewBag.IdTheLoai = new SelectList(db.TheLoais, "IdTheLoai", "TenTheLoai", baiHat.IdTheLoai);
            return View(baiHat);
        }

        // POST: Songs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "IdBaiHat,TenBaiHat,LinkBaiHat,HinhBaiHat,SoLanNghe,IdCaSi,IdTheLoai,IdAlbum")] BaiHat baiHat, 
            HttpPostedFileBase LinkBaiHat, 
            HttpPostedFileBase HinhBaiHat)
        {
            if (ModelState.IsValid)
            {
                if (LinkBaiHat != null)
                {
                    //Lấy tên file của hình được up lên
                    var fileName = Path.GetFileName(LinkBaiHat.FileName);

                    //Tạo đường dẫn tới file
                    var path = Path.Combine(Server.MapPath("~/Assets/Musics"), fileName);

                    //Lưu tên
                    baiHat.LinkBaiHat = fileName;

                    //Save vào Images Folder
                    LinkBaiHat.SaveAs(path);
                }
                if (HinhBaiHat != null)
                {
                    //Lấy tên file của hình được up lên
                    var fileName = Path.GetFileName(HinhBaiHat.FileName);

                    //Tạo đường dẫn tới file
                    var path = Path.Combine(Server.MapPath("~/Assets/Img"), fileName);

                    //Lưu tên
                    baiHat.HinhBaiHat = fileName;

                    //Save vào Images Folder
                    HinhBaiHat.SaveAs(path);
                }
                db.Entry(baiHat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdAlbum = new SelectList(db.Albums, "IdAlbum", "TenAlbum", baiHat.IdAlbum);
            ViewBag.IdCaSi = new SelectList(db.CaSis, "IdCaSi", "HoTen", baiHat.IdCaSi);
            ViewBag.IdTheLoai = new SelectList(db.TheLoais, "IdTheLoai", "TenTheLoai", baiHat.IdTheLoai);
            return View(baiHat);
        }

        // GET: Songs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiHat baiHat = db.BaiHats.Find(id);
            if (baiHat == null)
            {
                return HttpNotFound();
            }
            return View(baiHat);
        }

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BaiHat baiHat = db.BaiHats.Find(id);
            db.BaiHats.Remove(baiHat);
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
