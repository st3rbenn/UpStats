using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Upstats.content.Systems
{
    public class KeybindSystem : ModSystem
    {
        public static ModKeybind OpenStats { get; private set; }
        public static ModKeybind AddMinningLevel { get; private set; }
        public static ModKeybind ResetStats { get; private set; }

        public override void Load()
        {
            OpenStats = KeybindLoader.RegisterKeybind(Mod, "OpenStats", "P");
            AddMinningLevel = KeybindLoader.RegisterKeybind(Mod, "AddMinningLevel", "l");
            ResetStats = KeybindLoader.RegisterKeybind(Mod, "ResetStats", "m");
        }

        public override void Unload()
        {
            OpenStats = null;
            AddMinningLevel = null;
            ResetStats = null;
        }
    }
}
