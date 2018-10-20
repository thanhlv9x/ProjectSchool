using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication
{
    class InfoClient
    {
        public TcpClient Client { get; set; }
        public BinaryWriter Bw { get; set; }
    }
}
