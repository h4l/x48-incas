using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace BirthdayCake
{
	public class Vehicle
	{
		private float mass;

		private float maxForce;

		private float maxSpeed;

		private Vector2 velocity;

		private Vector2 position;

		public Vehicle()
		{
			reset();
		}

		public float Angle
		{
			get
			{
				return (float)Math.Atan2(velocity.Y, velocity.X);
			}
		}

		public Vector2 Position
		{
			get
			{
				return position;
			}
			set
			{
				position = value;
			}
		}

		public float MaxSpeed
		{
			get
			{
				return maxSpeed;
			}
			set
			{
				maxSpeed = Math.Max(0, value);
			}
		}

		public float MaxForce
		{
			get
			{
				return maxForce;
			}
			set
			{
				maxForce = value;
			}
		}

		public Vector2 Velocity
		{
			get
			{
				return velocity;
			}
			set
			{
				velocity = value;
			}
		}

		/// <summary>
		/// Resets the vehicles values to default.
		/// </summary>
		public void reset()
		{
			mass = 1f;
			maxForce = float.MaxValue;
			maxSpeed = float.MaxValue;
			lastWanderAngle = (float)(Engine.RND.NextDouble() * (Math.PI * 2));
		}

		public Vector2 seek(Vehicle target)
		{
			return seek(target.position);
		}

		public Vector2 seek(Vector2 target)
		{
			return Steering.constrain(Steering.seek(
				position, velocity, target), maxForce);
		}

		public Vector2 avoidCircle(BoundingCircle me, BoundingCircle circle)
		{
			Debug.Assert(position == me.Position);

			if (me.intersects(circle))
			{
				// calculate the direction we need to move to resolve the collision
				Vector2 dir = Vector2.Normalize(me.Position - circle.Position);
				// scale the direction by the amount we intersect the circle
				dir *= (me.Radius + circle.Radius) - Vector2.Distance(position, circle.Position);
				return dir;
			}
			return Vector2.Zero;
		}

		public Vector2 avoidPoint(BoundingCircle me, Vector2 other)
		{
			// FIXME: implement
			return Vector2.Zero;
		}

		private static readonly float CRAPPY_COLLISION_SEARCH_RESOLUTION = (float)(Math.PI / 10);

		/// <summary>
		/// Makes the peon avoid terrain. THIS IS FUCKED, DON'T USE IT
		/// </summary>
		/// <param name="terrain"></param>
		/// <returns></returns>
		public Vector2 avoidTerrain(Terrain terrain, float lookaheadDistance)
		{
			// we shouldn't be colliding now
			Debug.Assert(!terrain.CheckCollision(position));

			if (velocity == Vector2.Zero)
			{
				if (terrain.CheckCollision(position))
					throw new Exception("peon stuck in terrain w/ 0 velocity");
				return Vector2.Zero;
			}

			Vector2 nextPos = position + Vector2.Normalize(velocity) * lookaheadDistance;
			if (terrain.CheckCollision(nextPos))
			{
					// future intersection, try to avoid

				// brute force to find a resolution :/
				PolarVector2 solver = new PolarVector2();
				solver.Cartesian = nextPos - position;

				float startAngle = solver.angle;
				// search left and then right alternatley until we find a resolution 
				// to the collision - we can only check if a single pixel is colliding,
				// we don't know the actual areas of bounds
				int limit = (int)Math.Ceiling(Math.PI / CRAPPY_COLLISION_SEARCH_RESOLUTION);
				for (int count = 0, alternator = 0; count <= limit; alternator = ++alternator % 2)
				{
					solver.angle = startAngle;
					float delta = CRAPPY_COLLISION_SEARCH_RESOLUTION * count;

					if(alternator == 1)
					{
						// search backwards around the circle
						solver.angle = solver.angle + -startAngle;
						++count;
					}

					// see if we don't have a collision
					if (!terrain.CheckCollision(position + solver.Cartesian))
					{
						// no collision
						return seek(solver.Cartesian);
					}
				}
				
				// failed to find a resolution, not good
				Debug.WriteLine(String.Format("Failed to resolve collision at: {0}, lookahead: {1}", 
					position, lookaheadDistance));
				Debug.Assert(false);
			}

			Debug.WriteLine(String.Format("No collision at: {0}", position));
			return Vector2.Zero;
		}

		private float lastWanderAngle;

		public Vector2 wander(float deviation, float distance, float maxAngleDelta)
		{
			lastWanderAngle += (float)(Engine.RND.NextDouble() * (maxAngleDelta * 2)) - maxAngleDelta;

			Vector2 rndPoint = new PolarVector2(deviation, lastWanderAngle).Cartesian;
			Vector2 offset = new PolarVector2(distance, Angle).Cartesian;
			Vector2 target = (rndPoint + offset);
			target.Normalize();
			target *= distance;
			return seek(position + target);
		}

		/// <summary>
		/// Applies the specified acceleration and updates the position etc.
		/// </summary>
		/// <param name="rawAcceleration"></param>
		/// <param name="delta"></param>
		public void apply(Vector2 rawAcceleration, float delta)
		{
			Vector2 accel = rawAcceleration /= mass;
			velocity = Steering.constrain(velocity + accel, maxSpeed);
			position += velocity * delta;
		}
	}
	
	public class Steering
	{
		public static Vector2 seek(Vector2 position, Vector2 velocity, Vector2 target)
		{
			Vector2 desiredVel = target - position;
			return desiredVel - velocity;
		}

		public static Vector2 constrain(Vector2 value, float magnitude)
		{
			if (value.Length() > magnitude)
			{
				value.Normalize();
				return value * magnitude;
			}
			return value;
		}
	}
}
