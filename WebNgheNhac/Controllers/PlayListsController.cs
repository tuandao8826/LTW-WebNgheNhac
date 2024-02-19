using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using WebNgheNhac.Models;

namespace WebNgheNhac.Controllers
{
    public class PlayListsController : Controller
    {
        private QLWebNgheNhacEntities db = new QLWebNgheNhacEntities();

        // GET: PlayLists
        public ActionResult Index(int? IdTaiKhoan)
        {
            return View();
        }

        [HttpGet]
        // tìm kiếm bài hát thêm vào playlist
        public ActionResult Search(string searchString)
        {
            // lấy danh sách 
            var songs = db.BaiHats.ToList();
            // kiểm tra chuỗi nhập có rỗng không
            if (!String.IsNullOrEmpty(searchString))
            {
                // chuyển chuỗi sang chữ thường
                searchString = searchString.ToLower();
                // truy vẫn chuỗi có xuất hiện trong database
                songs = songs.Where(s => s.TenBaiHat.ToLower().Contains(searchString)).ToList();
            }
            // trả về danh sách đã truy vấn
            return View(songs);
        }

        // GET: PlayLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlayList playList = db.PlayLists.Find(id);
            if (playList == null)
            {
                return HttpNotFound();
            }
            return View(playList);
        }

        // GET: PlayLists/Create
        /*public ActionResult Create(int? idTaiKhoan)
        {
            ViewBag.IdTaiKhoan = new SelectList(db.TaiKhoans, "IdTaiKhoan", "Username");
            return View();
        }*/

        // POST: PlayLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id)
        {
            PlayList playlist = new PlayList();
            playlist.IdTaiKhoan = id;
            var count = db.PlayLists.Where(pl => pl.IdTaiKhoan == id).Count();

            if (count > 0)
            {
                var check = db.PlayLists.Where(pl => pl.IdTaiKhoan == id && pl.TenPlayList.Contains("Danh sách nhạc của tôi #"));
                if (check != null)
                {
                    var max = db.PlayLists.Where(pl => pl.IdTaiKhoan == id).OrderByDescending(pl => pl.TenPlayList).FirstOrDefault();
                    var quantity = (int)max.TenPlayList.Last() - 48 + 1;
                    playlist.TenPlayList = $"Danh sách nhạc của tôi #{quantity}";
                }
                else
                    playlist.TenPlayList = $"Danh sách nhạc của tôi #0";
            }
            else
            {
                playlist.TenPlayList = $"Danh sách nhạc của tôi #0";
            }

            playlist.HinhPlayList = "user_primary.png";
            db.PlayLists.Add(playlist);
            db.SaveChanges();
            return Redirect($"~/Home/Index/{id}");
        }

        // GET: PlayLists/Edit/5
        /*public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlayList playList = db.PlayLists.Find(id);
            if (playList == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdTaiKhoan = new SelectList(db.TaiKhoans, "IdTaiKhoan", "Username", playList.IdTaiKhoan);
            return View(playList);
        }*/

        // POST: PlayLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "IdPlayList,TenPlayList,HinhPlayList,MoTa,IdTaiKhoan")] PlayList playList,
            HttpPostedFileBase HinhPlayList)
        {
            if (ModelState.IsValid)
            {
                if (HinhPlayList != null)
                {
                    //Lấy tên file của hình được up lên
                    var fileName = Path.GetFileName(HinhPlayList.FileName);

                    //Tạo đường dẫn tới file
                    var path = Path.Combine(Server.MapPath("~/Assets/Img"), fileName);

                    //Lưu tên
                    playList.HinhPlayList = fileName;

                    //Save vào Images Folder
                    HinhPlayList.SaveAs(path);
                }
                db.Entry(playList).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect($"~/MyPlayLists/Index?idPlaylist={playList.IdPlayList}");
            }
            ViewBag.IdTaiKhoan = new SelectList(db.TaiKhoans, "IdTaiKhoan", "Username", playList.IdTaiKhoan);
            return View(playList);
        }

        // GET: PlayLists/Delete/5
        public ActionResult Delete(int? id)
        {
            PlayList playList = db.PlayLists.Find(id);
            var m = db.MyPlayLists.Where(mpl => mpl.IdPlayList == playList.IdPlayList);
            foreach (var item in m)
            {
                db.MyPlayLists.Remove(item);
            }
            db.PlayLists.Remove(playList);
            db.SaveChanges();
            return Redirect($"~/Home/Index/{Session["IdTaiKhoan"]}");
        }

        /*// POST: PlayLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            PlayList playList = db.PlayLists.Find(id);
            db.PlayLists.Remove(playList);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }*/

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
