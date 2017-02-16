using UnityEngine;
using System.Collections;
namespace BaseGameLogic.Audio
{   
    /// <summary>
    /// Base sound effect manager.
    /// </summary>
    public class BaseSoundEffectManager : MonoBehaviour 
    {
        protected const string audioSourceHostObjectName = "ObjectWithAudioSources";

        /// <summary>
        /// The reference to object with SoundSource attached to.
        /// </summary>
        [SerializeField]
        protected GameObject objectWithSoundEffects = null;

        /// <summary>
        /// Basic initialization BaseSoundEffectManager.
        /// </summary>
        public virtual void Initialize() 
        {
            CheackAudioSourceHostObject();
        }

        /// <summary>
        /// Cheacks the audioSourceHostObjectName exist.
        /// </summary>
        protected virtual void CheackAudioSourceHostObject()
        {            
            if (objectWithSoundEffects == null)
            {
                objectWithSoundEffects = new GameObject();
                objectWithSoundEffects.name = audioSourceHostObjectName;
                objectWithSoundEffects.transform.SetParent(this.transform);
                objectWithSoundEffects.transform.localPosition = Vector3.zero;
            }
        }
    }
}
