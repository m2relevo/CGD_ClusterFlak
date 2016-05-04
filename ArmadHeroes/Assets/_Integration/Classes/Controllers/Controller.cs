/// <summary>
/// Lifted from AstroDillos project
/// Stolen by Daniel Weston ~ thanks Alan!- 09/02/16
/// </summary>
using UnityEngine;
using System.Collections;
using Rewired;
using Rewired.ControllerExtensions;
using DG.Tweening;

#if UNITY_PS4
using UnityEngine.PS4;
#endif

namespace ArmadHeroes{
	public class Controller {

		public enum SplitSide
		{
			none,
			left,
			right
		}
        public bool assigned = false;

		public ControlInput shootButton = new ControlInput();
		public ControlInput accelerateButton = new ControlInput();
        public ControlInput anchorButton = new ControlInput();
		public ControlInput decelerateButton = new ControlInput();
		public ControlInput aimX = new ControlInput();
        public ControlInput moveX = new ControlInput();
        public ControlInput moveY = new ControlInput();
		public ControlInput aimY = new ControlInput();
		public ControlInput hudButton = new ControlInput();
		public ControlInput pauseButton = new ControlInput();
        public ControlInput useButton = new ControlInput();
        public ControlInput boostButton = new ControlInput();
        public ControlInput activateButton = new ControlInput();
        public ControlInput d_left = new ControlInput();
        public ControlInput d_right = new ControlInput();
        public ControlInput d_up = new ControlInput();
        public ControlInput d_down = new ControlInput();
		public ControlInput splitButton = new ControlInput();
        public ControlInput confirm = new ControlInput();
        public ControlInput back = new ControlInput();


        //Rewired player index
        public int playerIndex;
		public int controllerIndex;
		public bool isSplit { 
			get 
			{ return splitSide != SplitSide.none; } 
			private set{}
		}

	    private Sequence vibrationTimer;
        private Sequence flashTimer;
        private Sequence colourTimer;
        
        //controller light colour
        private Color startColour;

		public SplitSide splitSide;

		public Controller(int player = 0, int _controllerIndex = 0){
			playerIndex = player;
			splitSide = SplitSide.none;
			controllerIndex = _controllerIndex;

		}

		public void StartVibration(float strength, float vibrateTime = 0.1f)
        {

            foreach (Joystick joy in ReInput.players.GetPlayer(playerIndex).controllers.Joysticks) {
				if(joy.supportsVibration && joy.vibrationLeftMotor<strength){

					joy.SetVibration(strength,strength);
					if (vibrationTimer!=null && vibrationTimer.IsPlaying()){
						vibrationTimer.Kill();
					}

					vibrationTimer = DOTween.Sequence();
					vibrationTimer.AppendInterval(vibrateTime);
					vibrationTimer.OnComplete(StopVibration);
					break;
				}

			}
		}

		public void StopVibration(){
			foreach (Joystick joy in ReInput.players.GetPlayer(playerIndex).controllers.Joysticks) {
				if(joy.supportsVibration){
					joy.StopVibration();

					DualShock4Extension ps4Pad = joy.GetExtension<DualShock4Extension>();
					if (ps4Pad == null) continue;
					
					ps4Pad.StopVibration();
				}
			}
		}

#region Dualshock 4 lights
        /// <summary>
        /// Sets the light colour of the DS4 (ignores non-DS4 controllers)
        /// </summary>
        /// <param name="colour">colour of the light</param>
        public void SetLightColour(Color colour)
        {
            foreach (Joystick joy in ReInput.players.GetPlayer(playerIndex).controllers.Joysticks)
            {

				DualShock4Extension DS4_Extention = joy.GetExtension<DualShock4Extension>();
                if (DS4_Extention == null)
                    continue;
                DS4_Extention.SetLightColor(colour);

            }
        }
        /// <summary>
        /// Sets light colour to flash for DS4 (ignores non-ds4 controllers)
        /// </summary>
        /// <param name="on">time light is on </param>
        /// <param name="off">time light is off</param>
        public void SetLightFlash(float on, float off,float interval = 0.2f)
        {
            foreach (Joystick joy in ReInput.players.GetPlayer(playerIndex).controllers.Joysticks)
            {
				DualShock4Extension DS4_Extention = joy.GetExtension<DualShock4Extension>();
                if (DS4_Extention == null)
                    continue;
                DS4_Extention.SetLightFlash(on, off);
                if (flashTimer != null && flashTimer.IsPlaying())
                {
                    flashTimer.Kill();
                }
                flashTimer = DOTween.Sequence();
                flashTimer.AppendInterval(interval);
                flashTimer.OnComplete(StopLightFlash);
                break;
            }
        }
        /// <summary>
        /// Stops the flashing colour  (ignores non-ds4 controllers)
        /// </summary>
        void StopLightFlash()
        {
            foreach (Joystick joy in ReInput.players.GetPlayer(playerIndex).controllers.Joysticks)
            {
				DualShock4Extension DS4_Extention = joy.GetExtension<DualShock4Extension>();
                if (DS4_Extention == null)
                    continue;
                DS4_Extention.StopLightFlash();

            }
        }
        /// <summary>
        /// Flashes with an alternative colour
        /// </summary>
        /// <param name="colour">colour to change to</param>
        /// <param name="time">length of colour flash</param>
        public void FlashAltColour(Color colour, float time = 0.1f)
        {
            
            foreach(Joystick joy in ReInput.players.GetPlayer(playerIndex).controllers.Joysticks)
            {
				DualShock4Extension DS4_Extention = joy.GetExtension<DualShock4Extension>();
                if (DS4_Extention == null) continue;

                if (colourTimer != null && colourTimer.IsPlaying())
                {
                    flashTimer.Kill();
                }
                else
                {
                    startColour = DS4_Extention.GetLightColor();
                }
                SetLightColour(colour);
                colourTimer = DOTween.Sequence();
                colourTimer.AppendInterval(time);
                colourTimer.OnComplete(RevertLightColour);
                
                break;
                
          	}
		}
        /// <summary>
        /// Changes the colour back
        /// </summary>
        void RevertLightColour()
        {
            foreach (Joystick joy in ReInput.players.GetPlayer(playerIndex).controllers.Joysticks)
            {
				DualShock4Extension DS4_Extention = joy.GetExtension<DualShock4Extension>();
                if (DS4_Extention == null) continue;
                SetLightColour(startColour);
            }
        }
#endregion

	}

	//Base class for control input
	public class ControlInput{
		protected bool isDown = false;
		protected bool justPressed = false;
		protected bool justReleased = false;

		public ControlInput(){

		}

		public virtual bool IsDown(){
			return false;
		}

		public virtual bool JustPressed(){
			return false;
		}

		public virtual bool JustReleased(){
			return false;
		}

		public virtual float GetValue(){
			return 0;
		}
	}
}





