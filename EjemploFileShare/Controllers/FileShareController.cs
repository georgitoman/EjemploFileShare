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
        ServiceFileShare Service;

        public FileShareController(ServiceFileShare servicefileshare)
        {
            this.Service = servicefileshare;
        }

        public async Task<IActionResult> Index(String filename)
        {
            if (filename != null)
            {
                String content =
                    await this.Service.GetFileContentAsync(filename);
                ViewData["TEXTO"] = content;
            }
            List<String> files =await this.Service.GetFilesAsync();
            return View(files);
        }

        public IActionResult Subir()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Subir(IFormFile file)
        {
            String filename = file.FileName;
            using (var stream = file.OpenReadStream())
            {
                await this.Service.UploadFileAsync(filename, stream);
            }
            return View();
        }

        public async Task<IActionResult> Eliminar(String filename)
        {
            await this.Service.DeleteFileAsync(filename);
            return RedirectToAction("Index");
        }
    }
}
