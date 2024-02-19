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
    public class UsersController : Controller
    {
        QLWebNgheNhacEntities db = new QLWebNgheNhacEntities();

        // Get: đăng ký tài khoản
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(TaiKhoan _user)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(_user.Username))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (string.IsNullOrEmpty(_user.MatKhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (string.IsNullOrEmpty(_user.Email))
                    ModelState.AddModelError(string.Empty, "Email không được để trống");

                //Kiểm tra xem có người nào đã đăng kí với tên đăng nhập này hay chưa
                var thanhvien = db.TaiKhoans.FirstOrDefault(k => k.Username == _user.Username);
                if (thanhvien != null)
                    ModelState.AddModelError(string.Empty, "Đã có người đăng kí tên này");

                if (ModelState.IsValid)
                {
                    _user.NgayDangKy = DateTime.Now;
                    db.TaiKhoans.Add(_user);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
            }
            return RedirectToAction("Register");
        }


        // Get: đăng nhập tài khoản
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(TaiKhoan _user)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(_user.Username))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (string.IsNullOrEmpty(_user.MatKhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (ModelState.IsValid)
                {
                    //Tìm tài khoản có tên đăng nhập và password hợp lệ trong CSDL
                    var thanhvien = db.TaiKhoans.FirstOrDefault(k => k.Username == _user.Username && k.MatKhau == _user.MatKhau);
                    if (thanhvien != null)
                    {
                        // tài khoản bị khóa
                        if (thanhvien.CheckTk == false)
                        {
                            return RedirectToAction("Login");
                        }
                        ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";

                        //Lưu vào session
                        Session["TaiKhoan"] = thanhvien;
                        Session["IdTaiKhoan"] = thanhvien.IdTaiKhoan;
                        Session["HoTen"] = thanhvien.HoTen;
                        Session["HinhTaiKhoan"] = thanhvien.HinhTaiKhoan;
                        Session["Admin"] = thanhvien.CheckTk == true ? true : false;

                        // kiểm tra tài khoản admin
                        if (thanhvien.CheckTk == true)
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        return RedirectToAction("Index", "Home", new { id = Session["IdTaiKhoan"]});
                    }
                    else
                    {
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                        return RedirectToAction("Login");
                    }
                }
            }
            return RedirectToAction("Login");
        }

        // Get: đăng xuất tài khoản
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: thông tin tài khoản
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // GET: chỉnh sử hồ sơ tài khoản
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "IdTaiKhoan,Username,Email,HoTen,MatKhau,NgaySinh,GioiTinh,SoDienThoai,DiaChi,NgayDangKy,CheckTk, HinhTaiKhoan")] TaiKhoan taiKhoan, 
            HttpPostedFileBase HinhTaiKhoan)
        {
            if (ModelState.IsValid)
            {
                if (HinhTaiKhoan != null)
                {
                    //Lấy tên file của hình được up lên
                    var fileName = Path.GetFileName(HinhTaiKhoan.FileName);

                    //Tạo đường dẫn tới file
                    var path = Path.Combine(Server.MapPath("~/Assets/Img"), fileName);

                    //Lưu tên
                    taiKhoan.HinhTaiKhoan = fileName;

                    Session["HinhTaiKhoan"] = taiKhoan.HinhTaiKhoan;

                    //Save vào Images Folder
                    HinhTaiKhoan.SaveAs(path);
                }
                db.Entry(taiKhoan).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect($"~/Users/Details/{taiKhoan.IdTaiKhoan}");
            }
            return View(taiKhoan);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // Get: đổi mật khẩu tài khoản
        public ActionResult ChangePassword(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(int? id, string crrPassword, string newPassword1, string newPassword2)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(crrPassword))
                    ModelState.AddModelError(string.Empty, "Mật khẩu hiện tại không được để trống");
                if (String.IsNullOrEmpty(newPassword1))
                    ModelState.AddModelError(string.Empty, "Vui lòng nhập mật khẩu mới");
                if (String.IsNullOrEmpty(newPassword2))
                    ModelState.AddModelError(string.Empty, "Vui lòng nhập lại mật khẩu mới");

                TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
                if (taiKhoan != null)
                {
                    if (taiKhoan.MatKhau != crrPassword)
                    {
                        ViewBag.ThongBao = "Rất tiếc mật khẩu không chính xác";
                        return RedirectToAction("ChangePassword");
                    }
                    if (newPassword1 != newPassword2)
                    {
                        ViewBag.ThongBao = "Vui lòng xác minh mật khẩu của bạn";
                        return RedirectToAction("ChangePassword");
                    }
                    taiKhoan.MatKhau = newPassword1;
                    db.Entry(taiKhoan).State = EntityState.Modified;
                    db.SaveChanges();
                    return Redirect($"~/Users/Details/{taiKhoan.IdTaiKhoan}");
                }
            }
            return View();
        }
    }
}