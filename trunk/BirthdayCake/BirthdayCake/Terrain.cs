using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Colour = Microsoft.Xna.Framework.Graphics.Color;

namespace BirthdayCake
{
    public class Terrain : DrawableGameComponent
    {
        List<ParticleEmitter> powerupEmitters;
        ParticleSettings powerupSettings;

        Random rand;

        Texture2D terrain;

        Texture2D waterOne;
        Texture2D waterTwo;

        Texture2D collisionMap;
        bool[] collisionData;

        List<Powerup> powerups;

        public Terrain(Game g)
            : base(g)
        {
            terrain = Engine.ContentManager.Load<Texture2D>("mapTop");
            collisionMap = Engine.ContentManager.Load<Texture2D>("collisionMap");

            powerups = new List<Powerup>();

            waterOne = Engine.ContentManager.Load<Texture2D>("waterOne");
            waterTwo = Engine.ContentManager.Load<Texture2D>("waterTwo");

            Color[] collisionColours = new Color[collisionMap.Width * collisionMap.Height];
            collisionData = new bool[collisionMap.Width * collisionMap.Height];
            collisionMap.GetData<Color>(collisionColours);

            for (int i = 0; i < collisionColours.Length; i++)
            {
                collisionData[i] = collisionColours[i] != Colour.Black;
            }

            rand = new Random(DateTime.Now.Millisecond);

            powerupSettings = new ParticleSettings(Engine.graphics.GraphicsDevice, null, 1000);
            powerupSettings.LoadFromFile("Content/powerup.psf", true);

            powerupEmitters = new List<ParticleEmitter>(10);
        }

        public override void Update(GameTime t)
        {
            Vector2 powerupPosition = Vector2.Zero;
            while (CheckCollision(new Rectangle((int)powerupPosition.X, (int)powerupPosition.Y, 90, 90)))
            {
                powerupPosition = new Vector2(rand.Next(100, 1180), rand.Next(100, 924));
                powerupPosition -= new Vector2(35, 35);
            }

            if (powerups.Count < 3)
            {
                if (rand.Next(5000) == 1)
                {
                    powerups.Add(new Powerup(powerupPosition, PowerupType.NUKE));
                }
                if (rand.Next(1000) == 1)
                {
                    powerups.Add(new Powerup(powerupPosition, PowerupType.SPEED));
                }
                if (rand.Next(1000) == 1)
                {
                    powerups.Add(new Powerup(powerupPosition, PowerupType.FREEZE));
                }
                if (rand.Next(500) == 1)
                {
                    powerups.Add(new Powerup(powerupPosition, PowerupType.RADIUS));
                }
            }

            List<Powerup> removeList = new List<Powerup>();
            for (int i = 0; i < powerups.Count; i++)
            {
                powerups[i].Update(t);
                if (powerups[i].removeMe)
                {
                    removeList.Add(powerups[i]);
                }
            }
            for (int i = 0; i < removeList.Count; i++)
            {
                powerupEmitters.Add(new ParticleEmitter(Engine.spriteBatch, powerupSettings));
                powerupEmitters[powerupEmitters.Count-1].Position = removeList[i].position + new Vector2(removeList[i].texture.Width / 2, removeList[i].texture.Height / 2);
                powerupEmitters[powerupEmitters.Count-1].TimeLimit = TimeSpan.FromMilliseconds(700);

                powerups.Remove(removeList[i]);
            }

            for (int i = 0; i < powerupEmitters.Count; i++)
            {
                powerupEmitters[i].Update(t);
            }
        }

        public bool CheckCollision(Rectangle r)
        {
            for (int x = r.Left; x < r.Right; x++)
            {
                for (int y = r.Top; y < r.Bottom; y++)
                {
                    if (CheckCollision(x, y))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool CheckCollision(Vector2 pos)
        {
            return CheckCollision((int)Math.Round(pos.X), (int)Math.Round(pos.Y));
        }

        public bool CheckCollision(int x, int y)
        {
            int i = x + (y * collisionMap.Width);
            if (i >= collisionData.Count()) { return true; }
            if (i < 0) { return true; }

            return collisionData[i];
        }

        public override void Draw(GameTime t)
        {
            Engine.spriteBatch.Draw(waterOne, Vector2.Zero, Color.White);
            Engine.spriteBatch.Draw(waterTwo, Vector2.Zero, new Color(Color.White, ((float)Math.Sin(t.TotalGameTime.TotalMilliseconds / 700) + 1) / 2));
            Engine.spriteBatch.Draw(terrain, Vector2.Zero, Color.White);

            for (int i = 0; i < powerups.Count; i++)
            {
                powerups[i].Draw();
            }

            Engine.spriteBatch.End();
            Engine.spriteBatch.Begin(SpriteBlendMode.Additive);
            for (int i = 0; i < powerupEmitters.Count; i++)
            {
                powerupEmitters[i].Draw(t);
            }
            Engine.spriteBatch.End();
            Engine.spriteBatch.Begin();
        }
    }
}
