using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinataNET.Models
{
    public class PinFileResponse
    {
        public string IpfsHash { get; set; }
        public int PinSize { get; set; }
        public string Timestamp { get; set; }
        public bool isDuplicate { get; set; }
    }
}
