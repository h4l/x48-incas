using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;

namespace BirthdayCake
{
    public class WinScreen
    {
        const int EMITTER_COUNT = 50;
        
        Random rand;

        Texture2D winScreenBackground;

        ParticleEmitter[] emitters;
        ParticleSettings[] fireworks;

        Color[] colors = new Color[] { Color.Red, Color.Blue, Color.LimeGreen, Color.Yellow };

        Texture2D winner;

        bool show;

        int timer;

        public bool IsShowing
        {
            get { return show; }
        }

        public WinScreen()
        {
            rand = new Random(DateTime.Now.Millisecond);

            winScreenBackground = Engine.ContentManager.Load<Texture2D>("winScreenBackground");

            winner = Engine.ContentManager.Load<Texture2D>("playerOneWins");

            fireworks = new ParticleSettings[EMITTER_COUNT];
            for (int i = 0; i < EMITTER_COUNT; i++)
            {
                fireworks[i] = new ParticleSettings(Engine.graphics.GraphicsDevice, null, 1000);
                fireworks[i].LoadFromFile("Content/fireworks.psf", true);
                fireworks[i].MinSpeed = 32;
                fireworks[i].MaxSpeed = 64;
            }

            emitters = new ParticleEmitter[EMITTER_COUNT];
            for (int i = 0; i < EMITTER_COUNT; i++)
            {
                emitters[i] = new ParticleEmitter(Engine.spriteBatch, fireworks[i]);
                emitters[i].Position = new Vector2(rand.Next(1280), rand.Next(1024));
                emitters[i].TimeLimit = TimeSpan.FromMilliseconds(750);
                emitters[i].elapsed = TimeSpan.FromMilliseconds(751);
                emitters[i].SetColour2(colors[rand.Next(colors.Length)]);
            }
        }

        public void Show(PlayerIndex winner)
        {
            this.winner = Engine.ContentManager.Load<Texture2D>("player" + winner.ToString() + "Wins");
            show = true;
        }

        public void Update(GameTime t)
        {
            if (show)
            {
                for (int i = 0; i < EMITTER_COUNT; i++)
                {
                    emitters[i].Update(t);

                    if (rand.Next(250) == 10)
                    {
                        emitters[i].Position = new Vector2(rand.Next(1280), rand.Next(1024));
                        emitters[i].Reset();
                    }
                }
            }
        }

        public void Draw(GameTime t)
        {
            if (show)
            {
                Engine.spriteBatch.Draw(winScreenBackground, Vector2.Zero, Color.White);

                Engine.spriteBatch.End();
                Engine.spriteBatch.Begin(SpriteBlendMode.Additive);
                for (int i = 0; i < EMITTER_COUNT; i++)
                {
                    emitters[i].Draw(t);
                }
                Engine.spriteBatch.Draw(winner, Vector2.Zero, Color.White);
                Engine.spriteBatch.End();
                Engine.spriteBatch.Begin();

                timer += t.ElapsedGameTime.Milliseconds;

                if (timer > 4000)
                {
                    Application.Exit();
                }
            }
        }
    }
}
