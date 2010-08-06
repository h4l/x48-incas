using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace BirthdayCake
{
    public class ParticleSettings
    {
        static GraphicsDevice g;
        public static GraphicsDevice Graphics
        {
            get { return g; }
            set { g = value; }
        }

        int maxParticles;
        public int MaxParticles
        {
            get { return maxParticles; }
        }

        float createSpeed = 10;
        public float CreateSpeed
        {
            get { return createSpeed; }
            set { createSpeed = value; }
        }

        float minSpeed = 0.0f; // pixels per second
        public float MinSpeed
        {
            get { return minSpeed; }
            set { minSpeed = value; }
        }

        float maxSpeed = 64.0f; // pixels per second
        public float MaxSpeed
        {
            get { return maxSpeed; }
            set { maxSpeed = value; }
        }

        float minRotationalVelocity = 0.0f; // radians per second
        public float MinRotationalVelocity
        {
            get { return minRotationalVelocity; }
            set { minRotationalVelocity = value; }
        }

        float maxRotationalVelocity = (float)Math.PI; //radians per second
        public float MaxRotationalVelocity
        {
            get { return maxRotationalVelocity; } 
            set { maxRotationalVelocity = value; }
        }

        Vector2 minTargetScale = Vector2.One;
        public Vector2 MinTargetScale
        {
            get { return minTargetScale; }
            set { minTargetScale = value; }
        }

        Vector2 maxTargetScale = Vector2.One;
        public Vector2 MaxTargetScale 
        {
            get { return maxTargetScale; }
            set { maxTargetScale = value; }
        }

        float minStartRadius = 0.0f;
        public float MinStartRadius
        {
            get { return minStartRadius; }
            set { minStartRadius = value; }
        }

        float maxStartRadius = 0.0f;
        public float MaxStartRadius
        {
            get { return maxStartRadius; }
            set { maxStartRadius = value; }
        }

        float minLifetime = 0.0f;
        public float MinLifetime
        {
            get { return minLifetime; }
            set { minLifetime = value; }
        }

        float maxLifetime = 2.5f; // in seconds
        public float MaxLifetime
        {
            get { return maxLifetime; }
            set { maxLifetime = value; }
        }

        List<Texture2D> textures;
        public List<Texture2D> Textures
        {
            get { return textures; }
        }

        List<Color> startColours = new List<Color>();
        public List<Color> StartColours
        {
            get { return startColours; }
            set { startColours = value; }
        }

        List<Color> endColours = new List<Color>();
        public List<Color> EndColours
        {
            get { return endColours; }
            set { endColours = value; }
        }

        float spread = (float)MathHelper.Pi;
        public float Spread
        {
            get { return spread; }
            set { spread = MathHelper.WrapAngle(value); }
        }

        float direction = -MathHelper.PiOver2;
        public float Direction
        {
            get { return direction; }
            set { direction = MathHelper.WrapAngle(value); }
        }

        Vector2 minGravity = Vector2.Zero; // pixels per second
        public Vector2 MinGravity
        {
            get { return minGravity; }
            set { minGravity = value; }
        }

        Vector2 maxGravity = Vector2.Zero; // pixels per second
        public Vector2 MaxGravity
        {
            get { return maxGravity; }
            set { maxGravity = value; }
        }

        public ParticleSettings(GraphicsDevice graphicsDevice, List<Texture2D> textures, int particleLimit)
        {
            g = graphicsDevice;
            this.maxParticles = particleLimit;
            this.textures = textures;

            if (this.textures == null)
            {
                this.textures = new List<Texture2D>(1);
            }

            if (this.textures.Count == 0)
            {
                this.textures.Add(new Texture2D(g, 1, 1));
                this.textures[0].SetData<uint>(new uint[] { 0xFFFFFFFF });
            }

            startColours = new List<Color>(1);
            startColours.Add(Color.White);

            endColours = new List<Color>(1);
            endColours.Add(Color.White);
        }

        public void LoadFromFile(string filename, bool loadTextures)
        {
            BinaryReader r = new BinaryReader(File.Open(filename, FileMode.Open));

            this.createSpeed = r.ReadSingle();
            this.direction = r.ReadSingle();
            this.maxGravity.X = r.ReadSingle();
            this.maxGravity.Y = r.ReadSingle();
            this.maxLifetime = r.ReadSingle();
            this.maxParticles = r.ReadInt32();
            this.maxRotationalVelocity = r.ReadSingle();
            this.maxSpeed = r.ReadSingle();
            this.maxStartRadius = r.ReadSingle();
            this.maxTargetScale.X = r.ReadSingle();
            this.maxTargetScale.Y = r.ReadSingle();
            this.minGravity.X = r.ReadSingle();
            this.minGravity.Y = r.ReadSingle();
            this.minLifetime = r.ReadSingle();
            this.minRotationalVelocity = r.ReadSingle();
            this.minSpeed = r.ReadSingle();
            this.minStartRadius = r.ReadSingle();
            this.minTargetScale.X = r.ReadSingle();
            this.minTargetScale.Y = r.ReadSingle();
            this.spread = r.ReadSingle();

            int startCount = r.ReadInt32();
            startColours = new List<Color>(startCount);
            for (int i = 0; i < startCount; i++)
            {
                startColours.Add(new Color(r.ReadByte(), r.ReadByte(), r.ReadByte(), r.ReadByte()));
            }

            int endCount = r.ReadInt32();
            endColours = new List<Color>(endCount);
            for (int i = 0; i < endCount; i++)
            {
                endColours.Add(new Color(r.ReadByte(), r.ReadByte(), r.ReadByte(), r.ReadByte()));
            }

            if (loadTextures)
            {
                int textureCount = r.ReadInt32();
                textures = new List<Texture2D>(textureCount);
                for (int i = 0; i < textureCount; i++)
                {
                    int width = r.ReadInt32();
                    int height = r.ReadInt32();
                    UInt32[] data = new UInt32[width * height];

                    for (int j = 0; j < data.Length; j++)
                    {
                        data[j] = r.ReadUInt32();
                    }
                    textures.Add(new Texture2D(g, width, height));
                    textures[textures.Count - 1].SetData<UInt32>(data);
                    textures[textures.Count - 1].Name = i.ToString();
                }
            }

            r.Close();
        }

        public void SaveToFile(string filename, bool saveTextures)
        {
            BinaryWriter w = new BinaryWriter(File.Open(filename, FileMode.OpenOrCreate));

            w.Write(this.createSpeed);
            w.Write(this.direction);
            w.Write(this.maxGravity.X);
            w.Write(this.maxGravity.Y);
            w.Write(this.maxLifetime);
            w.Write(this.maxParticles);
            w.Write(this.maxRotationalVelocity);
            w.Write(this.maxSpeed);
            w.Write(this.maxStartRadius);
            w.Write(this.maxTargetScale.X);
            w.Write(this.maxTargetScale.Y);
            w.Write(this.minGravity.X);
            w.Write(this.minGravity.Y);
            w.Write(this.minLifetime);
            w.Write(this.minRotationalVelocity);
            w.Write(this.minSpeed);
            w.Write(this.minStartRadius);
            w.Write(this.minTargetScale.X);
            w.Write(this.minTargetScale.Y);
            w.Write(this.spread);

            w.Write(startColours.Count);
            for (int i = 0; i < startColours.Count; i++)
            {
                w.Write(startColours[i].R);
                w.Write(startColours[i].G);
                w.Write(startColours[i].B);
                w.Write(startColours[i].A);
            }

            w.Write(endColours.Count);
            for (int i = 0; i < endColours.Count; i++)
            {
                w.Write(endColours[i].R);
                w.Write(endColours[i].G);
                w.Write(endColours[i].B);
                w.Write(endColours[i].A);
            }

            if (saveTextures)
            {
                w.Write(textures.Count);
                for (int i = 0; i < textures.Count; i++)
                {
                    UInt32[] data = new UInt32[textures[i].Width * textures[i].Height];
                    textures[i].GetData<UInt32>(data);

                    w.Write(textures[i].Width);
                    w.Write(textures[i].Height);

                    for (int j = 0; j < data.Length; j++)
                    {
                        w.Write(data[j]);
                    }
                }
            }
            else
            {
                w.Write((int)0);
            }

            w.Close();
        }
    }
}
