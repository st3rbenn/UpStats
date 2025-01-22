using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Upstats.content.Players;

namespace Upstats.content.UI
{
    public class StatsInfoDisplay : UIState
    {
        private UIPanel panel;
        private UIText titleText;
        private UIList statsList;
        private UIScrollbar scrollbar;
        private UIButton closeButton;

        public bool Visible { get; set; } = false;

        public override void OnInitialize()
        {
            panel = new UIPanel();
            panel.Width.Set(0, 0.18f); // 15% of the screen width
            panel.Height.Set(0, 0.40f); // 40% of the screen height

            // Semi-transparent, glassy background color
            panel.BackgroundColor = new Color(0, 0, 0, 128); // Black with 50% opacity
            panel.BorderColor = new Color(255, 255, 255, 100); // White border with some transparency

            // Align the panel under the minimap
            panel.HAlign = 1f; // Align to the far-right (same as the minimap)
            panel.VAlign = 0f; // Align to the top of the screen
            panel.Top.Set(0, 0.4f); // Adjust vertical offset to place it under the map

            Append(panel);

            // Title Text
            titleText = new UIText("Player Stats", 1.2f);
            titleText.HAlign = 0.5f;
            titleText.Top.Set(10, 0f);
            panel.Append(titleText);

            // Close Button
            closeButton = new UIButton("X", 1f);
            closeButton.Width.Set(20, 0f);
            closeButton.Height.Set(20, 0f);
            closeButton.Left.Set(-30, 1f); // Position relative to the panel's right edge
            closeButton.Top.Set(10, 0f);
            closeButton.OnLeftClick += (evt, element) => { Visible = false; };
            panel.Append(closeButton);

            // Stats List
            statsList = new UIList();
            statsList.Width.Set(-25, 1f); // Full width minus 25 pixels for padding
            statsList.Height.Set(-60, 1f); // Full height minus space for title and padding
            statsList.Top.Set(40, 0f);
            statsList.SetPadding(5);
            panel.Append(statsList);

            // Scrollbar
            scrollbar = new UIScrollbar();
            scrollbar.Width.Set(20, 0f);
            scrollbar.Height.Set(-60, 1f); // Matches statsList height
            scrollbar.Top.Set(40, 0f);
            scrollbar.HAlign = 1f;
            panel.Append(scrollbar);

            statsList.SetScrollbar(scrollbar);
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!Visible)
            {
                Remove();
                return;
            }

            PlayerStats playerStats = Main.LocalPlayer.GetModPlayer<PlayerStats>();
            if (playerStats == null) return;

            statsList.Clear();
            foreach (var stat in playerStats.Stats)
            {
                string text = $"{stat.Name} {(stat.Id != 0 ? $": Level {stat.Level} (XP: {stat.Experience})" : "")}";
                var statText = new UIText(text);
                statText.Width.Set(0, 1f);
                statText.Height.Set(30, 0f);
                statsList.Add(statText);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
                base.Draw(spriteBatch);
        }
    }

    // Simple button for UI
    public class UIButton : UIText
    {
        private Color _defaultColor = Color.White;
        private Color _hoverColor = Color.Yellow;

        public UIButton(string text, float textScale = 1f) : base(text, textScale)
        {
            TextColor = _defaultColor;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Check if the mouse is hovering over the button
            if (IsMouseHovering)
            {
                TextColor = _hoverColor; // Change color when hovered
            }
            else
            {
                TextColor = _defaultColor; // Reset color
            }
        }
    }
}
