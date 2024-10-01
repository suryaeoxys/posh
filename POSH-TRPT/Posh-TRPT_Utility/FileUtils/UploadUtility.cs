using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Posh_TRPT_Utility.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Utility.FileUtils
{
    public static class UploadUtility
    {
        public static HttpContext _httpContext => new HttpContextAccessor().HttpContext!;
        private static  IWebHostEnvironment _environment=>(IWebHostEnvironment)_httpContext.RequestServices.GetService(typeof(IWebHostEnvironment))!;
        public static string ProfilePhotoUpload(IFormFile profilePhoto)
        {
            try
            {
                string wwwPath = _environment.WebRootPath;
                string connectionPath = _environment.ContentRootPath;
                string path = Path.Combine(wwwPath, GlobalResourceFile.ProfilePic);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = Path.GetFileName(Guid.NewGuid()+profilePhoto.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    profilePhoto.CopyTo(stream);
                    return fileName;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string DocumentsUpload(IFormFile? documentPhoto)
        {
            if (documentPhoto is not null)
            {
                try
                {
                    string wwwPath = _environment.WebRootPath;
                    string connectionPath = _environment.ContentRootPath;
                    string path = Path.Combine(wwwPath, GlobalResourceFile.UploadDocument);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                }
                    string fileName = Path.GetFileName(Guid.NewGuid() + documentPhoto.FileName);
                    using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        documentPhoto.CopyTo(stream);
                        return  fileName;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return null!;
            }
        }
    }
}
