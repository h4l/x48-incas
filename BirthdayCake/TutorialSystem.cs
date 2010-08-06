using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BirthdayCake
{
    public class TutorialSystem
    {
        Texture2D demigod;

        bool isShowing;
        public bool IsShowing
        {
            get { return isShowing; }
            set { isShowing = value; }
        }

        bool press;

        public bool demigodDone;

        public TutorialSystem()
        {
            isShowing = false;
            demigod = Engine.ContentManager.Load<Texture2D>("tutorial_demigod");
        }

        public void Update(GameTime t)
        {
            if (isShowing)
            {
				if (Input.isGamepadBtnDown(Buttons.A) || Input.isGamepadBtnDown(Buttons.B) && !press)
				{
					isShowing = false;
					demigodDone = true;
				}
				press = Input.isGamepadBtnDown(Buttons.A) || Input.isGamepadBtnDown(Buttons.B);
            }
        }

        public void Draw()
        {
            if (isShowing)
            {
                Engine.spriteBatch.Draw(demigod, Vector2.One, Color.White);
            }
        }
    }
}
