using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.NotesModel
{
    public class AddFileToFormFile
    {
       public ICollection<IFormFile> AddFileToFormFileMethod(string fileName)
        {
            string stringValue = fileName;

            // Read the file contents as a byte array
            byte[] fileBytes = File.ReadAllBytes(stringValue);

            // Create a stream from the byte array
            Stream fileStream = new MemoryStream(fileBytes);

            // Create a FormFile instance from the stream, file name, and content type
            IFormFile fileValue = new FormFile(fileStream, 0, fileBytes.Length, "file", stringValue);

            // Create a collection of files with one element
            ICollection<IFormFile> filesValue = new List<IFormFile>() { fileValue };
            return filesValue;
        }


        public string ToDb(ICollection<IFormFile> files)
        {
            ICollection<IFormFile> filesValue = files;

            // Create a list of file names
            List<string> fileNames = new List<string>();
            foreach (var file in filesValue)
            {
                // Get the file name from the file path
                string fileName = Path.GetFileName(file.FileName);
                fileNames.Add(fileName);
            }

            // Join the file names with a comma
            string stringValue = string.Join(", ", fileNames);
            return stringValue;
        }

    }
}
