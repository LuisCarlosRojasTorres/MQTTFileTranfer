using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class BrokerOptions
    {
        public string Ip { get; set; }
        public int Port { get; set; }
        public int ConnectionTimeout { get; set; }
        public string Topic { get; set; }
        public string FileToTransfer { get; set; }
        public string DirectoryToTransfer { get; set; }        


    }
}
