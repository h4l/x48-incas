using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BirthdayCake
{
	/// <summary>
	/// 
	/// </summary>
	public class PeonManager
	{
		// O(n) who cares :)
		private List<Peon> peons;

		public PeonManager()
		{
			peons = new List<Peon>();
		}

		public ReadOnlyCollection<Peon> Peons
		{
			get
			{
				return peons.AsReadOnly();
			}
		}

		public bool addPeon(Peon peon)
		{
			if (!peons.Contains(peon))
			{
				peons.Add(peon);
				return true;
			}
			return false;
		}

		public bool contains(Peon peon)
		{
			return peons.Contains(peon);
		}

		public bool remove(Peon peon)
		{
			return peons.Remove(peon);
		}

		public void Update(GameTime gameTime)
		{
			foreach (Peon p in peons)
				p.Update(gameTime);
		}
        public void Draw(GameTime t)
        {
            foreach (Peon p in peons)
                p.Draw(t);
        }
	}
}
