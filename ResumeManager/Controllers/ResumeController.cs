using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using ResumeManager.Models;
using ResumeManager.DataCon;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using ResumeManager.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;




namespace ResumeManager.Controllers
{
    public class ResumeController : Controller

    {
        private readonly IApplicant _applicant;
        private readonly ResumeDBContext _context;
        private readonly IWebHostEnvironment _webHost;
        public ResumeController(ResumeDBContext context, IWebHostEnvironment webHost, IApplicant applicant)
        {
            _context = context;
            _webHost = webHost;
            _applicant = applicant;
        }
    
        public async Task<IActionResult> Index()
        {
            var getall = await _applicant.GetApplicants();
          
            return View(getall);
            //List<Applicant> applicants = _context.Applicants.ToList();
            //return View(applicants);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.ApplicantList= await _applicant.GetApplicants();
            ViewBag.gender = GetGender();
            //  Applicant applicant = new Applicant();
            //  applicant.Experinces.Add(new Experience() { ExperinceId = 1 });
            //applicant.Experinces.Add(new Experince() { ExperinceId=2 });
            //applicant.Experinces.Add(new Experince() { ExperinceId=3 });
            return View();
        }
        [HttpPost]
        public IActionResult Create(Applicant applicant)
        {

            applicant.Experinces.RemoveAll(n => n.YearsWorkrd == 0);
            string uniqueFileName = GetUploadedFileName(applicant);
            applicant.PhotoUrl = uniqueFileName;
            _context.Add(applicant);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private string GetUploadedFileName(Applicant applicant)
        {
            string uniqueFileName = null;
            if (applicant.ProfilePhoto != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "image");

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + timestamp + "_" + applicant.ProfilePhoto.FileName;

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);


                Directory.CreateDirectory(uploadsFolder);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    applicant.ProfilePhoto.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        //private string GetUploadedFileName(Applicant applicant)
        //{
        //    string uniqueFileName = null;

        //    if (applicant.ProfilePhoto != null)
        //    {
        //        string uploadsFolder = Path.Combine(_webHost.WebRootPath, "image");
        //        uniqueFileName = Guid.NewGuid().ToString() + "_" + applicant.ProfilePhoto.FileName;
        //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            applicant.ProfilePhoto.CopyTo(fileStream);
        //        }
        //    }
        //    return uniqueFileName;
        //}

        [HttpGet]
        public IActionResult Details(int Id)
        {

            Applicant applicant = _context.Applicants.Include(e => e.Experinces).Where(a => a.Id == Id).FirstOrDefault();
            return View(applicant);
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            Applicant applicant = _context.Applicants.Include(e => e.Experinces).Where(a=>a.Id==Id).FirstOrDefault();
            return View(applicant);

        }
        //[HttpPost]
        //public IActionResult Delete(Applicant applicant)
        //{
        //    _context.Attach(applicant);
        //    _context.Entry(applicant).State = EntityState.Deleted;
        //    _context.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicant = await _context.Applicants.FindAsync(id);
            if (applicant == null)
                return NotFound();

            try
            {

                var experiences = _context.Experinces.Where(d => d.Applicant.Id == id).ToList();

                _context.Experinces.RemoveRange(experiences);
                _context.Applicants.Remove(applicant);


                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.InnerException?.Message ?? ex.Message;
                ModelState.AddModelError(string.Empty, $"Delete failed: {sqlException}");
                return View(applicant);
            }
        }

        private List<SelectListItem> GetGender()
        {
            List<SelectListItem> selGender = new List<SelectListItem>();
            var selItem = new SelectListItem() { Value = "", Text = "Select Gender" };
            selGender.Insert(0, selItem);
            selItem = new SelectListItem() { Value = "Male", Text = "Male" };
            selGender.Add(selItem);
            selItem = new SelectListItem() { Value = "Female", Text = "Female" };
            selGender.Add(selItem);
            return selGender;
        }

    }
}
