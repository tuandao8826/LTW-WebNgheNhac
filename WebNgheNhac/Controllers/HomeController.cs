using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebNgheNhac.Models;

namespace WebNgheNhac.Controllers
{
    public class HomeController : Controller
    {
        private QLWebNgheNhacEntities db = new QLWebNgheNhacEntities();

        // Get BaiHat
        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                // lấy playlist
                var myPlaylist = db.PlayLists.Where(pl => pl.IdTaiKhoan == id);
                Session["MyPlaylist"] = myPlaylist.ToList();
            }

            // lấy tên thể loại
            var theLoaiNhac = db.TheLoais.OrderBy(bh => Guid.NewGuid());
            ViewBag.TheLoaiNhac = theLoaiNhac;

            // lấy bài hát ngẫu nhiên
            var songs = db.BaiHats.OrderBy(bh => Guid.NewGuid());
            return View(songs.ToList());
        }

        [HttpGet]
        // tìm kiếm bài hát
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
                songs = songs.Where(s => s.TenBaiHat.ToLower().Contains(searchString)).OrderBy(s => Guid.NewGuid()).ToList(); 
            }
            // trả về danh sách đã truy vấn
            return View(songs);
        }

        public ActionResult Album(int? id)
        {
            if (id != null)
            {
                // lấy playlist
                var myPlaylist = db.PlayLists.Where(pl => pl.IdTaiKhoan == id);
                Session["MyPlaylist"] = myPlaylist.ToList();
            }

            // lấy album
            var albums = db.Albums.OrderBy(ab => Guid.NewGuid());
            ViewBag.Albums = albums;

            // lấy bài hát ngẫu nhiên
            var songs = db.BaiHats.OrderBy(bh => Guid.NewGuid());
            return View(songs.ToList());
        }

        // Thông tin album
        public ActionResult DetailsAlbum(int? id)
        {
            var album = db.Albums.Find(id);

            if (album != null)
            {
                // lấy danh sách bài hát của ca sĩ
                var songs = db.BaiHats.Where(bh => bh.IdAlbum == album.IdAlbum);

                // lấy thông tin ca sĩ
                ViewBag.Album = album;

                // trả về view
                return View(songs);
            }
            else
                return RedirectToAction("Index");
        }

        // Thông tin ca sĩ
        public ActionResult DetailsSinger(int? id)
        {
            var singer = db.CaSis.Find(id);

            if (singer != null)
            {
                // lấy danh sách bài hát của ca sĩ
                var songs = db.BaiHats.Where(bh => bh.IdCaSi == singer.IdCaSi);

                // lấy thông tin ca sĩ
                ViewBag.Singer = singer;

                // trả về view
                return View(songs);
            }
            else
                return RedirectToAction("Index");
        }

        // Thông tin bài hát
        public ActionResult DetailsSong(int? id)
        {
            var song = db.BaiHats.Find(id);

            if (song != null)
            {
                // lấy danh sách bài hát của ca sĩ
                var songs = db.BaiHats.Where(bh => bh.IdCaSi == song.IdCaSi);

                // lấy thông tin ca sĩ
                ViewBag.Song = song;

                // trả về view
                return View(songs);
            }
            else
                return RedirectToAction("Index");
        }

        // danh sách tất cả nhạc theo thể loại
        public ActionResult DetailsGenres(int? id)
        {
            var checkTheLoai = db.TheLoais.Find(id);
            ViewBag.TenTheLoai = checkTheLoai.TenTheLoai;
            var songs = db.BaiHats.Where(bh => bh.IdTheLoai == checkTheLoai.IdTheLoai).OrderBy(bh => Guid.NewGuid());
            return View(songs);
        } 
    }
}