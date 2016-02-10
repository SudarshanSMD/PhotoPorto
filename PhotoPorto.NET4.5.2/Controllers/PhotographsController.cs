using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PhotoPorto.Models;
using PhotoPorto.Utility;

namespace PhotoPorto.Controllers
{
    [Authorize]
    public class PhotographsController : Controller
    {
        private DAL.PhotoPortoDbContext db = new DAL.PhotoPortoDbContext();

        // GET: Photographs
        [Route("Photographs")]        
        public ActionResult Index(int? galleryId)
        {
            /*if (galleryId != null) {
                return RedirectToAction("Details", "Galleries", new { id = galleryId});
            }*/
            return View(db.Photograph.ToList());
        }

        // GET: Photographs/Manage
        [Authorize(Roles = "administrator")]
        [Route("Photographs/Manage",Name = "Photographs_Manage_route")]       
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

            var photographs = from p in db.Photograph select p;

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    galleries = galleries.Where(s => s.LastName.Contains(searchString)
            //                           || s.FirstMidName.Contains(searchString));
            //}
            switch (sortOrder)
            {
                case "name_desc":
                    photographs = photographs.OrderByDescending(p => p.Title);
                    break;
                case "Date":
                    photographs = photographs.OrderBy(p => p.CreationDate);
                    break;
                case "date_desc":
                    photographs = photographs.OrderByDescending(p => p.CreationDate);
                    break;
                default:  // Name ascending 
                    photographs = photographs.OrderBy(p => p.Title);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(photographs.ToPagedList(pageNumber, pageSize));
        }



        // GET: Photographs/Details/5
        [Route("Photographs/{id?}")]        
        [Route("Galleries/{galleryId}/Photographs/{id}", Name = "Galleries_GalleryId_Photographs_PhotographId_Route")]
        public ActionResult Details(int? galleryId, int? id, string resolution)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photograph photograph = db.Photograph.Find(id);
            if (photograph == null)
            {
                return HttpNotFound();
            }
            return View(photograph);
        }

        // GET: Photographs/Create
        [Authorize(Roles = "administrator")]
        [Route("Photographs/Create", Name = "Photographs_Create_route")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Photographs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Photographs")]
        public ActionResult Create([Bind(Include = "ID,Title,Description,Type,Tags,Salt,SplitPhotographKey,PhotographWidth,PhotographHeight,UHD4K,FHD,HD,nHD,LikeCount,FavouriteCount,CreationDate")] Photograph photograph, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                // Create original photograph key                
                string originalPhotographKey = Utility.Crypto.GenerateRandomAlphaNumericString(10);
                photograph.OriginalPhotographKey = originalPhotographKey;

                // Create SplitPhotographKey
                string splitImageKey = Utility.Crypto.GenerateRandomAlphaNumericString(8);
                photograph.SplitPhotographKey = splitImageKey;

                //Setting row or column count
                /*
                TODO: specify splitcoumnCount and splitRowCount.
                Need to just specify count. Logic for splitting and displaying images is already sorted.
                */
                photograph.SplitColumnCount = Constants.SPLIT_IMAGE_COLUMN_COUNT;
                photograph.SplitRowCount = Constants.SPLIT_IMAGE_ROW_COUNT;

                photograph.CreationDate = DateTime.Now;
                db.Photograph.Add(photograph);
                db.SaveChanges();



                if (upload != null && upload.ContentLength > 0)
                {
                    var photographDirecotoryPath = HttpContext.Server.MapPath("~/photograph");

                    //Naming of folder and files associated with image
                    var imageFolderPath = Path.Combine(photographDirecotoryPath, photograph.ID.ToString());                    
                    if (!System.IO.Directory.Exists(imageFolderPath))
                    {
                        System.IO.Directory.CreateDirectory(imageFolderPath);
                    }
                    var originalFilePath = Path.Combine(imageFolderPath, photograph.ID + "_" + originalPhotographKey + ".jpg");                    
                    var loweResolutionImagePath = Path.Combine(imageFolderPath, photograph.ID + ".jpg");
                    var thumbnailImagePath = Path.Combine(imageFolderPath, photograph.ID + "_th.jpg");

                    //Copy stream as original file
                    using (Stream outputFile = System.IO.File.Create(originalFilePath))
                    {
                        Utility.FileUtility.CopyStream(upload.InputStream, outputFile);
                    }

                    Bitmap img = new Bitmap(upload.InputStream);
                    var imageHeight = img.Height;
                    var imageWidth = img.Width;

                    //Updating photographs width and height
                    photograph.PhotographWidth = imageWidth;
                    photograph.PhotographHeight = imageHeight;

                    bool isImageHorizontal = true;
                    if (imageHeight > imageWidth)
                    {
                        isImageHorizontal = false;
                    }

                    //Generating thumbnail
                    Utility.ImageUtility.ResizeImage(originalFilePath, thumbnailImagePath, 0, 75);

                    //Generating low resolution image
                    if (isImageHorizontal) {
                        Utility.ImageUtility.ResizeImage(originalFilePath, loweResolutionImagePath, 400, 0);
                    } else {
                        Utility.ImageUtility.ResizeImage(originalFilePath, loweResolutionImagePath, 0, 300);
                    }
                    //Utility.ImageUtility.CropImage(originalFilePath, temp2FilePath, 0 , 0, 200, 200);


                    /*
                    Creating images in different resolution and their respective split images 
                    */
                    // Photograph size currently not available
                    photograph.HD = false;
                    photograph.nHD = false;
                    photograph.UHD4K = false;
                    photograph.FHD = false;
                    photograph.qHD = false;
                    photograph.og = false;

                    //Create list of imageResolution. These ImageResolution's will be create for image. For adding new ImageResolution, add it to this list.
                    List<ImageResolution> imageResolutionList = new List<ImageResolution>();
                    imageResolutionList.Add(ImageResolution.UHD4K);
                    imageResolutionList.Add(ImageResolution.FHD);
                    imageResolutionList.Add(ImageResolution.qHD);
                    imageResolutionList.Add(ImageResolution.nHD);
                    //Special case when image resolution is smaller than 'nHD'. Create split image and name it as _og.jpg
                    if ((imageWidth < ImageResolution.nHD.Width && imageHeight < ImageResolution.nHD.Height) && (imageWidth > 400 || imageHeight > 300))
                    {
                        ImageResolution customOgImageResolution = ImageResolution.og;
                        customOgImageResolution.Width = imageWidth;
                        customOgImageResolution.Height = imageHeight;    

                        imageResolutionList.Add(customOgImageResolution);
                        photograph.og = true;
                    }                   

                        foreach (ImageResolution imageResolution in imageResolutionList)
                    {
                        if (imageWidth >= imageResolution.Width || imageHeight >= imageResolution.Height)
                        {
                            //Create resized image for particular iamge resolution
                            var resizedImagePath = Path.Combine(imageFolderPath, photograph.ID + "_" + originalPhotographKey + "_" + imageResolution.Representation + ".jpg");
                            if (isImageHorizontal)
                            {
                                Utility.ImageUtility.ResizeImage(originalFilePath, resizedImagePath, imageResolution.Width, 0);
                            }
                            else {
                                Utility.ImageUtility.ResizeImage(originalFilePath, resizedImagePath, 0, imageResolution.Height);
                            }

                            //Split Image wiht spili image key
                            Service.PhotographService.SplitImage(resizedImagePath, imageFolderPath, photograph.ID.ToString(), splitImageKey, Constants.SPLIT_IMAGE_COLUMN_COUNT, Constants.SPLIT_IMAGE_ROW_COUNT, imageResolution.Representation);

                            //Setting ture value for resolution in Photograph
                            switch (imageResolution.Representation)
                            {
                                case Constants.RESOLUTION_4K :
                                    photograph.UHD4K = true;
                                    break;
                                case Constants.RESOLUTION_FHD:
                                    photograph.FHD = true;
                                    break;
                                case Constants.RESOLUTION_qHD:
                                    photograph.qHD = true;
                                    break;
                                case Constants.RESOLUTION_nHD:
                                    photograph.nHD = true;
                                    break;
                            }                            
                        }
                    }                    
                }
                
                db.SaveChanges();

                return RedirectToAction("Manage");
            }

            return View(photograph);
        }

