using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Animation
{
	[Serializable]
	public class AnimatorParametrHandler
	{
		[SerializeField] private RuntimeAnimatorController _animator = null;
        public RuntimeAnimatorController Animator { get { return _animator; } }

        [SerializeField] private string _name = string.Empty;
        public string Name { get { return _name; } }

        [SerializeField] private int _nameHash = 0;
        public int NameHash { get { return _nameHash; } }


        public void Set(Animator animator)
		{
			animator.SetTrigger(NameHash);
		}
		
		public void Set(Animator animator, float value)
		{
			animator.SetFloat(NameHash, value);
		}

		public void Set(Animator animator, bool value)
		{
			animator.SetBool(NameHash, value);
		}
	}
}
