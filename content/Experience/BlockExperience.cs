using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Terraria.ID;

namespace Upstats.content.Experience
{

    public static class BlockExperience
    {
        public static readonly List<Block> TreeBlockXP = new List<Block>
        {
            new Block(TileID.Trees, 2),
            new Block(TileID.MushroomTrees, 2),
            new Block(TileID.TreeTopaz, 2),
            new Block(TileID.TreeAmethyst, 2),
            new Block(TileID.TreeSapphire, 2),
            new Block(TileID.TreeEmerald, 2),
            new Block(TileID.TreeRuby, 2),
            new Block(TileID.TreeDiamond, 2),
            new Block(TileID.TreeAmber, 2),
            new Block(TileID.VanityTreeSakura, 2),
            new Block(TileID.VanityTreeYellowWillow, 2),
            new Block(TileID.TreeAsh, 2),
        };


        public static readonly List<Block> BlockXP = new List<Block>
        {
            new Block(TileID.Dirt, 1),
            new Block(TileID.Grass, 1),
            new Block(TileID.Stone, 3),
        };

        public static readonly List<Block> allBlocks = BlockXP.Concat(TreeBlockXP).ToList();


        public static Block GetBlockXPByTileID(int tileID)
        {
            return allBlocks.FirstOrDefault(x => x.Id == tileID);
        }
    }


    public class Block
    {
        public int Id { get; set; }
        public int Experience { get; set; }

        public Block(int id, int experience)
        {
            Id = id;
            Experience = experience;
        }
    }
}
