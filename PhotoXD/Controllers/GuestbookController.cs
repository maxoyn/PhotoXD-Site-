using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;

namespace PhotoXD.Controllers
{
    [Authorize]
    public class GuestbookController : Controller
    {
        //
        // GET: /Guestbook/
        private PhotoXD.Models.Guestbookcontext _db = new PhotoXD.Models.Guestbookcontext();

        //[AllowAnonymous]
        //public ActionResult Index()
        //{
        //    var mostRecentEntries = (from entry in _db.Entries orderby entry.DateAdded descending select entry).Take(20);
        //    ViewBag.Entries = mostRecentEntries.ToList();
        //    return View();
        //}

        [AllowAnonymous]
        public async Task<ActionResult> IndexAsync()
        {
            var mostRecentEntries = (from entry in _db.Entries orderby entry.DateAdded descending select entry).Take(20);
            ViewBag.Entries = await mostRecentEntries.ToListAsync();
            return View();
        }

        [AllowAnonymous]
        public ActionResult UserComments(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("IndexAsync");
            }
            var mostRecentEntries = (from entry in _db.Entries orderby entry.DateAdded descending select entry).Take(20);
            ViewBag.Entries = mostRecentEntries.ToList();
            var entrys = _db.Entries.Find(id);
            if (entrys == null)
            {
                return RedirectToAction("IndexAsync");
            }
            ViewBag.UserName = entrys.Name;
            //refactor
            return View();
        }

        //[AllowAnonymous]
        //public async Task<ActionResult> UserCommentsAsync(int id)
        //{
        //    var mostRecentEntries = (from entry in _db.Entries orderby entry.DateAdded descending select entry).Take(20);
        //    ViewBag.Entries = await mostRecentEntries.ToListAsync();
        //    var entrys = _db.Entries.Find(id);
        //    if (entrys == null)
        //    {
        //        return RedirectToAction("IndexAsync");
        //    }
        //    ViewBag.UserName = entrys.Name;
        //    //refactor
        //    return View();
        //}

        //[AllowAnonymous]
        //public ActionResult CommentsByDate(string userDate)
        //{
        //    DateTime myDate = new DateTime();
        //    DateTime myUpToDate = new DateTime();
        //    if (!string.IsNullOrEmpty(userDate))
        //    {
        //        myDate = DateTime.Parse(userDate.Replace("!", ":"));
        //        myUpToDate = myDate.AddDays(1);
        //    }
        //    var entriesPerDate = (from entry in _db.Entries where entry.DateAdded <= myUpToDate orderby entry.Name descending select entry).Take(20);
        //    ViewBag.Entries = entriesPerDate.ToList();
        //    return View();
        //}

        [AllowAnonymous]
        public async Task<ActionResult> CommentsByDateAsync(string userDate)
        {
            DateTime myDate = new DateTime();
            DateTime myUpToDate = new DateTime();
            if (!string.IsNullOrEmpty(userDate))
            {
                myDate = DateTime.Parse(userDate.Replace("!", ":"));
                myUpToDate = myDate.AddDays(1);
            }
            var entriesPerDate = (from entry in _db.Entries where entry.DateAdded <= myUpToDate orderby entry.Name descending select entry).Take(20);
            ViewBag.Entries = await entriesPerDate.ToListAsync();

            return View();
        }

        public ActionResult Edit(int id)
        {
            var entry = _db.Entries.Find(id);
            //check if somebody is playing with the link edit/x for missing comment
            if (entry == null)
            {
                return RedirectToAction("IndexAsync");
            }
            if (User.Identity.Name == entry.Name || User.IsInRole("Admin"))
            {
                return View(entry);
            }
            else
                return RedirectToAction("IndexAsync");
        }

        [HttpPost]
        public ActionResult Edit(PhotoXD.Models.GuestbookEntry entry)
        {
            var editEntry = _db.Entries.Find(entry.Id);
            if (User.Identity.Name == editEntry.Name || User.IsInRole("Admin"))
            {
                editEntry.Message = entry.Message;
                _db.Entry(editEntry).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return RedirectToAction("IndexAsync");
        }

        [AllowAnonymous]
        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.UserName = User.Identity.Name;
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Create(PhotoXD.Models.GuestbookEntry entry)
        {
            if (User.Identity.IsAuthenticated)
            {
                entry.Name = User.Identity.Name;
            }
            entry.DateAdded = DateTime.Now;
            _db.Entries.Add(entry);
            _db.SaveChanges();

            return RedirectToAction("IndexAsync");
        }


        //[AllowAnonymous]
        //[HttpPost]
        //public async Task<ActionResult> CreateAsync(PhotoXD.Models.GuestbookEntry entry)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        entry.Name = User.Identity.Name;
        //    }
        //    entry.DateAdded = DateTime.Now;
        //    _db.Entries.Add(entry);
        //    await _db.SaveChangesAsync();

        //    return RedirectToAction("Index");
        //}

        public ActionResult Delete(int? id)
        {
            var entry = _db.Entries.Find(id);
            if (entry == null)
            {
                return RedirectToAction("IndexAsync");
            }
            if (User.Identity.Name == entry.Name || User.IsInRole("Admin"))
            {
                return View(entry);
            }
            else
            {
                return RedirectToAction("IndexAsync");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult
        DeleteConfirmed(PhotoXD.Models.GuestbookEntry entry)
        {
            var editEntry = _db.Entries.Find(entry.Id);
            if (User.Identity.Name == editEntry.Name || User.IsInRole("Admin"))
            {
                _db.Entries.Remove(editEntry);
                _db.SaveChanges();
            }
            return RedirectToAction("IndexAsync");
        }


        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult>
        //DeleteConfirmedAsync(PhotoXD.Models.GuestbookEntry entry)
        //{
        //    var editEntry = _db.Entries.Find(entry.Id);
        //    if (User.Identity.Name == editEntry.Name)
        //    {
        //        _db.Entries.Remove(editEntry);
        //        await _db.SaveChangesAsync();
        //    }
        //    return RedirectToAction("Index");
        //}
    }
}

