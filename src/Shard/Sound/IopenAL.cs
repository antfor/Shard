using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Sound
{
    public interface IopenAL : ISound
    {
        internal int Id { get; set; }

        internal void stop(string id);
        internal void loadSound(string id, int sound);
    }
}
