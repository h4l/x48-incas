using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BirthdayCake
{
    public enum PowerupType { SPEED, RADIUS, NUKE, FREEZE } 

    public class Powerup
    {
        public Texture2D texture;

        PowerupType type;

        public Vector2 position;

        BoundingCircle bounds;

        public bool removeMe;

        DateTime createTime;

        public Powerup(Vector2 position, PowerupType type)
        {
            this.type = type;
            this.position = position;

            createTime = DateTime.Now;

            switch (type)
            {
                case PowerupType.NUKE:
                    texture = Engine.ContentManager.Load<Texture2D>("ankh");
                    break;

                case PowerupType.RADIUS:
                    texture = Engine.ContentManager.Load<Texture2D>("flute");
                    break;

                case PowerupType.SPEED:
                    texture = Engine.ContentManager.Load<Texture2D>("goat");
                    break;

                case PowerupType.FREEZE:
                    texture = Engine.ContentManager.Load<Texture2D>("freeze");
                    break;
            }

            //bounds = new BoundingBox(new Vector3(position, 0), new Vector3(position + new Vector2(texture.Width, texture.Height), 0));
            bounds = new BoundingCircle(this.position + (new Vector2(texture.Width, texture.Height) / 2), texture.Width / 2);
        }

        public void Update(GameTime t)
        {
            foreach (KeyValuePair<PlayerIndex, Player> p in Engine.players)
            {
                if (p.Value.BoundingCircle.intersects(this.bounds))
                {
                    switch (type)
                    {
                        case PowerupType.NUKE:
                            foreach (KeyValuePair<PlayerIndex, Temple> temple in Engine.temples)
                            {
                                if (temple.Key != p.Key)
                                {
                                    temple.Value.ReleaseAllPeons();
                                }
                            }
                            break;

                        case PowerupType.RADIUS:
                            p.Value.grantRadiusBoost();
                            break;

                        case PowerupType.SPEED:
                            p.Value.grantSpeedBoost();
                            break;

                        case PowerupType.FREEZE:
                            foreach (KeyValuePair<PlayerIndex, Temple> temple in Engine.temples)
                            {
                                if (temple.Key != p.Key)
                                {
                                    temple.Value.Freeze();
                                }
                            }
                            break;
                    }

                    removeMe = true;
                }
            }
        }

        public void Draw()
        {
            Engine.spriteBatch.Draw(texture, position, new Color(Color.White, MathHelper.Clamp((float)(DateTime.Now - createTime).TotalMilliseconds / 2000, 0, 1)));
        }
    }
}
