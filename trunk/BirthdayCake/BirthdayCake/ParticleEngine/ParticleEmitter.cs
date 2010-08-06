using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BirthdayCake
{
    public class ParticleEmitter
    {
        SpriteBatch s;

        int particleCount;
        public float createQueue;

        ParticleSettings settings;

        Particle[] particles;

        Vector2 pos;
        public Vector2 Position
        {
            get { return pos; }
            set { pos = value; }
        }

        Vector2 lastPos;

        static Random r;

        int particlesAlive;
        public int ParticlesAlive
        {
            get { return particlesAlive; }
        }

        public TimeSpan TimeLimit;
        public TimeSpan elapsed;
        public ParticleEmitter(SpriteBatch s, ParticleSettings settings)
        {
            //Loop = true;

            this.s = s;
            this.settings = settings;

            particles = new Particle[settings.MaxParticles];

            if (r == null)
            {
                r = new Random((int)System.DateTime.Now.Ticks);
            }
            elapsed = TimeSpan.FromMilliseconds(251);
            TimeLimit = TimeSpan.FromMilliseconds(250);
        }

        public void Reset()
        {
            elapsed = new TimeSpan();
        }

        public void SetColour(Color c)
        {
            settings.StartColours = new List<Color>(1);
            settings.StartColours.Add(c);
            settings.EndColours = new List<Color>(1);
            settings.EndColours.Add(Color.White);
        }

        public void SetColour2(Color c)
        {
            settings.StartColours = new List<Color>(1);
            settings.StartColours.Add(c);
            settings.EndColours = new List<Color>(1);
            settings.EndColours.Add(c);
        }

        void ReviveParticle()
        {
            if (elapsed < TimeLimit)
            {
                for (int i = 0; i < settings.MaxParticles; i++)
                {
                    //instantiate the particle first, if it hasn't been done already
                    if (particles[i] == null)
                    {
                        particles[i] = new Particle(s);
                        particleCount++;
                    }

                    //Then initialise it
                    if (!particles[i].IsAlive)
                    {
                        InitialiseParticle(i);
                        return;
                    }
                }
            }
        }

        void InitialiseParticle(int i)
        {
            particles[i].IsAlive = true;

            float length = MathHelper.Lerp(settings.MinSpeed, settings.MaxSpeed, (float)r.NextDouble());
            float theta = (float)(((settings.Spread*2) * r.NextDouble()) - settings.Spread) + settings.Direction;
            particles[i].Velocity = new Vector2(length * (float)Math.Cos(theta), length * (float)Math.Sin(theta));

            float startRadius = MathHelper.Lerp(settings.MinStartRadius, settings.MaxStartRadius, (float)r.NextDouble());
            theta = (float)(((settings.Spread*2) * r.NextDouble()) - settings.Spread) + settings.Direction;
            Vector2 startOffset = new Vector2(startRadius * (float)Math.Cos(theta), startRadius * (float)Math.Sin(theta));
            particles[i].Position = Vector2.Lerp(lastPos, pos, (float)r.NextDouble()) + startOffset;

            particles[i].Gravity = Vector2.Lerp(settings.MinGravity, settings.MaxGravity, (float)r.NextDouble());

            particles[i].Rotation = MathHelper.Lerp(settings.MinRotationalVelocity, settings.MaxRotationalVelocity, (float)r.NextDouble());
            particles[i].RotationalVelocity = MathHelper.Lerp(settings.MinRotationalVelocity, settings.MaxRotationalVelocity, (float)r.NextDouble());

            particles[i].Scale = Vector2.One;
            particles[i].TargetScale = Vector2.Lerp(settings.MinTargetScale, settings.MaxTargetScale, (float)r.NextDouble());

            particles[i].TimeToLive = (float)r.NextDouble() * settings.MaxLifetime;

            particles[i].StartColour = settings.StartColours[r.Next(settings.StartColours.Count)];
            particles[i].EndColour = settings.EndColours[r.Next(settings.EndColours.Count)];

            particles[i].Texture = settings.Textures[r.Next(settings.Textures.Count)];
        }

        public void Update(GameTime t)
        {
            float time = (float)(t.ElapsedGameTime.Milliseconds + 1) / 1000;
            elapsed += t.ElapsedGameTime;

            particlesAlive = 0;

            if (particlesAlive < settings.MaxParticles)
            {
                createQueue += settings.CreateSpeed * time;
                for (int i = 0; i < (int)createQueue; i++)
                {
                    ReviveParticle();
                    createQueue -= 1.0f;
                }
            }

            for (int i = 0; i < particleCount; i++) //check all non-null particles
            {
                if (particles[i].IsAlive)
                {
                    particlesAlive++;
                    particles[i].Update(t);
                }
            }

            lastPos.X = pos.X;
            lastPos.Y = pos.Y;
        }

        public void Draw(GameTime t)
        {
            for (int i = 0; i < particleCount; i++)
            {
                particles[i].Draw(t);
            }
        }
    }
}
