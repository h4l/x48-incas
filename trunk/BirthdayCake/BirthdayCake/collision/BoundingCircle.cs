using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BirthdayCake
{
	public struct BoundingCircle
	{
		private Vector2 position;

		private float radius;

		public BoundingCircle(Vector2 position, float radius)
		{
			this.position = position;
			this.radius = radius;
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

		public float Radius
		{
			get
			{
				return radius;
			}
			set
			{
				radius = value;
			}
		}

		public bool contains(Vector2 position)
		{
			return Vector2.Distance(position, this.position) <= radius;
		}

		public bool intersects(BoundingCircle other)
		{
			return Vector2.Distance(other.position, position) <= (radius + other.radius);
		}

		private static Random RND = new Random();

		/// <summary>
		/// Gets a random point inside the bounding sphere.
		/// </summary>
		/// <returns></returns>
		public Vector2 getRandomPoint()
		{
			return position + new PolarVector2(
				(float)(RND.NextDouble() * radius), 
				(float)(RND.NextDouble() * Math.PI * 2))
				.Cartesian;
		}
	}
}
