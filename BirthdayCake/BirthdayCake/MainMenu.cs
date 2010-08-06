using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace BirthdayCake
{
	class MenuItem
	{
		public SpriteSheet selected, idle;

		public MenuItem(SpriteSheet selected, SpriteSheet idle)
		{
			this.selected = selected;
			this.idle = idle;
		}
	}

	public class MainMenu : DrawableGameComponent
	{
		private Engine engine;

		private List<MenuItem> menuItems;

		private SpriteSheet txBackground;

		private int selectedIndex;

		private bool isShowing;

		private int lastPressMs;

        bool press;

		private static int PRESS_INTERVAL_MS = 200;

		public MainMenu(Engine engine)
			: base(engine)
		{
			this.engine = engine;
			menuItems = new List<MenuItem>();
			isShowing = true;
		}

		public bool IsShowing
		{
			get
			{
				return isShowing;
			}
			set
			{
				isShowing = value;
				if (isShowing)
					onStartShowing();
			}
		}

		private void onStartShowing()
		{
			
		}

		protected override void LoadContent()
		{
			Texture2D tx = engine.Content.Load<Texture2D>("menuBkg");
			txBackground = new SpriteSheet(tx, new Vector2(tx.Width, tx.Height), 1);

			Vector2 startPos = new Vector2(312, 473);
			tx = engine.Content.Load<Texture2D>("btn_start");
			SpriteSheet txBtnStart = new SpriteSheet(tx, new Vector2(tx.Width, tx.Height), 1);
            txBtnStart.Position = startPos;
			tx = engine.Content.Load<Texture2D>("btn_startDown");
			SpriteSheet txBtnStartDown = new SpriteSheet(tx, new Vector2(tx.Width, tx.Height), 1);
            txBtnStartDown.Position = startPos;

            Vector2 exitPos = new Vector2(312, 833);
			tx = engine.Content.Load<Texture2D>("btn_exit");
			SpriteSheet txBtnExit = new SpriteSheet(tx, new Vector2(tx.Width, tx.Height), 1);
            txBtnExit.Position = exitPos;
			tx = engine.Content.Load<Texture2D>("btn_exitDown");
			SpriteSheet txBtnExitDown = new SpriteSheet(tx, new Vector2(tx.Width, tx.Height), 1);
            txBtnExitDown.Position = exitPos;

            Vector2 tutorialPos = new Vector2(312, 653);
            tx = engine.Content.Load<Texture2D>("btn_tutorial");
            SpriteSheet txBtnTutorial = new SpriteSheet(tx, new Vector2(tx.Width, tx.Height), 1);
            txBtnTutorial.Position = tutorialPos;
            tx = engine.Content.Load<Texture2D>("btn_tutorialDown");
            SpriteSheet txBtnTutorialDown = new SpriteSheet(tx, new Vector2(tx.Width, tx.Height), 1);
            txBtnTutorialDown.Position = tutorialPos;

			menuItems.Add(new MenuItem(txBtnStart, txBtnStartDown));
            menuItems.Add(new MenuItem(txBtnTutorial, txBtnTutorialDown));
			menuItems.Add(new MenuItem(txBtnExit, txBtnExitDown));
		}

		public override void Update(GameTime gameTime)
		{
			int now = (int)gameTime.TotalGameTime.TotalMilliseconds;
			Debug.WriteLine(now);
			if (now - lastPressMs > PRESS_INTERVAL_MS)
			{
				if (Input.isGamepadBtnDown(Buttons.DPadUp))
				{
                    
					selectedIndex = --selectedIndex % menuItems.Count;
					lastPressMs = now;
                    if (selectedIndex == -1)
                    {
                        selectedIndex = 2;
                    }
				}
				else if (Input.isGamepadBtnDown(Buttons.DPadDown))
				{
					selectedIndex = ++selectedIndex % menuItems.Count;
					lastPressMs = now;
				}
                if(GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) && !press)
                {
                    if(selectedIndex == 0)
                    {
                        //start game
                        isShowing = false;
                    }
                    if (selectedIndex == 1)
                    {
                        //tutorial
                        Engine.state = Engine.State.Tutorial1;
                    }
                    if(selectedIndex == 2)
                    {
                        //exit game
                        engine.Exit();
                    }
                }
			}

            press = GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A);
		}

		public override void Draw(GameTime gameTime)
		{
			if (!IsShowing)
				return;

			txBackground.Draw();

			for(int i = 0, count = menuItems.Count; i < count; ++i)
			{
				SpriteSheet ss;
				if (i == selectedIndex)
				{
					ss = menuItems[i].selected;
					ss.Opacity = 1;
				}
				else
				{
					ss = menuItems[i].idle;
					ss.Opacity = 0.5f;
				}
				ss.Draw();
			}
		}
	}
}
