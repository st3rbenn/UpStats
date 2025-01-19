using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace Upstats.content.Experience
{
    public class EntityExperience
    {


        public static readonly List<EntityXP> SlimeEntityXP = new List<EntityXP>
        {
            new EntityXP(NPCID.BlueSlime, 2),
            new EntityXP(NPCID.YellowSlime, 2),
            new EntityXP(NPCID.RedSlime, 2),
            new EntityXP(NPCID.PurpleSlime, 2),
            new EntityXP(NPCID.GreenSlime, 2),
            new EntityXP(NPCID.IceSlime, 2),
            new EntityXP(NPCID.SandSlime, 3),
        };

        public static readonly List<EntityXP> SquirrelEntityXP = new List<EntityXP>
        {
            new EntityXP(NPCID.Squirrel, 1),
            new EntityXP(NPCID.SquirrelGold, 2),
            new EntityXP(NPCID.SquirrelRed, 2),
            new EntityXP(NPCID.GemSquirrelAmber, 7),
            new EntityXP(NPCID.GemSquirrelAmethyst, 7),
            new EntityXP(NPCID.GemSquirrelDiamond, 7),
            new EntityXP(NPCID.GemSquirrelEmerald, 7),
            new EntityXP(NPCID.GemSquirrelRuby, 7),
            new EntityXP(NPCID.GemSquirrelSapphire, 7),
            new EntityXP(NPCID.GemSquirrelTopaz, 7),
        };

        public static readonly List<EntityXP> BirdEntityXP = new List<EntityXP>
        {
            new EntityXP(NPCID.Bird, 1),
            new EntityXP(NPCID.BirdBlue, 1),
            new EntityXP(NPCID.BirdRed, 1),
            new EntityXP(NPCID.GoldBird, 5),
        };

        public static readonly List<EntityXP> SandBiomeEntityXP = new List<EntityXP>
        {
            new EntityXP(NPCID.Vulture, 3),
            new EntityXP(NPCID.Antlion, 3),
        };

        public static readonly List<EntityXP> EntityXP = new List<EntityXP>
        {
        };

        public static readonly List<EntityXP> allEntity = EntityXP
            .Concat(SlimeEntityXP)
            .Concat(SquirrelEntityXP)
            .Concat(BirdEntityXP)
            .Concat(SandBiomeEntityXP)
            .ToList();

        public static EntityXP GetEntityXPByEntityID(int entityID)
        {
            return allEntity.FirstOrDefault(x => x.Id == entityID); ;
        }
    }



    public class EntityXP
    {
        public int Id { get; set; }
        public int Experience { get; set; }

        public string Name { get; set; }

        public EntityXP(int id, int experience)
        {
            Id = id;
            Experience = experience;
        }
    }
}
