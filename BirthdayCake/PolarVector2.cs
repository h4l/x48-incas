using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BirthdayCake
{
	public struct PolarVector2
	{
		public float length;

		public float angle;

		public PolarVector2(float length, float angle)
		{
			this.length = length;
			this.angle = angle;
		}

		public Vector2 Cartesian
		{
			get
			{
				return new Vector2(
					(float)Math.Cos(angle) * length, 
					(float)Math.Sin(angle) * length);
			}
			set
			{
				length = value.Length();
				angle = (float)Math.Atan2(value.Y, value.X);
			}
		}
	}
}
