using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Upstats.content.Players;
using Upstats.content.UI;

namespace Upstats
{
    public class Upstats : ModSystem
    {
        public static StatsInfoDisplay StatsUI;
        internal UserInterface _statsInterface;
        private GameTime _lastUpdateUiGameTime;


        public override void Load()
        {
            if (!Main.dedServ)
            {
                _statsInterface = new UserInterface();

                StatsUI = new StatsInfoDisplay();
                StatsUI.Activate();

                ShowMyUI();
            }

        }

        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (_statsInterface?.CurrentState != null)
            {
                _statsInterface.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Fancy UI"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "UpStats: _statsInterface",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && _statsInterface?.CurrentState != null)
                        {
                            _statsInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }

        internal void ShowMyUI()
        {
            _statsInterface?.SetState(StatsUI);
        }

        internal void HideMyUI()
        {
            _statsInterface?.SetState(null);
        }

        public override void Unload()
        {
            base.Unload();

            // Unload UI resources
            StatsUI = null;
            _statsInterface = null;
        }
    }
}
