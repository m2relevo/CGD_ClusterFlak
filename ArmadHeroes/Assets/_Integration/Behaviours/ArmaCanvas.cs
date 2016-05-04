/// <summary>
/// ArmaCanvas.cs
/// Created & Implemented by Daniel Weston 23/04/2016
/// Edited to include AutoFill by Shaun Landy 23/04/2016
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ArmadHeroes
{
    #region State
    public enum CanvasState
    {
        Alive,
        Sleep, 
        Dead
    };
    #endregion

    public class ArmaCanvas : MonoBehaviour
    {
        #region Public Members
        public GameObject m_SliderOneSlot, m_SliderTwoSlot, m_CountSlot;
        public Image m_SliderOneFill, m_SliderTwoFill;
        public Text m_CountText, m_PickedUp;
        #endregion

        #region Private Members
        SliderComponent m_SliderOneComponent = new SliderComponent();
        SliderComponent m_SliderTwoComponent = new SliderComponent();
        CounterComponent m_CounterComponent = new CounterComponent();
        #endregion

        #region Unity Callbacks
        void Awake()
        {
            if (m_SliderOneSlot)
            {
                m_SliderOneComponent.InitComponent(m_SliderOneSlot, m_SliderOneFill, Color.cyan, Color.red);
            }
            if (m_SliderTwoSlot)
            {
                m_SliderTwoComponent.InitComponent(m_SliderTwoSlot, m_SliderTwoFill, Color.green, Color.red);
            }
            if (m_CountSlot)
            {
                m_CounterComponent.InitComponent(m_CountSlot, m_CountText);
            }
        }

        void Update()
        {
            if (m_SliderOneSlot)
            {
                m_SliderOneComponent.Update();
            }
            if (m_SliderTwoSlot)
            {
                m_SliderTwoComponent.Update();
            }
            if (m_CountSlot)
            {
                m_CounterComponent.Update();
            }
        }
        #endregion

        #region ArmaCanvas Behaviours
        public void UpdateSliderOne(float value)
        {
            m_SliderOneComponent.UpdateFill(value / 100.0f);
        }
        public void UpdateSliderTwo(float value)
        {
            m_SliderTwoComponent.UpdateFill(value / 100.0f);
        }
        public void UpdateCount(int number)
        {
			m_CounterComponent.UpdateCount(number.ToString());
        }
		public void UpdateCount(string number)
		{
			m_CounterComponent.UpdateCount(number);
		}
        //Set sliders to automatically replenish, can assign function to automatically be called upon full refill
        public void AutoSliderOne(float _speed, SliderComponent.AutoFillFunction _function = null)
        {
            m_SliderOneComponent.AutoFill(_speed, _function);
        }
        public void AutoSliderTwo(float _speed, SliderComponent.AutoFillFunction _function = null)
        {
            m_SliderTwoComponent.AutoFill(_speed, _function);
        }
        /// <summary>
        /// Activate all UI 
        /// Components
        /// </summary>
        public void Activate()
        {
            if (m_SliderOneFill)
            {
                m_SliderOneComponent.state = CanvasState.Alive;
            }
            if (m_SliderTwoFill)
            {
                m_SliderTwoComponent.state = CanvasState.Alive;
            }
            if (m_CountSlot)
            {
                m_CounterComponent.state = CanvasState.Alive;
            }
        }

        /// <summary>
        /// Perma turns off 
        /// all UI components
        /// </summary>
        public void Deactivate()
        {
            if (m_SliderOneFill)
            {
                m_SliderOneComponent.state = CanvasState.Dead;
            }
            if (m_SliderTwoFill)
            {
                m_SliderTwoComponent.state = CanvasState.Dead;
            }
            if (m_CountSlot)
            {
                m_CounterComponent.state = CanvasState.Dead;
            }
        }

        public void ActivateWeaponPickup(string weaponName)
        {
            m_PickedUp.enabled = true;
            m_PickedUp.text = weaponName;
            Invoke("DeactivateWeaponPickup", 5);
        }

        public void DeactivateWeaponPickup()
        {
            m_PickedUp.enabled = false;
        }
        #endregion
    }

    /// <summary>
    /// Base Class for UI 
    /// Components
    /// </summary>
    public class Component
    {
        protected GameObject UISlot;
        private CanvasState m_state;
        public CanvasState state { get { return m_state; } set { SetState(value); } }
        private void SetState(CanvasState _state)
        {
            switch (_state)
            {
                case CanvasState.Alive:
                    CurrentAliveTime = AliveTime;
                    UISlot.gameObject.SetActive(true);
                    break;
                case CanvasState.Sleep:
                    UISlot.gameObject.SetActive(false);
                    break;
                case CanvasState.Dead:
                    UISlot.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
            m_state = _state;
        }
        protected float AliveTime = 2.0f,
           CurrentAliveTime;

        protected void Init(GameObject _slot)
        {
            UISlot = _slot;
            CurrentAliveTime = AliveTime;
        }

        public virtual void Update()
        {
            if (!(CurrentAliveTime <= 0))
            {
                CurrentAliveTime -= Time.deltaTime;
            }
            else
            {
                state = CanvasState.Sleep;
            }
        }
    }

    /// <summary>
    /// Slider Component
    /// for UI elements
    /// </summary>
    public class SliderComponent : Component
    {
        private Image UIFill;
        private Color start, end;

        private bool autoFill;
        private float autoSpeed;
        public delegate void AutoFillFunction();
        private AutoFillFunction fillComplete;

        public void InitComponent(GameObject _slot, Image _fill, Color _max, Color _end)
        {
            Init(_slot);
            UIFill = _fill;
            start = _max;
            end = _end;
            autoFill = false;
            UpdateFill(1);
        }

        public void AutoFill(float _speed, AutoFillFunction _function = null)
        {
            autoFill = true;
            autoSpeed = _speed;
            fillComplete = _function;
        }

        public void UpdateFill(float val)
        {
            state = CanvasState.Alive;
            UIFill.fillAmount = val;
            UIFill.color = Color.Lerp(end, start, val);
            CurrentAliveTime = AliveTime;
        }

        //Auto fill functionality for bar
        public override void Update()
        {
            if(autoFill)
            {
                if (UIFill.fillAmount != 1.0f)
                {
                    UpdateFill(Mathf.Min(UIFill.fillAmount + (autoSpeed * Time.deltaTime), 1.0f));
                    if(UIFill.fillAmount == 1.0f)
                    {
                        if (fillComplete != null)
                        {
                            fillComplete();
                        }
                    }
                }
            }
            base.Update();
        }
    }

    /// <summary>
    /// Counter Component 
    /// for UI elements
    /// </summary>
    public class CounterComponent : Component
    {
        private Text UICount;

        public void InitComponent(GameObject _slot, Text _count)
        {
            Init(_slot);
            UICount = _count;
        }

		public void UpdateCount(string val){
			state = CanvasState.Alive;
			CurrentAliveTime = AliveTime;
			UICount.text = val;
		}
    }
}