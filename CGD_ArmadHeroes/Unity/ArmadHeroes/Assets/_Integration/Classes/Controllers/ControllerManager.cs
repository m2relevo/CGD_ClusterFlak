/// <summary>
/// Lifted from AstroDillos project
/// Stolen by Daniel Weston ~ thanks Alan!- 09/02/16
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Rewired;
using System.Text;

namespace ArmadHeroes{
	public class ControllerManager : MonoBehaviour {

		public static ControllerManager instance;
		
		//Controllers
		List<Controller> controllers = new List<Controller>();

		private Controller nullController;

		public int controllerCount {
			get{ return controllers.Count; }
			private set{}
		}
        private int assignableControllers = 0;
        public int assignableCount
        {
            get { return assignableControllers; }
            private set { }
        }
        int gamepadCount = 0;
		
		void Awake() {
            

			instance = this;

			nullController = new Controller ();

			CheckForNewControllers ();
		}
		
		void Update(){
			CheckForNewControllers ();
		}
		
		void CheckForNewControllers(){
			if (gamepadCount < ReInput.controllers.joystickCount) {
				
				//Add a new controller
				for (int i = gamepadCount; i < ReInput.controllers.joystickCount; i++) {
					AddController();
					
				}
			}
		}
		
		//Get a controller from it's id
		public Controller GetController(int id){
			if (id >= controllers.Count || id<0) {
				return nullController;
			}
			
			return controllers [id];
		}
		
		public void AddController(){
            int _emptySlot = FindEmptySlot();
            if (_emptySlot == controllers.Count)
            {
                controllers.Add(new Controller_PS4_Full(gamepadCount, controllers.Count));
            }
            else
            {
                controllers[_emptySlot] = new Controller_PS4_Full(gamepadCount, _emptySlot);
            }
            assignableControllers++;
            gamepadCount++;
        }

		public int SplitController(Controller controller){
			int index = controller.controllerIndex;
			controllers [index] = new Controller_PS4_Left (controllers [index].playerIndex, controllers [index].controllerIndex);
            controllers[index].assigned = true;
            //Find empty slot before trying to find new one
            int _emptySlot = FindEmptySlot();
            if (_emptySlot == controllers.Count)
            {
                controllers.Add(new Controller_PS4_Right(controllers[index].playerIndex, controllers.Count));
                controllers[controllers.Count - 1].assigned = true;
                return controllers.Count - 1;
            }
            else
            {
                controllers[_emptySlot] = new Controller_PS4_Right(controllers[index].playerIndex, _emptySlot);
                controllers[_emptySlot].assigned = true;
                return _emptySlot;
            }
		}

        public void FuseController(Controller _controller)
        {
            int _index = _controller.playerIndex;
            bool _left = true;
            for (int i = 0; i < controllers.Count; ++i)
            {
                if (controllers[i] == null) continue;

                //If the same controller
                if (controllers[i].playerIndex == _index)
                {
                    //Replace the left
                    if (_left)
                    {
                        controllers[i] = new Controller_PS4_Full(_index, i);
                        controllers[i].assigned = true;
                        _left = false;
                    }
                    //Then remove the right
                    else
                    {
                        //Set empty position - means controller index do not move around
                        controllers[i] = null;
                        break;
                    }  
                }
            }
        }

        public int AssignNext()
        {
            if(assignableControllers <= 0)
            {
                return -1; //This should never be called, ever
            }
            --assignableControllers;
            int _unassigned = FindUnassigned();
            controllers[_unassigned].assigned = true;
            return _unassigned;
        }

        public void UnassignAll()
        {
            for (int i = 0; i < controllers.Count; ++i)
            {
                if (controllers[i] == null) continue;
                if (controllers[i].assigned == true)
                {
                    controllers[i].assigned = false;
                    assignableControllers++;
                }
            }
        }

        public int FindEmptySlot()
        {
            for (int i = 0; i < controllers.Count; ++i)
            {
                if(controllers[i] == null)
                {
                    return i;
                }
            }
            return controllers.Count;
        }

        public int FindUnassigned()
        {
            for (int i = 0; i < controllers.Count; ++i)
            {
                if (controllers[i] != null)
                {
                    if (!controllers[i].assigned)
                    {
                        return i;
                    }
                }
            }
            return controllers.Count;
        }


        public void ResetContollers()
        {
            controllers.Clear();
            assignableControllers = 0;
            gamepadCount = 0;
            CheckForNewControllers();
        }

        void OnApplicationQuit(){
			//Stop all controller vibration
			for(int i = 0; i<controllers.Count; i++){
                if (controllers[i] == null) continue;
                controllers[i].StopVibration();
			}
		}
		
	}

}



