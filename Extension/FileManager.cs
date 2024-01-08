namespace WebApplication9.Extension
{
    public static class FileManager
    {
        public static string CreateImage(this IFormFile file, IWebHostEnvironment env, params string[] folders)
        {
            string fileName = $"{Guid.NewGuid()}_{DateTime.Now.ToString("yyyyMMddHHmmssffff")}_{file.FileName}";

            string path = env.WebRootPath;
            
            foreach (string folder in folders)
            {
                path = Path.Combine(path, folder);
            }

            path = Path.Combine(path, fileName);

            using (FileStream fileStream = new FileStream(path,FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return fileName;
        }

        public static bool CheckFileSize(this IFormFile file, double size)
        {
            return Math.Round((double)file.Length / 1024) < size;
        }

        public static Dictionary<string, List<byte[]>> fileSignatures = new Dictionary<string, List<byte[]>>
        {
            { ".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                }
            },
            { ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                }
            },
            { ".png", new List<byte[]>
                {
                    new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A },
                }
            },
        };


        public static bool CheckFileType(this IFormFile file)
        {
            byte[] fileContent;
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                fileContent = memoryStream.ToArray();
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (fileSignatures.TryGetValue(fileExtension, out var signatures))
            {
                foreach (var signature in signatures)
                {
                    bool matchesSignature = true;
                    for (int i = 0; i < signature.Length; i++)
                    {
                        if (fileContent.Length <= i || fileContent[i] != signature[i])
                        {
                            matchesSignature = false;
                            break;
                        }
                    }
                    if (matchesSignature)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
