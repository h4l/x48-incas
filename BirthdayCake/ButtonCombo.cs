using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BirthdayCake
{
    public class ButtonCombo
    {
        Vector2 barOffset = new Vector2(50, -50);

        ParticleEmitter emitter;
        ParticleSettings particleSettings;

        const float BUTTON_SPACING = 20.0f;

        Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                emitter.Position = this.position;
            }
        }

        PlayerIndex player;

        static Texture2D[] buttonTextures;
        static Texture2D background;

        static Buttons[] buttons;

        static Random rand;

        Queue<byte> buttonQueue;

        TimeSpan vibrateTimer;
        bool vibrate;

        Vector2 buttonPosition = new Vector2(11, 5);

        public ButtonCombo(PlayerIndex player)
        {
            this.player = player;

            particleSettings = new ParticleSettings(Engine.graphics.GraphicsDevice, null, 500);
            particleSettings.LoadFromFile("Content/buttonpress.psf", true);
            emitter = new ParticleEmitter(Engine.spriteBatch, particleSettings);
            emitter.Position += this.position + buttonPosition + new Vector2(10, 10);

            background = Engine.ContentManager.Load<Texture2D>("combo_background");

            if (buttonTextures == null)
            {
                buttonTextures = new Texture2D[4];
                buttonTextures[0] = Engine.ContentManager.Load<Texture2D>("button_a");
                buttonTextures[1] = Engine.ContentManager.Load<Texture2D>("button_b");
                buttonTextures[2] = Engine.ContentManager.Load<Texture2D>("button_x");
                buttonTextures[3] = Engine.ContentManager.Load<Texture2D>("button_y");
            }

            if (buttons == null)
            {
                buttons = new Buttons[4];
                buttons[0] = Buttons.A;
                buttons[1] = Buttons.B;
                buttons[2] = Buttons.X;
                buttons[3] = Buttons.Y;
            }

            if (rand == null)
            {
                rand = new Random(DateTime.Now.Millisecond);
            }

            buttonQueue = new Queue<byte>(5);

            for (int i = 0; i < 5; i++)
            {
                buttonQueue.Enqueue((byte)rand.Next(4));
            }

            Engine.InputManager.ButtonUp += new Input.ControllerButtonEvent(InputManager_ButtonUp);
        }

        void InputManager_ButtonUp(PlayerIndex player, Buttons button, GameTime t)
        {
            if (this.player == player)
            {
                if (button == buttons[buttonQueue.Peek()])
                {
                    buttonQueue.Dequeue();
                    buttonQueue.Enqueue((byte)rand.Next(4));
                    switch (button)
                    {
                        case Buttons.A:
                            emitter.SetColour(Color.LimeGreen);
                            playerChecker(player);
                            break;

                        case Buttons.B:
                            emitter.SetColour(Color.Red);
                            playerChecker(player);
                            break;

                        case Buttons.X:
                            emitter.SetColour(Color.Blue);
                            playerChecker(player);
                            break;

                        case Buttons.Y:
                            emitter.SetColour(Color.Yellow);
                            playerChecker(player);
                            break;
                    }
                    emitter.Reset();
                }
                else
                {
                    if (button == Buttons.A || button == Buttons.B || button == Buttons.X || button == Buttons.Y)
                    {
                        vibrate = true;
                        for (int i = 0; i < Engine.players[player].attachedPeons.Count; i++)
                        {
                            Engine.players[player].attachedPeons[i].Favor -= 0.25f;
                            if (Engine.players[player].attachedPeons[i].Favor == 0)
                            {
                                Engine.players[player].attachedPeons[i].startWandering();
                            }
                        }
                    }
                }
            }
        }
        public void playerChecker(PlayerIndex player)
        {
			Engine.players[player].onComboButtonPressed();
        }

        public void Update(GameTime t)
        {
            emitter.Update(t);

            if (vibrate)
            {
                GamePad.SetVibration(player, 1.0f, 1.0f);
                vibrateTimer += t.ElapsedGameTime;
            }

            if (vibrateTimer > TimeSpan.FromMilliseconds(100))
            {
                GamePad.SetVibration(player, 0.0f, 0.0f);
                vibrate = false;
                vibrateTimer = new TimeSpan();
            }
            if (!vibrate)
            {
                GamePad.SetVibration(player, 0.0f, 0.0f);
            }
        }

        public void Draw(GameTime t)
        {
            Engine.spriteBatch.Draw(background, position + new Vector2(7, 2) + barOffset, Engine.PlayerColours[player]);

            Engine.spriteBatch.End();
            Engine.spriteBatch.Begin(SpriteBlendMode.Additive);
            emitter.Draw(t);
            Engine.spriteBatch.End();
            Engine.spriteBatch.Begin();

            for (int i = 0; i < 5; i++)
            {
                byte buttonID = buttonQueue.ElementAt(i);
                Engine.spriteBatch.Draw(buttonTextures[buttonID], position + buttonPosition + new Vector2(0, BUTTON_SPACING * i) + barOffset, new Color(Color.White, MathHelper.Lerp(1.0f, 0.0f, (float)i / 5)));
            }
        }
    }
}
