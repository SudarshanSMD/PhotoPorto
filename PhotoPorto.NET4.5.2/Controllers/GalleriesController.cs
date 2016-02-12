using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PhotoPorto.DAL;
using PhotoPorto.Models;

namespace PhotoPorto.Controllers
{
    //[Route("Galleries")]
    public class GalleriesController : Controller
    {
        private DAL.PhotoPortoDbContext db = new DAL.PhotoPortoDbContext();

        // GET: Galleries
        [Route("Galleries")]
        public ActionResult Index()
        {
            IEnumerable<Gallery> GalleryList = db.Gallery.Include(p => p.Photograph).ToList();
            foreach (Gallery g in GalleryList)
            {
                db.Entry(g).Reference(p => p.Photograph).Load();
            }

            return View(GalleryList);
            //            return View(db.Gallery.Include(p => p.Photograph).ToList());
        }

        // GET: Galleries/Manage
        // [Route("Galleries/Manage",Name = "Galleries_Manage_route")]
        //    public ActionResult Manage()
        // {
        //     IEnumerable<Gallery> GalleryList = db.Gallery.Include(p => p.Photograph).ToList(); 
        //     return View(GalleryList);
        //  }

        // GET: Galleries/Manage
        [Authorize(Roles = "administrator")]
        [Route("Galleries/Manage", Name = "Galleries_Manage_route")]
        public ActionResult Manage(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            //if (searchString != null)
            //{
            //    page = 1;
            //}
            //else
            //{
            //    searchString = currentFilter;
            //}

            //ViewBag.CurrentFilter = searchString;

            var galleries = from g in db.Gallery  select g;

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    galleries = galleries.Where(s => s.LastName.Contains(searchString)
            //                           || s.FirstMidName.Contains(searchString));
            //}
            switch (sortOrder)
            {
                case "name_desc":
                    galleries = galleries.OrderByDescending(g => g.Title);
                    break;
                case "Date":
                    galleries = galleries.OrderBy(g => g.CreationDate);
                    break;
                case "date_desc":
                    galleries = galleries.OrderByDescending(g => g.CreationDate);
                    break;
                default:  // Name ascending 
                    galleries = galleries.OrderBy(g => g.Title);
                    break;
            }

            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(galleries.ToPagedList(pageNumber, pageSize));            
        }


        // GET: Galleries/Details/5
        //[HttpGet]
        [Route("Galleries/{id?}")]
        [Route("Galleries/{id?}/Photographs")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = db.Gallery.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }

            return View(gallery);
        }

        // GET: Galleries/Create
        [Authorize(Roles = "administrator")]
        [Route("Galleries/Create", Name = "Galleries_Create_route")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Galleries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Galleries")]
        public ActionResult Create([Bind(Include = "ID,Title,Description")] Gallery gallery)
        {
            if (ModelState.IsValid)
            {
                gallery.CreationDate = DateTime.Now;
                gallery.LastModifiedDate = DateTime.Now;
                db.Gallery.Add(gallery);
                db.SaveChanges();
                return RedirectToAction("Manage");
            }

            return View(gallery);
        }

        // GET: Galleries/Edit/5
        [Authorize(Roles = "administrator")]
        [HttpGet]
        [Route("Galleries/Edit/{id}", Name = "Galleries_Edit_GalleryId_route")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = db.Gallery.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            List<Photograph> photographs = db.Photograph.ToList<Photograph>();

            GalleryPhotographsModel galleryPhotographsModel = new GalleryPhotographsModel();
            galleryPhotographsModel.Photographs = photographs;
            galleryPhotographsModel.Gallery = gallery;
            return View(galleryPhotographsModel);
        }

        // POST: Galleries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "administrator")]
        [HttpPut]
        [ValidateAntiForgeryToken]
        [Route("Galleries")]
        public ActionResult Edit([Bind(Include = "ID,Title,Description")] Gallery gallery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gallery).State = EntityState.Modified;
                gallery.LastModifiedDate = DateTime.Now;
                db.Entry(gallery).Property(g => g.CreationDate).IsModified = false;
               
                if (Request["photographs"] != null)
                {
                    //Clear all photographs entries from GalleryPhotographs many-to-many table
                    db.Entry(gallery).Collection("Photographs").Load();
                    List<Photograph> allPhotographs = db.Photograph.ToList();
                    foreach (Photograph photograph in allPhotographs)
                    {
                        gallery.Photographs.Remove(photograph);
                    }
                    
                    String photographsInputString = Request["photographs"];
                    List<string> photographIds = photographsInputString.Split(',').ToList<string>();                                        
                    foreach (String photographIdString in photographIds)
                    {                        
                        if (photographIdString != null  && !("").Equals(photographIdString)) {
                            int photographId = int.Parse(photographIdString);
                            Photograph photograph = db.Photograph.Find(photographId);
                            if (photograph != null)
                            {
                                gallery.Photographs.Add(photograph);                                
                            }
                        }                        
                    }                 
                }               
                db.SaveChanges();
                return RedirectToAction("Manage");
            }
            return View(gallery);
        }

        // GET: Galleries/Delete/5
        [Authorize(Roles = "administrator")]
        [Route("Galleries/Delete/{id}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = db.Gallery.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        // POST: Galleries/Delete/5
        //        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "administrator")]
        [HttpDelete]
        [ValidateAntiForgeryToken]
        [Route("Galleries")]
        public ActionResult DeleteConfirmed(int id)
        {
            Gallery gallery = db.Gallery.Find(id);
            db.Gallery.Remove(gallery);
            db.SaveChanges();
            return RedirectToAction("Manage");
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
