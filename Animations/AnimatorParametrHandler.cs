using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Animation
{
	[Serializable]
	public class AnimatorParametrHandler
	{
		[SerializeField] private Animator _animator = null;
		[SerializeField] private string _name = string.Empty;
		[SerializeField] private int _nameHash = 0;

		public void Set()
		{
			Set(_animator);
		}

		public void Set(float value)
		{
			Set(_animator, value);
		}

		public void Set(bool value)
		{
			Set(_animator, value);
		}

		public void Set(Animator animator)
		{
			animator.SetTrigger(_nameHash);
		}
		
		public void Set(Animator animator, float value)
		{
			animator.SetFloat(_nameHash, value);
		}

		public void Set(Animator animator, bool value)
		{
			animator.SetBool(_nameHash, value);
		}
	}
}
