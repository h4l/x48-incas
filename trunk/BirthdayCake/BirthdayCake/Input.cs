using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BirthdayCake
{
    public class Input
    {
        Vector2 stick;

        GamePadState oldStateOne;
        GamePadState oldStateTwo;
        GamePadState oldStateThree;
        GamePadState oldStateFour;

        public delegate void ControllerButtonEvent(PlayerIndex player, Buttons button, GameTime t);
        public delegate void ControllerStickEvent(PlayerIndex player, Vector2 v, GameTime t);
        public delegate void ControllerTriggerEvent(PlayerIndex player, float f, GameTime t);

        public event ControllerButtonEvent ButtonDown;
        public event ControllerButtonEvent ButtonUp;
        public event ControllerStickEvent LeftStickMove;
        public event ControllerStickEvent RightStickMove;
        public event ControllerTriggerEvent LeftTriggerChange;
        public event ControllerTriggerEvent RightTriggerChange;

        static Buttons[] buttons = new Buttons[] { Buttons.A, Buttons.B, Buttons.Back, Buttons.BigButton, Buttons.DPadDown,
            Buttons.DPadLeft, Buttons.DPadRight, Buttons.DPadUp, Buttons.LeftShoulder, Buttons.LeftStick, Buttons.LeftThumbstickDown,
            Buttons.LeftThumbstickLeft, Buttons.LeftThumbstickRight, Buttons.LeftThumbstickUp, Buttons.LeftTrigger, Buttons.RightShoulder,
            Buttons.RightStick, Buttons.RightThumbstickDown, Buttons.RightThumbstickLeft, Buttons.RightThumbstickRight, Buttons.RightThumbstickUp,
            Buttons.RightTrigger, Buttons.Start, Buttons.X, Buttons.Y };

        public Input()
        {
            oldStateOne = GamePad.GetState(PlayerIndex.One);
            oldStateTwo = GamePad.GetState(PlayerIndex.Two);
            oldStateThree = GamePad.GetState(PlayerIndex.Three);
            oldStateFour = GamePad.GetState(PlayerIndex.Four);
        }

        public void Update(GameTime t)
        {
            GamePadState stateOne = GamePad.GetState(PlayerIndex.One);
            GamePadState stateTwo = GamePad.GetState(PlayerIndex.Two);
            GamePadState stateThree = GamePad.GetState(PlayerIndex.Three);
            GamePadState stateFour = GamePad.GetState(PlayerIndex.Four);

            #region Sticks
            if (stateOne.ThumbSticks.Left.Length() != 0)
            {
                stick = stateOne.ThumbSticks.Left;
                stick.Y *= -1;
                if (LeftStickMove != null) { LeftStickMove(PlayerIndex.One, stick, t); }
            }
            if (stateOne.ThumbSticks.Right.Length() != 0)
            {
                stick = stateOne.ThumbSticks.Right;
                stick.Y *= -1;
                if (RightStickMove != null) { RightStickMove(PlayerIndex.One, stick, t); }
            }
            if (stateTwo.ThumbSticks.Left.Length() != 0)
            {
                stick = stateTwo.ThumbSticks.Left;
                stick.Y *= -1;
                if (LeftStickMove != null) { LeftStickMove(PlayerIndex.Two, stick, t); }
            }
            if (stateTwo.ThumbSticks.Right.Length() != 0)
            {
                stick = stateTwo.ThumbSticks.Right;
                stick.Y *= -1;
                if (RightStickMove != null) { RightStickMove(PlayerIndex.Two, stick, t); }
            }
            if (stateThree.ThumbSticks.Left.Length() != 0)
            {
                stick = stateThree.ThumbSticks.Left;
                stick.Y *= -1;
                if (LeftStickMove != null) { LeftStickMove(PlayerIndex.Three, stick, t); }
            }
            if (stateThree.ThumbSticks.Right.Length() != 0)
            {
                stick = stateThree.ThumbSticks.Right;
                stick.Y *= -1;
                if (RightStickMove != null) { RightStickMove(PlayerIndex.Three, stick, t); }
            }
            if (stateFour.ThumbSticks.Left.Length() != 0)
            {
                stick = stateFour.ThumbSticks.Left;
                stick.Y *= -1;
                if (LeftStickMove != null) { LeftStickMove(PlayerIndex.Four, stick, t); }
            }
            if (stateFour.ThumbSticks.Right.Length() != 0)
            {
                stick = stateFour.ThumbSticks.Right;
                stick.Y *= -1;
                if (RightStickMove != null) { RightStickMove(PlayerIndex.Four, stick, t); }
            }
            #endregion

            #region Triggers
            if (stateOne.Triggers.Left != oldStateOne.Triggers.Left)
            {
                if (LeftTriggerChange != null) { LeftTriggerChange(PlayerIndex.One, stateOne.Triggers.Left, t); }
            }
            if (stateOne.Triggers.Right != oldStateOne.Triggers.Right)
            {
                if (RightTriggerChange != null) { RightTriggerChange(PlayerIndex.One, stateOne.Triggers.Right, t); }
            }
            if (stateTwo.Triggers.Left != oldStateTwo.Triggers.Left)
            {
                if (LeftTriggerChange != null) { LeftTriggerChange(PlayerIndex.Two, stateTwo.Triggers.Left, t); }
            }
            if (stateTwo.Triggers.Right != oldStateTwo.Triggers.Right)
            {
                if (RightTriggerChange != null) { RightTriggerChange(PlayerIndex.Two, stateTwo.Triggers.Right, t); }
            }
            if (stateThree.Triggers.Left != oldStateThree.Triggers.Left)
            {
                if (LeftTriggerChange != null) { LeftTriggerChange(PlayerIndex.Three, stateThree.Triggers.Left, t); }
            }
            if (stateThree.Triggers.Right != oldStateThree.Triggers.Right)
            {
                if (RightTriggerChange != null) { RightTriggerChange(PlayerIndex.Three, stateThree.Triggers.Right, t); }
            }
            if (stateFour.Triggers.Left != oldStateFour.Triggers.Left)
            {
                if (LeftTriggerChange != null) { LeftTriggerChange(PlayerIndex.Four, stateFour.Triggers.Left, t); }
            }
            if (stateFour.Triggers.Right != oldStateFour.Triggers.Right)
            {
                if (RightTriggerChange != null) { RightTriggerChange(PlayerIndex.Four, stateFour.Triggers.Right, t); }
            }
            #endregion

            #region Buttons
            for (ushort i = 0; i < buttons.Length; i++)
            {
                if (stateOne.IsButtonDown(buttons[i]))
                {
                    if (ButtonDown != null) { ButtonDown(PlayerIndex.One, buttons[i], t); }
                }
                else if (oldStateOne.IsButtonDown(buttons[i]))
                {
                    if (ButtonUp != null) { ButtonUp(PlayerIndex.One, buttons[i], t); }
                }

                if (stateTwo.IsButtonDown(buttons[i]))
                {
                    if (ButtonDown != null) { ButtonDown(PlayerIndex.Two, buttons[i], t); }
                }
                else if (oldStateTwo.IsButtonDown(buttons[i]))
                {
                    if (ButtonUp != null) { ButtonUp(PlayerIndex.Two, buttons[i], t); }
                }

                if (stateThree.IsButtonDown(buttons[i]))
                {
                    if (ButtonDown != null) { ButtonDown(PlayerIndex.Three, buttons[i], t); }
                }
                else if (oldStateThree.IsButtonDown(buttons[i]))
                {
                    if (ButtonUp != null) { ButtonUp(PlayerIndex.Three, buttons[i], t); }
                }

                if (stateFour.IsButtonDown(buttons[i]))
                {
                    if (ButtonDown != null) { ButtonDown(PlayerIndex.Four, buttons[i], t); }
                }
                else if (oldStateFour.IsButtonDown(buttons[i]))
                {
                    if (ButtonUp != null) { ButtonUp(PlayerIndex.Four, buttons[i], t); }
                }
            }
            #endregion

            oldStateOne = stateOne;
            oldStateTwo = stateTwo;
            oldStateThree = stateThree;
            oldStateFour = stateFour;
        }

		private static PlayerIndex[] PLAYERS = { PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four };

		public static bool isGamepadBtnDown(Buttons button)
		{
			foreach (PlayerIndex index in PLAYERS)
			{
				GamePadState state = GamePad.GetState(index);
				if (state.IsButtonDown(button))
					return true;
			}
			return false;
		}
    }
}
