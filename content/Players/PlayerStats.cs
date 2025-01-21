using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Upstats.content.Systems;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Upstats.content.Experience;

namespace Upstats.content.Players
{
    public class PlayerStats : ModPlayer
    {
        private HashSet<(int, int)> monitoredTiles = new HashSet<(int, int)>();
        private Dictionary<(int, int), ushort> monitoredTileTypes = new Dictionary<(int, int), ushort>();
        private int treeLengthTemp = 0;
        private Item? currentItemUsed;

        private List<Stat> _stats = new List<Stat>();

        public List<Stat> Stats { get { return _stats; } }

        public Stat GetStatById(int id)
        {
            return Stats.FirstOrDefault(x => x.Id == id);
        }

        public override void Initialize()
        {
            _stats = new List<Stat>
            {
                new Stat(1, "Mining", 1, 0),
                new Stat(2, "WoodCutting", 1, 0),
                new Stat(0, "-------------", 1, 0),
                new Stat(3, "melee", 1, 0),
            };
        }

        public override void OnEnterWorld()
        {
            Main.NewText($"Thank you for using UpStats v{Mod.Version} ! Made By St3rbenn", 122, 44, 255);
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            // Check if the keybind was just pressed
            if (KeybindSystem.OpenStats.JustPressed)
            {
                if (Upstats.StatsUI != null)
                {
                    Upstats.StatsUI.Visible = !Upstats.StatsUI.Visible;
                }
            }

            if (KeybindSystem.AddMinningLevel.JustPressed)
            {
                Stat minningStat = GetStatById(1);

                if (minningStat != null)
                {
                    minningStat.LevelUp();

                    Main.NewText($"added 1 level to minning");
                }
            }

            if (KeybindSystem.ResetStats.JustPressed)
            {
                foreach (Stat stat in Stats)
                {
                    stat.Reset();
                }
            }
        }

        public override void PostUpdate()
        {
            if (Player.controlUseItem && Player.itemAnimation > 0)
            {
                int targetX = Player.tileTargetX;
                int targetY = Player.tileTargetY;

                Tile tile = Main.tile[targetX, targetY];
                if (tile != null && tile.HasTile && !tile.IsActuated)
                {
                    if (!monitoredTiles.Contains((targetX, targetY)))
                    {
                        monitoredTiles.Add((targetX, targetY));
                        monitoredTileTypes[(targetX, targetY)] = tile.TileType;

                        if (IsTreeWood(tile.TileType))
                        {
                            int treeLength = CountTreeWood(targetX, targetY);
                            treeLengthTemp = treeLength;
                        }
                    }
                }
            }

            foreach (var tileCoords in monitoredTiles.ToList())
            {
                int x = tileCoords.Item1;
                int y = tileCoords.Item2;
                Tile tile = Main.tile[x, y];

                if (tile == null || !tile.HasTile)
                {
                    HandleMinningBlock(x, y, monitoredTileTypes[tileCoords]);
                    monitoredTiles.Remove(tileCoords);
                    monitoredTileTypes.Remove(tileCoords);
                }
            }
        }

        public void OnNPCDefeated(NPC target)
        {
            EntityXP entity = EntityExperience.GetEntityXPByEntityID(target.type);

            if (entity != null)
            {
                if (Player.HeldItem.DamageType == DamageClass.Melee || Player.HeldItem.DamageType == DamageClass.MeleeNoSpeed)
                {
                    GetStatById(3).AddExperience(entity.Experience);
                }

            }
        }


        private void HandleMinningBlock(int x, int y, ushort tileType)
        {
            Mod.Logger.Info($"Mining Tile at ({x}, {y}) - Type: {tileType} - IsTreeWood {IsTreeWood(tileType)}");
            Block block = BlockExperience.GetBlockXPByTileID(tileType);
            if (block != null)
            {
                if (IsTreeWood(tileType))
                {
                    int experience = CalculateTreeExperience(treeLengthTemp, block.Experience, 0.7);
                    GetStatById(2).AddExperience(experience);
                    treeLengthTemp = 0;
                }
                else
                {
                    GetStatById(1).AddExperience(block.Experience);
                }
            }
        }

