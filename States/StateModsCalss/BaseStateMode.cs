using UnityEngine;

using System.Collections;

namespace BaseGameLogic.States
{
    /// <summary>
    /// Base state mode.
    /// </summary>
	public class BaseStateMode
	{
        public BaseStateMode(BaseState hostState)
        {
        }

		protected StateModeBlending _updateBlending = StateModeBlending.Additive;
		public StateModeBlending UpdateBlending {
			get { return this._updateBlending; }
			set { _updateBlending = value; }
		}

		protected StateModeBlending _lateUpdateBlending = StateModeBlending.Additive;
		public StateModeBlending FixedUpdateBlending {
			get { return this._fixedUpdateBlending; }
			set { _fixedUpdateBlending = value; }
		}
		

		protected StateModeBlending _fixedUpdateBlending = StateModeBlending.Additive;
		public StateModeBlending LateUpdateBlending {
			get { return this._lateUpdateBlending; }
			set { _lateUpdateBlending = value; }
		}

        public virtual void ModAdded()
        {
        }

        public virtual void ModRemoved()
        {
        }

		public virtual StateModeBlending Update()
		{
			return _updateBlending;
		}

		public virtual StateModeBlending FixedUpdate()
		{
			return _fixedUpdateBlending;
		}

		public virtual StateModeBlending LateUpdate()
		{
			return _fixedUpdateBlending;
		}

	}

	public enum StateModeBlending
	{
		Override = 0,
		Additive = 10
	}

	public enum StateModeUpdate
	{
		Normal,
		Fixed,
		Late
	}
}