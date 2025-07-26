using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Common.Services
{
    public interface IAttachment
    {
        string UploadFile(IFormFile file, string FolderName);
        bool DeleteFile(string FilePath);
    }
}
