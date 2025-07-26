using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Common.Services
{
    public class Attachment : IAttachment
    {
        #region AllowedExtensions

        private readonly List<string> AllowedExtensions = new()
        {
            ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".docx", ".xlsx"
        };

        #endregion

        private const int MaxFileSizeInBytes = 2_097_152; // 2 MB  /*5 * 1024 * 1024*/ =  // 5 MB


        #region UploadFile
        public string UploadFile(IFormFile file, string FolderName)
        {
            #region Validations

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Invalid File Extension Please Try Again");
            }
            if (file.Length > MaxFileSizeInBytes)
            {
                throw new Exception("File Size Exceeded 2 MB");
            }


            #endregion

            // Get Located Folder Path;

            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot\\Files\\Images", FolderName);

            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            // Generate Unique File Name
            var FileName = $"{Guid.NewGuid()}{fileExtension}";
            var FilePath = Path.Combine(FolderPath, FileName);
            // Save the file
            using var stream = new FileStream(FilePath, FileMode.Create);
                file.CopyTo(stream);
            
            return FileName;
        } 

        #endregion

        #region DeleteFile
        public bool DeleteFile(string FilePath)
        {
            if(File.Exists(FilePath))
            {
                File.Delete(FilePath);
                return true;
            }
            return false;

        } 

        #endregion
    }
}
