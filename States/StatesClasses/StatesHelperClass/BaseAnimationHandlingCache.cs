using UnityEngine;
using System.Collections;
namespace BaseGameLogic.States
{
    public class BaseAnimationHandlingCache
    {
		public void ExtractAnimationHandlingDataContainer(
			AnimationHandlingData animationHandlingData, 
			string animationHandlingDataContainerName, 
			ref AnimationHandlingDataContainer animationHandlingDataContainer)
		{
			if (animationHandlingData != null) 
			{
				if (animationHandlingDataContainer == null) 
				{
					animationHandlingDataContainer = animationHandlingData.GetAnimationHandlingDataCointainer (animationHandlingDataContainerName);
				}
			}
		}
    }
}