        private int CalculateTreeExperience(int woodLength, double baseExperience, double scalingFactor)
        {
            return (int)Math.Floor(baseExperience * Math.Pow(woodLength, scalingFactor));
        }


        private int CountTreeWood(int startX, int startY)
        {
            int woodCount = 0;
            int y = startY;

            Mod.Logger.Info($"Starting wood count at X: {startX}, Y: {startY}");

            while (y > 0 && IsTreeWoodTile(startX, y))
            {
                woodCount++;
                y--; // Move upward
            }

            Mod.Logger.Info($"Total wood count: {woodCount} starting from X: {startX}, Y: {startY}");
            return woodCount;
        }

        private bool IsTreeWoodTile(int x, int y)
        {

            if (x < 0 || x >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
                return false; // Out of bounds

            Tile tile = Main.tile[x, y];
            return tile != null && IsTreeWood(tile.TileType);
        }

        private bool IsTreeWood(ushort tileType)
        {
            return TileID.Sets.IsATreeTrunk[tileType];
        }

        #region DataSaving

        public override void SaveData(TagCompound tag)
        {
            var statsTagList = new List<TagCompound>();
            foreach (var stat in _stats)
            {
                statsTagList.Add(stat.Save());
            }
            tag["StatsList"] = statsTagList;
            Main.NewText($"Stats Saved !");
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("StatsList"))
            {
                var statsTagList = tag.GetList<TagCompound>("StatsList");
                _stats.Clear();
                foreach (var statTag in statsTagList)
                {
                    Stat newStat = Stat.Load(statTag);
                    Main.NewText($"Stat: {newStat.Name}, level: {newStat.Level}");
                    _stats.Add(newStat);
                }
            }
        }

        #endregion
    }



    public class Stat
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public int Experience { get; private set; }
        public int Level { get; private set; } = 1;
        public int ExperienceToNextLevel { get; private set; } = 100;

        private const int BaseXP = 100;
        private const double ScalingFactor = 1.5;

        public Stat(int id, string name, int level, int experience)
        {
            Id = id;
            Name = name;
            Level = level;
            Experience = experience;
        }

        public void AddExperience(int amount)
        {
            Main.NewText($"Added {amount} experience to {Name} skill");
            Experience += amount;

            // Check for level-ups
            while (Experience >= ExperienceToNextLevel)
            {
                Experience -= ExperienceToNextLevel;
                LevelUp();
            }
        }

        public void LevelUp()
        {
            Level++;
            ExperienceToNextLevel = CalculateNextLevelXP(Level);
            OnLevelUp();
        }

        private int CalculateNextLevelXP(int level)
        {
            // Use exponential scaling as an example
            return (int)(BaseXP * Math.Pow(ScalingFactor, level - 1));
        }

        private void OnLevelUp()
        {
            // Example: Unlock a new skill or notify the player
            Color color = Color.Fuchsia;
            ShowPopupText($"Level Up! New Level: {Level}", color);
            SoundEngine.PlaySound(SoundID.AchievementComplete);
        }

        public void Reset()
        {
            Level = 1;
            Experience = 0;

            Save();
            Main.NewText($"{Name} Stat reset !");
        }
        public void ShowPopupText(string message, Color color)
        {

            int playerX = (int)Main.LocalPlayer.position.X;
            int playerY = (int)Main.LocalPlayer.position.Y;

            int popupX = playerX;
            int popupY = playerY - 20;

            CombatText.NewText(new Rectangle(popupX, popupY, 0, 0), color, message);
        }




        public TagCompound Save()
        {
            return new TagCompound
            {
                { "Id", Id },
                { "Name", Name },
                { "Level", Level },
                { "Experience", Experience }
            };
        }

        public static Stat Load(TagCompound tag)
        {
            return new Stat(
                tag.GetInt("Id"),
                tag.GetString("Name"),
                tag.GetInt("Level"),
                tag.GetInt("Experience")
            );
        }
    }
}
