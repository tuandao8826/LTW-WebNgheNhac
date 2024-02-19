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
    public class SingersController : Controller
    {
        private QLWebNgheNhacEntities db = new QLWebNgheNhacEntities();

        // GET: Singers
        public ActionResult Index()
        {
            return View(db.CaSis.ToList());
        }

        // GET: Singers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaSi caSi = db.CaSis.Find(id);
            if (caSi == null)
            {
                return HttpNotFound();
            }
            return View(caSi);
        }

        // GET: Singers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Singers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "IdCaSi,HoTen,NgaySinh,GioiTinh,GioiThieu,HinhCaSi")] CaSi caSi,
            HttpPostedFileBase HinhCaSi)
        {
            if (ModelState.IsValid)
            {
                if (HinhCaSi != null)
                {
                    //Lấy tên file của hình được up lên
                    var fileName = Path.GetFileName(HinhCaSi.FileName);

                    //Tạo đường dẫn tới file
                    var path = Path.Combine(Server.MapPath("~/Assets/Img"), fileName);

                    //Lưu tên
                    caSi.HinhCaSi = fileName;

                    //Save vào Images Folder
                    HinhCaSi.SaveAs(path);
                }
                db.CaSis.Add(caSi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(caSi);
        }

        // GET: Singers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaSi caSi = db.CaSis.Find(id);
            if (caSi == null)
            {
                return HttpNotFound();
            }
            return View(caSi);
        }

        // POST: Singers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "IdCaSi,HoTen,NgaySinh,GioiTinh,GioiThieu,HinhCaSi")] CaSi caSi,
            HttpPostedFileBase HinhCaSi)
        {
            if (ModelState.IsValid)
            {
                if (HinhCaSi != null)
                {
                    //Lấy tên file của hình được up lên
                    var fileName = Path.GetFileName(HinhCaSi.FileName);

                    //Tạo đường dẫn tới file
                    var path = Path.Combine(Server.MapPath("~/Assets/Img"), fileName);

                    //Lưu tên
                    caSi.HinhCaSi = fileName;

                    //Save vào Images Folder
                    HinhCaSi.SaveAs(path);
                }
                db.Entry(caSi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(caSi);
        }

        // GET: Singers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaSi caSi = db.CaSis.Find(id);
            if (caSi == null)
            {
                return HttpNotFound();
            }
            return View(caSi);
        }

        // POST: Singers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CaSi caSi = db.CaSis.Find(id);
            db.CaSis.Remove(caSi);
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
