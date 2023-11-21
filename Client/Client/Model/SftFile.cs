using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PubSub.Model
{
    public class SftFile
    {
        public string FileName { get; set; }
        public byte[] Payload { get; set; }
        public string HashMd5 { get; set; }
        public int NumOfTotalFiles { get; set; }
        public int Index { get; set; }

        public SftFile()
        {
            this.FileName = "";
            this.Payload = new byte[1];
            this.HashMd5 = "";
            this.NumOfTotalFiles = 0;
            this.Index = 0;
        }

        public SftFile(string filePath, int numOfTotalFiles = 1, int index = 1 ) 
        {
            try {
                    this.FileName = filePath;        
                    this.Payload = File.ReadAllBytes(filePath);
                    this.HashMd5 = this.CalculateHashMd5(filePath);
                    this.NumOfTotalFiles = numOfTotalFiles;
                    this.Index = index;
                }            
            catch {
                this.FileName = "";
                this.Payload = new byte[1];
                this.HashMd5 = "";
                this.NumOfTotalFiles = 0;
                this.Index = 0;
            }            
        }

        public string CalculateHashMd5(string filename)
        {
            try {
                using (var stream = File.OpenRead(filename))
                {
                    using (var md5 = MD5.Create())
                    {
                        var hash = md5.ComputeHash(stream);
                        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    }
                }
            }
            catch {
                return "";
            }
            
            
        }
    }
}
