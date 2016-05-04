/// <summary>
/// Lifted from AstroDillos project
/// Stolen by Daniel Weston ~ thanks Alan!- 09/02/16
/// </summary>
using UnityEngine;
using System.Collections;
using Rewired;

namespace ArmadHeroes
{
	public class Controller_PS4_Full : Controller {

		public Controller_PS4_Full(int player, int _controllerIndex) : base(player,_controllerIndex){
           
			accelerateButton = new ControlInputHybrid(player, "accelerate");
            anchorButton = new ControlInputButton(player, "anchor");
			decelerateButton = new ControlInputHybrid(player, "deccelerate");
			shootButton = new ControlInputButton (player, "shoot");
			aimY = new ControlInputAxis (player, "aim_up");
			aimX = new ControlInputAxis (player, "aim_right");
			moveX = new ControlInputAxis(player, "move_right");
			moveY = new ControlInputAxis(player, "move_up");
			hudButton = new ControlInputButton(player, "hud_left");
			splitButton = new ControlInputButton(player, "split_controller");
			pauseButton = new ControlInputButton(player, "pause");
			useButton = new ControlInputButton(player, "use");
			activateButton = new ControlInputButton(player, "activate");
			boostButton = new ControlInputButton(player, "boost");

            d_left = new ControlInputHybrid(player, "dpad_left");
			d_right = new ControlInputHybrid(player, "dpad_right");
            d_up = new ControlInputHybrid(player, "dpad_up");
            d_down = new ControlInputHybrid(player, "dpad_down");
            confirm = new ControlInputButton(player, "confirm");
            back = new ControlInputButton(player, "back");
        }
	}

	//Split sides of the controller
	public class Controller_PS4_Left : Controller {
		
		public Controller_PS4_Left(int player, int _controllerIndex) : base(player,_controllerIndex){
			splitSide = SplitSide.left;
			accelerateButton = new ControlInputHybrid(player, "split_left_trigger");
			decelerateButton = new ControlInputHybrid(player, "split_left_bumper");
			shootButton = new ControlInputButton (player, "split_left_shoot");
			aimY = new ControlInputAxis (player, "split_left_analog_up");
			aimX = new ControlInputAxis (player, "split_left_analog_right");
			moveX = new ControlInputAxis(player, "split_left_analog_right");
			moveY = new ControlInputAxis(player, "split_left_analog_up");
            hudButton = new ControlInputButton(player, "hud_left");
            splitButton = new ControlInputButton(player, "split_controller");
            pauseButton = new ControlInputButton(player, "pause");
			useButton = new ControlInputButton(player, "split_left_dpad_right");
			activateButton = new ControlInputButton(player, "split_left_dpad_up");
			boostButton = new ControlInputButton(player, "split_left_dpad_down");
			anchorButton = new ControlInputHybrid (player, "split_left_trigger");
			d_left = new ControlInputHybrid(player, "split_left_dpad_left");
			d_right = new ControlInputHybrid(player, "split_left_dpad_right");

            d_up = new ControlInputHybrid(player, "dpad_up");
            d_down = new ControlInputHybrid(player, "dpad_down");
            confirm = new ControlInputButton(player, "confirm");
            back = new ControlInputButton(player, "back");
        }
	}

	public class Controller_PS4_Right : Controller {
		
		public Controller_PS4_Right(int player, int _controllerIndex) : base(player,_controllerIndex){
			splitSide = SplitSide.right;
			accelerateButton = new ControlInputHybrid(player, "split_right_trigger");
			decelerateButton = new ControlInputHybrid(player, "split_right_bumper");
			shootButton = new ControlInputButton (player, "shoot");
			aimY = new ControlInputAxis (player, "split_right_analog_up");
			aimX = new ControlInputAxis (player, "split_right_analog_right");
			moveX = new ControlInputAxis(player, "split_right_analog_right");
			moveY = new ControlInputAxis(player, "split_right_analog_up");
            hudButton = new ControlInputButton(player, "hud_right");
            splitButton = new ControlInputButton(player, "split_controller");
			anchorButton = new ControlInputHybrid (player, "split_right_trigger");
            //pauseButton = new ControlInputButton(player, "pause");
            useButton = new ControlInputButton(player, "split_right_dpad_right");
			activateButton = new ControlInputButton(player, "split_right_dpad_up");
			boostButton = new ControlInputButton(player, "split_right_dpad_down");
			d_left = new ControlInputHybrid(player, "split_right_dpad_left");
			d_right = new ControlInputHybrid(player, "split_right_dpad_right");

            d_up = new ControlInputHybrid(player, "split_right_dpad_up");
            d_down = new ControlInputHybrid(player, "split_right_dpad_down");
            confirm = new ControlInputButton(player, "confirm");
            back = new ControlInputButton(player, "back");
        }
    }


	//Gamepad controller input
	public class ControlInputButton : ControlInput{

		int playerIndex;
		string buttonName;

		public ControlInputButton(int player, string button):base(){
			playerIndex = player;
			buttonName = button;
		}

		public override bool IsDown(){

			return ReInput.players.GetPlayer(playerIndex).GetButton (buttonName);
		}

		public override bool JustPressed(){
			return ReInput.players.GetPlayer(playerIndex).GetButtonDown (buttonName);
		}

		public override bool JustReleased(){
			return ReInput.players.GetPlayer(playerIndex).GetButtonUp (buttonName);
		}
	}

	public class ControlInputAxis : ControlInput{
		int playerIndex;
		string axisName;
		
		public ControlInputAxis(int player, string axis):base(){
			playerIndex = player;
			axisName = axis;
		}
		
		public override float GetValue(){
			return ReInput.players.GetPlayer(playerIndex).GetAxis (axisName);
		}

		public override bool IsDown(){
            return (ReInput.players.GetPlayer(playerIndex).GetButton(axisName) ||
                    ReInput.players.GetPlayer(playerIndex).GetNegativeButton(axisName));
        }

		public override bool JustPressed(){
			float axisValue = ReInput.players.GetPlayer (playerIndex).GetAxis (axisName);
			if (axisValue!=0) {
				return (ReInput.players.GetPlayer(playerIndex).GetButtonDown (axisName) || 
					ReInput.players.GetPlayer(playerIndex).GetNegativeButtonDown (axisName));
			}
			return false;
		}

	}

    public class ControlInputHybrid : ControlInput
    {
        int playerIndex;
        string hybridName;

        public ControlInputHybrid(int player, string hybrid) : base()
        {
            playerIndex = player;
            hybridName = hybrid;
        }

        public override bool IsDown()
        {

            return ReInput.players.GetPlayer(playerIndex).GetButton(hybridName);
        }

        public override bool JustPressed()
        {
            return ReInput.players.GetPlayer(playerIndex).GetButtonDown(hybridName);
        }

        public override bool JustReleased()
        {
            return ReInput.players.GetPlayer(playerIndex).GetButtonUp(hybridName);
        }

        public override float GetValue()
        {
            return ReInput.players.GetPlayer(playerIndex).GetAxis(hybridName);
        }
    }
}	