        // GET: Photographs/Edit/5
        [Authorize(Roles = "administrator")]
        [Route("Photographs/Edit/{id}", Name = "Photographs_Edit_PhotographId_route")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photograph photograph = db.Photograph.Find(id);
            if (photograph == null)
            {
                return HttpNotFound();
            }
            return View(photograph);
        }

        // POST: Photographs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "administrator")]
        [HttpPut]
        [ValidateAntiForgeryToken]
        [Route("Photographs")]
        //public ActionResult Edit([Bind(Include = "ID,Title,Description,Type,Tags,Salt,SplitPhotographKey,PhotographWidth,PhotographHeight,UHD4K,FHD,HD,nHD,LikeCount,FavouriteCount,CreationDate")] Photograph photograph)
        public ActionResult Edit([Bind(Include = "ID,Title,Description,Type,Tags,LikeCount,FavouriteCount")] Photograph photograph)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photograph).State = EntityState.Modified;
                db.Entry(photograph).Property(p => p.UHD4K).IsModified = false;                
                db.Entry(photograph).Property(p => p.FHD).IsModified = false;
                db.Entry(photograph).Property(p => p.HD).IsModified = false;                
                db.Entry(photograph).Property(p => p.qHD).IsModified = false;
                db.Entry(photograph).Property(p => p.nHD).IsModified = false;
                db.Entry(photograph).Property(p => p.og).IsModified = false;

                db.Entry(photograph).Property(p => p.SplitColumnCount).IsModified = false;
                db.Entry(photograph).Property(p => p.SplitRowCount).IsModified = false;
                db.Entry(photograph).Property(p => p.OriginalPhotographKey).IsModified = false;
                db.Entry(photograph).Property(p => p.SplitPhotographKey).IsModified = false;
                db.Entry(photograph).Property(p => p.PhotographWidth).IsModified = false;
                db.Entry(photograph).Property(p => p.PhotographHeight).IsModified = false;
                db.Entry(photograph).Property(p => p.CreationDate).IsModified = false;                
                db.SaveChanges();
                return RedirectToAction("Manage");
            }
            return View(photograph);
        }

        // GET: Photographs/Delete/5
        [Authorize(Roles = "administrator")]
        [Route("Photographs/Delete/{id}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photograph photograph = db.Photograph.Find(id);
            if (photograph == null)
            {
                return HttpNotFound();
            }
            return View(photograph);
        }

        // POST: Photographs/Delete/5
        [Authorize(Roles = "administrator")]
        [HttpDelete]
        [ValidateAntiForgeryToken]
        [Route("Photographs")]
        public ActionResult DeleteConfirmed(int id)
        {
            Photograph photograph = db.Photograph.Find(id);
            db.Photograph.Remove(photograph);
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
