using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Upstats.content.Players;

namespace Upstats.content
{
    public class CustomGlobalNpc : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            PlayerStats playerStats = Main.LocalPlayer.GetModPlayer<PlayerStats>();
            if (playerStats == null) return;
            if (!npc.friendly && npc.life <= 0)
            {
                playerStats.OnNPCDefeated(npc);
            }
        }
    }
}
