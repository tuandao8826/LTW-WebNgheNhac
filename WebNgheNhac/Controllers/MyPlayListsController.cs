using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebNgheNhac.Models;

namespace WebNgheNhac.Controllers
{
    public class MyPlayListsController : Controller
    {
        private QLWebNgheNhacEntities db = new QLWebNgheNhacEntities();

        // GET: MyPlayLists
        public ActionResult Index(int? idPlaylist, string searchString)
        {
            // lấy bài hát đề xuất
            var songOffer = db.BaiHats.OrderBy(bh => Guid.NewGuid()).Take(8);
            ViewBag.SongOffer = songOffer;

            // lấy playlist
            var playlist = db.PlayLists.Where(pl => pl.IdPlayList == idPlaylist).FirstOrDefault();
            ViewBag.Playlist = playlist;

            Session["MyPlaylist"] = db.PlayLists.Where(pl => pl.IdTaiKhoan == playlist.IdTaiKhoan);

            // lấy danh sách nhạc từ myPlaylist đã có
            var songs = from s in db.BaiHats
                        join mpl in db.MyPlayLists on s.IdBaiHat equals mpl.IdBaiHat
                        where mpl.IdPlayList == playlist.IdPlayList
                        select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                // chuyển chuỗi sang chữ thường
                searchString = searchString.ToLower();
                ViewBag.Check = 1;

                // truy vẫn chuỗi có xuất hiện trong database
                songs = db.BaiHats.Where(s => s.TenBaiHat.ToLower().Contains(searchString)).OrderBy(s => Guid.NewGuid());
            }
            return View(songs);
        }

        // GET: MyPlayLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyPlayList myPlayList = db.MyPlayLists.Find(id);
            if (myPlayList == null)
            {
                return HttpNotFound();
            }
            return View(myPlayList);
        }

        // GET: MyPlayLists/Create
        public ActionResult Create()
        {
            ViewBag.IdBaiHat = new SelectList(db.BaiHats, "IdBaiHat", "TenBaiHat");
            ViewBag.IdPlayList = new SelectList(db.PlayLists, "IdPlayList", "TenPlayList");
            return View();
        }

        // POST: MyPlayLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdBaiHat,IdPlayList,hihi")] MyPlayList myPlayList)
        {
            if (ModelState.IsValid)
            {
                db.MyPlayLists.Add(myPlayList);
                db.SaveChanges();
                return Redirect($"~/MyPlayLists/Index?idPlaylist={myPlayList.IdPlayList}");
            }

            ViewBag.IdBaiHat = new SelectList(db.BaiHats, "IdBaiHat", "TenBaiHat", myPlayList.IdBaiHat);
            ViewBag.IdPlayList = new SelectList(db.PlayLists, "IdPlayList", "TenPlayList", myPlayList.IdPlayList);
            return View(myPlayList);
        }

        // GET: MyPlayLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyPlayList myPlayList = db.MyPlayLists.Find(id);
            if (myPlayList == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdBaiHat = new SelectList(db.BaiHats, "IdBaiHat", "TenBaiHat", myPlayList.IdBaiHat);
            ViewBag.IdPlayList = new SelectList(db.PlayLists, "IdPlayList", "TenPlayList", myPlayList.IdPlayList);
            return View(myPlayList);
        }

        // POST: MyPlayLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdBaiHat,IdPlayList,hihi")] MyPlayList myPlayList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(myPlayList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdBaiHat = new SelectList(db.BaiHats, "IdBaiHat", "TenBaiHat", myPlayList.IdBaiHat);
            ViewBag.IdPlayList = new SelectList(db.PlayLists, "IdPlayList", "TenPlayList", myPlayList.IdPlayList);
            return View(myPlayList);
        }

        // GET: MyPlayLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyPlayList myPlayList = db.MyPlayLists.Find(id);
            if (myPlayList == null)
            {
                return HttpNotFound();
            }
            return View(myPlayList);
        }

        // POST: MyPlayLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? idBaiHat, int? idPlayList)
        {
            MyPlayList myPlayList = db.MyPlayLists.Where(mpl => mpl.IdBaiHat == idBaiHat && mpl.IdPlayList == idPlayList).FirstOrDefault();
            db.MyPlayLists.Remove(myPlayList);
            db.SaveChanges();
            return Redirect($"~/MyPlayLists/Index?idPlaylist={myPlayList.IdPlayList}");
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
