using Humanizer;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria;

namespace Upstats.content.UI
{
    public class DraggableUIPanel : UIPanel
    {
        private bool dragging = false;
        private Vector2 offset;

        public override void LeftMouseDown(UIMouseEvent evt)
        {
            base.LeftMouseDown(evt);
            dragging = true;

            offset = evt.MousePosition - new Vector2(Left.Pixels, Top.Pixels);
        }

        public override void LeftMouseUp(UIMouseEvent evt)
        {
            base.LeftMouseUp(evt);
            dragging = false; // Stop dragging
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (dragging)
            {
                Vector2 newPosition = Main.MouseScreen - offset;

                Left.Set(MathHelper.Clamp(newPosition.X, 0, Main.screenWidth - Width.Pixels), 0f);
                Top.Set(MathHelper.Clamp(newPosition.Y, 0, Main.screenHeight - Height.Pixels), 0f);
                Recalculate();
            }
        }
    }
}
