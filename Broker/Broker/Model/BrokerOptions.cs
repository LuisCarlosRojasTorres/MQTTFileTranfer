using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Model
{
    public class BrokerOptions
    {
        public string Ip { get; set; }
        public int Port { get; set; }
        public string PublicKey { get; set; } 
        public string PrivateKey { get; set; }
        public string FileToTransfer { get; set; }
        public string DirectoryToTransfer { get; set; }

        public BrokerOptions()
        {
            this.Ip = "";
            this.Port = 6969;
            this.PublicKey = "";
            this.PrivateKey = "";
            this.FileToTransfer = "";
            this.DirectoryToTransfer = "";
        }


    }
}
