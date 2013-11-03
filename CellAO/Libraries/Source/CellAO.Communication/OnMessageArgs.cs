using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellAO.Communication
{
    [Serializable]
    public class OnMessageArgs
    {
        public short Length;
        public byte ID;
        public byte[] Data;

        public bool IsProtocolPacket;
    }
}
