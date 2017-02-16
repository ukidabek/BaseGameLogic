using UnityEngine;
using System.Collections;

namespace BaseGameLogic.Inputs
{
    public class BaseKeybardAndMouseImputSource : BaseInputSource 
    {
        public override InputSourceEnum InputSourceType
        {
            get { return InputSourceEnum.KeyboardAndMouse; }
        }

        [SerializeField]
        private ButtonInput _pauseButton = new ButtonInput();
        public ButtonInput PouseButton
        {
            get { return this._pauseButton; }
        }

        [SerializeField]
        private ButtonInput _forwad = new ButtonInput();
        public ButtonInput Forwad
        {
            get { return this._forwad; }
        }

        [SerializeField]
        private ButtonInput _backword = new ButtonInput();
        public ButtonInput Backword
        {
            get { return this._backword; }
        }

        [SerializeField]
        private ButtonInput _left= new ButtonInput();
        public ButtonInput Left
        {
            get { return this._left; }
        }

        [SerializeField]
        private ButtonInput _right= new ButtonInput();
        public ButtonInput Right
        {
            get { return this._right; }
        }

        public override Vector3 MovementVector
        {
            get
            {
                Vector3 newMovementVector = Vector3.zero;
                newMovementVector.x = _right.AnagloValue - _left.AnagloValue;
                newMovementVector.z = _forwad.AnagloValue - _backword.AnagloValue;
                return newMovementVector;
            }
        }
        
        public override Vector3 LookVector
        {
            get { return base.LookVector; }
        }
        
        public override bool PauseButtonDown
        {
            get { return _pauseButton.State == ButtonStateEnum.Down; }
        }

        protected override void Awake()
        {
            physicalInputs.Add(_pauseButton);
            physicalInputs.Add(_forwad);
            physicalInputs.Add(_backword);
            physicalInputs.Add(_left);
            physicalInputs.Add(_right);
            base.Awake();
        }
    }
}