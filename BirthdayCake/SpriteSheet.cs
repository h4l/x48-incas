using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BirthdayCake
{
    public class SpriteSheet
    {
        public Vector2 Position { get; set; }

        public Vector2 Origin { get; set; }

        public Vector2 Scale { get; set; }

        public float Rotation { get; set; }

        public Texture2D Texture { get; set; }

        public Color Tint { get; set; }

        public float FPS { get; set; }

        public Vector2 FrameSize { get; set; }

        public int CurrentFrame { get; protected set; }

        public int FrameCount { get; protected set; }

        public float Opacity
        {
            get { return Tint.A / 255; }
            set { Tint = new Color(Tint.R, Tint.G, Tint.B, (byte)(255 * value)); }
        }

        int internalTimer;

        public SpriteSheet(Texture2D texture, Vector2 frameSize, float fps)
        {
            this.Texture = texture;
            this.FrameSize = frameSize;
            this.FPS = fps;
            this.Tint = Color.White;
            this.Scale = Vector2.One;
            //Opacity = 0.1f;

            CurrentFrame = 0;
            FrameCount = (int)((texture.Width / frameSize.X));// + (texture.Height / frameSize.Y));
        }

        public void Update(GameTime t)
        {
            internalTimer += (int)t.ElapsedGameTime.TotalMilliseconds;
            internalTimer = internalTimer % (int)(FrameCount * (1000.0f / FPS));
            CurrentFrame = (int)(FPS * ((float)internalTimer / 1000.0f));
        }

        public void Draw()
        {
            int x = (int)((FrameSize.X * CurrentFrame) % Texture.Width);
            int y = (int)((int)((FrameSize.X * CurrentFrame) / Texture.Width) * FrameSize.X);
            Engine.spriteBatch.Draw(Texture, Position, new Rectangle(x, y, (int)FrameSize.X, (int)FrameSize.Y), Tint, Rotation, Origin, Scale, SpriteEffects.None,0);
        }
    }
}
