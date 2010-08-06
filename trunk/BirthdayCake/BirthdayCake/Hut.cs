using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BirthdayCake
{
    class Hut
    {
        Vector2 position;
        float spawnRadius;
        List<Peon> peonsAvaible;
        int maxPeons;
        float internalTimer;
        Engine game;
        Texture2D hutTexture;

        ParticleEmitter smokeEmitter;
        ParticleSettings smokeSettings;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public float SpawnRadius
        {
            get { return spawnRadius; }
        }
        public List<Peon> PeonsAvailable
        {
            get { return peonsAvaible; }
            set { peonsAvaible = value; }
        }
        public int MaxPeons
        {
            get { return maxPeons; }
        }
        
        public Hut(Vector2 position, int maxPeons, float spawnRadius, Engine game,Texture2D texture)
        {
            this.position = position;
            this.maxPeons = maxPeons;
            this.spawnRadius = spawnRadius;
            this.game = game;
            hutTexture = Engine.ContentManager.Load<Texture2D>("vilageSingle");

            smokeSettings = new ParticleSettings(Engine.graphics.GraphicsDevice, null, 1000);
            smokeSettings.LoadFromFile("Content/Smoke.psf", true);
            smokeSettings.StartColours[0] = new Color(Color.White, 0.5f);
            smokeSettings.EndColours[0] = new Color(Color.Gray, 0.5f);
            smokeEmitter = new ParticleEmitter(Engine.spriteBatch, smokeSettings);
            smokeEmitter.Position = this.position + new Vector2(61, 60);
            smokeEmitter.TimeLimit = TimeSpan.FromDays(1);
            peonsAvaible = new List<Peon>();
        }

        public void Update(GameTime t)
        {
            if (internalTimer > 45 && Engine.manager.Peons.Count <= maxPeons)
            {
                Random r = new Random();

                float tempX = r.Next((int)position.X - (int)spawnRadius, (int)position.X + (int)spawnRadius);
                float tempY = r.Next((int)position.Y - (int)spawnRadius, (int)position.Y + (int)spawnRadius);


                peonsAvaible.Add(new Peon(game, Engine.manager, Engine.ContentManager.Load<Texture2D>(@"spritePeon2"), new Vector2(tempX, tempY)));
                check();

                internalTimer = 0;
            }

            internalTimer++;

            smokeEmitter.Update(t);
            //foreach (Peon p in peonsAvaible)
            //{
            //    p.Update(t);
            //}
        }
        public void Draw(GameTime t)
        {
            //foreach (Peon p in peonsAvaible)
            //{
            //    p.Draw(t);
            //}
            Engine.spriteBatch.Draw(hutTexture,position,Color.White);
            smokeEmitter.Draw(t);
        }
        private bool check()
        {
            int temp = peonsAvaible.Count;
            for (int i = 0; i < temp - 1; i++)
            {
                if (peonsAvaible[i].BoundingCircle.intersects(peonsAvaible[temp - 1].BoundingCircle))
                {
                    peonsAvaible.RemoveAt(temp - 1);
                    return false;
                }
            }    
            return true;
            
        }

    }
}
