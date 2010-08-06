using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace BirthdayCake
{
	enum IntroState { Starting, Counting, Transitioning }

	public class Intro : DrawableGameComponent
	{
		private const int TRANSITION_TIME = 300;

		private const int SLIDE_TIMEOUT = 5000;

		private int startTime;

		private IntroState state;

		private Engine engine;

		private List<SpriteSheet> screens;

		private int slideIndex;

		private bool isDone;

		public Intro(Engine engine) : base(engine)
		{
			this.engine = engine;
			screens = new List<SpriteSheet>();
			state = IntroState.Counting;
			isDone = false;
		}

		public bool IsDone
		{
			get
			{
				return isDone;
			}
		}

		protected override void LoadContent()
		{
			Texture2D t2d = engine.Content.Load<Texture2D>("story1");
			screens.Add(new SpriteSheet(t2d, new Vector2(t2d.Width, t2d.Height), 1));
			t2d = engine.Content.Load<Texture2D>("story2");
			screens.Add(new SpriteSheet(t2d, new Vector2(t2d.Width, t2d.Height), 1));
			t2d = engine.Content.Load<Texture2D>("story3");
			screens.Add(new SpriteSheet(t2d, new Vector2(t2d.Width, t2d.Height), 1));
		}

		private bool keyPress = false;

		public override void Update(GameTime gameTime)
		{
			int now = (int)gameTime.TotalGameTime.TotalMilliseconds;

			if(!keyPress)
			{
				if (Input.isGamepadBtnDown(Buttons.A) || Input.isGamepadBtnDown(Buttons.B))
					keyPress = true;
			}
			else if(keyPress && !(Input.isGamepadBtnDown(Buttons.A) || Input.isGamepadBtnDown(Buttons.B)))
			{
				keyPress = false;
				isDone = true;
			}

			if (state == IntroState.Starting)
			{
				Debug.WriteLine("Starting " + slideIndex);
				startTime = now;
				state = IntroState.Counting;
			}

			if (state == IntroState.Transitioning)
			{
				Debug.WriteLine("Tranitioning " + slideIndex);
				if (now - startTime > TRANSITION_TIME)
				{
					showNextFrame(gameTime);
				}
			}
			else if (state == IntroState.Counting)
			{
				Debug.WriteLine("Counting " + slideIndex);
				if (now - startTime > SLIDE_TIMEOUT)
				{
					transitionToNextFrame(gameTime);
				}
			}
		}

		private void transitionToNextFrame(GameTime time)
		{
			startTime = (int)time.TotalGameTime.TotalMilliseconds;
			state = IntroState.Transitioning;
		}

		private void showNextFrame(GameTime time)
		{
			startTime = (int)time.TotalGameTime.TotalMilliseconds;
			++slideIndex;
			state = IntroState.Counting;
			if (slideIndex == screens.Count)
				isDone = true;
		}

		public override void Draw(GameTime gameTime)
		{
			if (state == IntroState.Transitioning)
			{
				float t = (float)(gameTime.TotalGameTime.TotalMilliseconds 
					- startTime) / TRANSITION_TIME;
				screens[slideIndex].Opacity = MathHelper.SmoothStep(1, 0, t);
				screens[slideIndex].Draw();
			}
			else if(state == IntroState.Counting)
			{
				screens[slideIndex].Draw();
			}
		}
	}
}
