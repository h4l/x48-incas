using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BirthdayCake
{
    public class Temple
    {
        static Random rand;

        Vector2 position;
        Texture2D greyTexture;
        Texture2D goldTexture;
        Texture2D blueTexture;

        BoundingCircle boundingCircle;

        PlayerIndex owner;

        List<Peon> peons;

        bool frozen;
        TimeSpan freezeTimer;

        float prayer;
        public float Prayer
        {
            get{return prayer;}
        }
        const float MAX_PRAYER = 300.0f;
        public float MaxPrayer
        {
            get { return MAX_PRAYER; }
        }

        public Temple(PlayerIndex owner, Vector2 position, Texture2D goldtexture,Texture2D greytexture, Player player)
        {
            this.owner = owner;
            this.position = position;
            greyTexture = greytexture;
            goldTexture = goldtexture;
            blueTexture = Engine.ContentManager.Load<Texture2D>("templeBlue");

            boundingCircle = new BoundingCircle(position + new Vector2(greytexture.Width / 2, greytexture.Height / 2), 150);

            peons = new List<Peon>();

            rand = new Random(DateTime.Now.Millisecond);
        }

        public void Freeze()
        {
            freezeTimer = TimeSpan.Zero;
            frozen = true;
        }

        public void Update(GameTime t)
        {
            List<Peon> transfers = new List<Peon>();
            for (int i = 0; i < Engine.players[owner].attachedPeons.Count; i++)
            {
                if (Engine.players[owner].attachedPeons[i].BoundingCircle.intersects(this.boundingCircle))
                {
                    transfers.Add(Engine.players[owner].attachedPeons[i]);
                    Engine.players[owner].attachedPeons[i].Worship(position + new Vector2(goldTexture.Width / 2, goldTexture.Height / 2));
                    peons.Add(Engine.players[owner].attachedPeons[i]);
                }
            }
            for (int i = 0; i < transfers.Count; i++)
            {
                Engine.players[owner].attachedPeons.Remove(transfers[i]);
            }

            //if (owner == PlayerIndex.One)
            //{
                
            //}
            //else if (owner == PlayerIndex.Two)
            //{
            //    for (int i = 0; i < Engine.playerTwo.attachedPeons.Count; i++)
            //    {
            //        if (Engine.playerTwo.attachedPeons[i].BoundingCircle.intersects(this.boundingCircle))
            //        {
            //            transfers.Add(Engine.playerTwo.attachedPeons[i]);
            //            Engine.playerTwo.attachedPeons[i].Worship(position);
            //            peons.Add(Engine.playerTwo.attachedPeons[i]);
            //        }
            //    }
            //    for (int i = 0; i < transfers.Count; i++)
            //    {
            //        Engine.playerTwo.attachedPeons.Remove(transfers[i]);
            //    }
            //}

            if (!frozen)
            {
                for (int i = 0; i < peons.Count; i++)
                {
                    float slice = MathHelper.TwoPi / peons.Count;
                    float offset = ((float)t.TotalGameTime.TotalMilliseconds / 2000);
                    Vector2 worshipPos = new Vector2(135 * (float)Math.Cos((slice * i) + offset), 135 * (float)Math.Sin((slice * i) + offset));
                    peons[i].worshipPosition = this.position + new Vector2(125, 125) + worshipPos;

                    prayer += 0.5f * (float)t.ElapsedGameTime.TotalSeconds;
                }
            }
            else
            {
                freezeTimer += t.ElapsedGameTime;
                if (freezeTimer > TimeSpan.FromMilliseconds(5000))
                {
                    frozen = false;
                    freezeTimer = TimeSpan.Zero;
                }
            }

            if (prayer >= MAX_PRAYER)
            {
                Engine.winScreen.Show(owner);
            }
        }

        public void ReleaseAllPeons()
        {
            int peonCount = peons.Count;
            for (int i = 0; i < peonCount; i++)
            {
                peons[0].startWandering();
                peons.RemoveAt(0);
            }
        }

        public void Draw()
        {
            Engine.spriteBatch.Draw(greyTexture, position, Color.White);
            Vector2 half = new Vector2(goldTexture.Width / 2, goldTexture.Height / 2);
            float percent = (prayer / MAX_PRAYER);
            Engine.spriteBatch.Draw(frozen ? blueTexture : goldTexture, position, new Rectangle(0, 0, goldTexture.Width, (int)(goldTexture.Height * percent)), Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);
        }
    }
}
