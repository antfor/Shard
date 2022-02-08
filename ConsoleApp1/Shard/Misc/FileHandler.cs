using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Misc
{
    public class FileHandler
    {

        public static byte[] fileTobyte(string file) {

            Stream stream = File.Open(file, FileMode.Open);

            if (stream == null)
                throw new ArgumentNullException("stream");

            BinaryReader reader = new BinaryReader(stream);

            return reader.ReadBytes((int)reader.BaseStream.Length); ;
        }
    }
}
