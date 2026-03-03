using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    public enum VsyncSetting
    {
        ON,
        OFF,
        ADAPTIVE

    }

    public enum DisplayBorder
    {
        Resizable = 0,
        BorderLess = 1,
        Fixed = 2
    }

    public enum DisplayState
    {
        Normal = 0,
        Fullscreen = 1,
        Maximized = 2,
        Minimized = 3
    }
}
