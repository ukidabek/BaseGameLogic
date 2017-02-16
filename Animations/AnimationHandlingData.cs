using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic
{
    [CreateAssetMenuAttribute(menuName = "AnimationData/AnimationHandlingData", fileName = "AnimationHandlingData.asset")]
    public class AnimationHandlingData : ScriptableObject 
    {
        [SerializeField]
        protected List<AnimationHandlingDataContainer> parametrs = new List<AnimationHandlingDataContainer>();
        public List<AnimationHandlingDataContainer> Parametrs {
            get { return this.parametrs; } 
        }

        public virtual void Initialization()
        {
            foreach (AnimationHandlingDataContainer item in parametrs)
            {
                item.InitializeParameter();
            }
        }

        public AnimationHandlingDataContainer GetAnimationHandlingDataCointainer(string parameterName)
        {
            foreach (AnimationHandlingDataContainer parametr in parametrs)
            {
				if (parametr.cointainerName.Equals(parameterName))
                {
                    return parametr;
                }
            }

            return null;
        }

        public AnimationHandlingDataContainer GetParameter(string cointainerName)
        {
            AnimationHandlingDataContainer _cointainer = null;
            foreach (AnimationHandlingDataContainer cointainer in Parametrs)
            {
                if (cointainer.cointainerName.Equals(cointainerName))
                {
                    _cointainer = cointainer;
                    break;
                }
            }

            if (_cointainer == null)
            {
                throw new NoAnimationHandlingDataCointainerException(cointainerName);
            }

            return _cointainer;
        }


        public int GetParameterNameID(string parameterName)
        {
            AnimationHandlingDataContainer animationHandlingDataCointainer = GetAnimationHandlingDataCointainer(parameterName);
            if (animationHandlingDataCointainer == null)
                return 0;
            else
                return animationHandlingDataCointainer.parameterNameHashID;
        }

    }
}