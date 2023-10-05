using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PubSub.Model
{
    public class SftFile
    {
        public string ParentDirectory { get; set; }
        public string FileName { get; set; }
        public byte[] Payload { get; set; }
        public string HashMd5 { get; set; }
        public int NumOfTotalFiles { get; set; }
        public int Index { get; set; }

        public SftFile(string filePath, int numOfTotalFiles = 1, int index = 1 ) 
        {
            try {
                using (var stream = File.OpenRead(filePath))                
                {
                    using (var md5 = MD5.Create())
                    {                        
                        var hash = md5.ComputeHash(stream);
                        this.HashMd5 = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    }
                    this.FileName = filePath;
                    this.ParentDirectory = System.IO.Path.GetDirectoryName(filePath);
                    this.Payload = File.ReadAllBytes(filePath);
                    this.NumOfTotalFiles = numOfTotalFiles;
                    this.Index = index;
                }
            } 
            catch {
                this.FileName = "";
                this.ParentDirectory = "";
                this.Payload = new byte[1];
                this.HashMd5 = "";
                this.NumOfTotalFiles = 0;
                this.Index = 0;
            }
            
        }
    }
}
