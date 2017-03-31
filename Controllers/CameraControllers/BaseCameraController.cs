using UnityEngine;
using System.Collections;

using BaseGameLogic.States;

namespace BaseGameLogic
{
    public class BaseCameraController : BaseStateObject 
    {
        protected Camera controlledCamera = null;
		public Camera ControlledCamera {
			get{ return controlledCamera; }
		}


        [SerializeField]
        public Transform target = null;

        [SerializeField]
        protected CameraSettings settings;
        public CameraSettings Settings
        {
            get { return this.settings; } 
        }

        protected override void Awake()
        {
            base.Awake();

            // Unparentign object. 
            this.transform.SetParent(null);

            controlledCamera = this.GetComponent<Camera>();
            #if UNITY_EDITOR
            MissingWarning(controlledCamera, gameObject.name);
            #endif
        }
    }
}
