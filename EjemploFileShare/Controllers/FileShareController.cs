using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EjemploFileShare.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EjemploFileShare.Controllers
{
    public class FileShareController : Controller
    {
        ServiceFileShare servicefiles;

        public FileShareController(ServiceFileShare servicefiles)
        {
            this.servicefiles = servicefiles;
        }

        public async Task<IActionResult> Index(String filename)
        {
            if (filename != null)
            {
                String content =
                    await this.servicefiles.GetFileContentAsync(filename);
                ViewData["CONTENT"] = content;
            }
            List<String> files =await this.servicefiles.GetFilesAsync();
            return View(files);
        }

        public IActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            String filename = file.FileName;
            using (var stream = file.OpenReadStream())
            {
                await this.servicefiles.UploadFileAsync(filename, stream);
            }
            ViewData["MENSAJE"] = "Archivo subido correctamente";
            return View();
        }

        public async Task<IActionResult> DeleteFile(String filename)
        {
            await this.servicefiles.DeleteFileAsync(filename);
            return RedirectToAction("Index");
        }
    }
}
