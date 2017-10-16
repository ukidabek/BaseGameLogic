using UnityEngine;

using System;
using System.Collections;
namespace BaseGameLogic.Inputs
{
    [Serializable]
    public class ButtonInput : PhysicalInput
    {
        
        private bool IsPressed = false;
        private bool WasPressed = false;

        [SerializeField]
        private ButtonStateEnum buttonState = ButtonStateEnum.Released;

        [SerializeField]
		public bool invert = false;

        [SerializeField]
        private float toAnalogConversionSpeed = 1f;

        public override bool PositiveReading
        {
            get 
			{
				return buttonState == ButtonStateEnum.Down ||
					buttonState == ButtonStateEnum.Held;
            }
        }

        public ButtonStateEnum State
        {
			get { return buttonState; }
			set { buttonState = value;}
        }

		public float AnalogTarget {
            get { return invert ? -1f : 1f; }
        }

		public bool Pressed {
			get{
				ButtonStateEnum tmpButtonState = State;

				return tmpButtonState == ButtonStateEnum.Down ||
				tmpButtonState == ButtonStateEnum.Held;
			}
				
		}

		// Tylko do wyświetlania w inspektorze.
        [SerializeField]
        private float anagloValue = 0f;
        public float AnagloValue
        {
            get {

				if (Pressed)
                {
                    return anagloValue = Mathf.MoveTowards(
                        anagloValue, 
                        AnalogTarget, 
                        toAnalogConversionSpeed * Time.deltaTime);
                }
                else
                {
                    return anagloValue = Mathf.MoveTowards(
                        anagloValue, 
                        0f, 
                        toAnalogConversionSpeed * Time.deltaTime);
                }
            }

			set { anagloValue = value; }
        }

        [SerializeField]
		public KeyCode keyCode;


		public override void Read ()
    	{
			WasPressed = IsPressed;
			IsPressed = Input.GetKey(keyCode);

			if (!IsPressed && !WasPressed)
			{
				buttonState = ButtonStateEnum.Released;
				return;
			} 
			else if (IsPressed && !WasPressed)
			{
				buttonState = ButtonStateEnum.Down;
				return;
			} 
			else if (IsPressed && WasPressed)
			{
				buttonState = ButtonStateEnum.Held;
				return;
			} 
			else if (!IsPressed && WasPressed)
			{
				buttonState = ButtonStateEnum.Up;
				return;
			}
    	}
    }
}