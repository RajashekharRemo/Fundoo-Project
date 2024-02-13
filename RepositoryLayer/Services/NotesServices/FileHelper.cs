using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services.NotesServices
{
    public class FileHelper
    {
        private static readonly string BaseDirectory = @"F:\NoteImages";
        public static string GetFilePath(string filePath)
        {
            string directoryPath = BaseDirectory;
            if (Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            return Path.Combine(directoryPath, filePath);
        }
    }
}
