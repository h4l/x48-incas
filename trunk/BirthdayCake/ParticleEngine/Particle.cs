using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BirthdayCake
{
    public class Particle
    {
        static SpriteBatch s;

        Texture2D tex;
        public Texture2D Texture
        {
            get { return tex; }
            set 
            { 
                tex = value;
                origin = new Vector2((float)tex.Width / 2, (float)tex.Height / 2);
            }
        }

        Vector2 origin;
        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        Vector2 scale = Vector2.One;
        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        Vector2 gravity;
        public Vector2 Gravity
        {
            get { return gravity; }
            set { gravity = value; }
        }

        Vector2 targetScale = Vector2.One;
        public Vector2 TargetScale
        {
            get { return targetScale; }
            set { targetScale = value; }
        }

        Color startColour;
        public Color StartColour
        {
            get { return startColour; }
            set 
            { 
                startColour = value;
                updateColorDiff();
            }
        }

        Color endColour;
        public Color EndColour
        {
            get { return endColour; }
            set 
            { 
                endColour = value;
                updateColorDiff();
            }
        }

        Vector3 colorDiff;

        Color tint;
        public Color Tint
        {
            get { return tint; }
        }

        Vector2 pos;
        public Vector2 Position
        {
            get { return pos; }
            set { pos = value; }
        }

        Vector2 velocity;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        float rotationalVelocity;
        public float RotationalVelocity
        {
            get { return rotationalVelocity; }
            set { rotationalVelocity = value; }
        }

        float rot;
        public float Rotation
        {
            get { return rot; }
            set { rot = MathHelper.WrapAngle(value); }
        }

        TimeSpan timeAlive;
        public TimeSpan TimeAlive
        {
            get { return timeAlive; }
        }

        float life;
        public float Life
        {
            get { return life; }
        }

        float ttl;
        public float TimeToLive
        {
            get { return ttl; }
            set { ttl = value; }
        }

        bool isAlive;
        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        public Particle(SpriteBatch sprite)
        {
            s = sprite;
        }

        public void Update(GameTime t)
        {
            float time = (float)(t.ElapsedGameTime.Milliseconds + 1) / 1000;
            timeAlive += t.ElapsedGameTime;
            pos += velocity * time;

            velocity += gravity * time;

            rot += rotationalVelocity * time;

            life = MathHelper.Clamp((float)timeAlive.TotalSeconds / ttl, 0.0f, 1.0f);

            scale = Vector2.Lerp(Vector2.One, targetScale, life);

            tint = new Color(
                (startColour.R + (colorDiff.X * life))/255, 
                (startColour.G + (colorDiff.Y * life))/255, 
                (startColour.B + (colorDiff.Z * life))/255, 
                1.0f - life
                );

            if (timeAlive.Seconds > ttl)
            {
                isAlive = false;
                timeAlive = new TimeSpan();
            }
        }

        void updateColorDiff()
        {
            colorDiff = new Vector3(endColour.R - startColour.R, endColour.G - startColour.G, endColour.B - startColour.B);
        }

        public void Draw(GameTime t)
        {
            if (isAlive)
            {
                s.Draw(tex, pos, null, tint, rot, origin, scale, SpriteEffects.None, 0);
            }
        }
    }
}
