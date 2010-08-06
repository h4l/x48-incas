using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BirthdayCake
{
    public class SplashScreen
    {
        public bool finished;

        Texture2D splash;

        public SplashScreen()
        {
            splash = Engine.ContentManager.Load<Texture2D>("cakeSplash");
        }

        public void Update(GameTime t)
        {
            if (t.TotalGameTime.Seconds == 3)
            {
                finished = true;
            }
        }

        public void Draw(GameTime t)
        {
            if (t.TotalGameTime.TotalMilliseconds < 1000)
            {
                Engine.graphics.GraphicsDevice.Clear(Color.Black);
                Engine.spriteBatch.Draw(splash, Vector2.Zero, new Color(Color.White, (float)t.TotalGameTime.TotalMilliseconds / 1000.0f));
            }
            else if ((t.TotalGameTime.TotalMilliseconds >= 1000) && (t.TotalGameTime.TotalMilliseconds < 2000))
            {
                 Engine.spriteBatch.Draw(splash, Vector2.Zero, new Color(Color.White, (float)t.TotalGameTime.TotalMilliseconds / 1000.0f));
            }
            else
            {
                Engine.spriteBatch.Draw(splash, Vector2.Zero, new Color(Color.White, (float)(4000 - t.TotalGameTime.TotalMilliseconds) / 1000.0f));
            }
        }
    }
}
