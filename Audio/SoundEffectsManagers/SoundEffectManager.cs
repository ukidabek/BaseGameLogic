using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Audio
{   
    /// <summary>
    /// Dezigned to handling sound effects envoked by animation events.
    /// </summary>
    public class SoundEffectManager : BaseSoundEffectManager 
    {
        [SerializeField, Header("On demand sound effects.")]
        private List<SoundEffect> _soundEffectsList = new List<SoundEffect>();
        private Dictionary<string , SoundEffect> _soundEffectsDictionaty = new Dictionary<string, SoundEffect>();


        protected void Awake()
        {
            foreach (SoundEffect soundEffectGroup in _soundEffectsList)
            {
                string key = soundEffectGroup.SoundEffectId;
                _soundEffectsDictionaty.Add(key, soundEffectGroup);
            }
        }

        /// <summary>
        /// Initialize this all soundEffect .
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            foreach (SoundEffect soundEffect in _soundEffectsList)
            {
                soundEffect.Initialize(objectWithSoundEffects);
            }
        }

        /// <summary>
        /// Play the sound effect of given id.
        /// </summary>
        /// <param name="soundEffectId">Sound effect identifier.</param>
        public void PlaySoundEffect(string soundEffectId)
        {
            SoundEffect soundEffectGroup = GetSoundEffect(soundEffectId);

            if (soundEffectGroup != null)
            {
                soundEffectGroup.Play();
            }
        }

        /// <summary>
        /// Returns the sound effect of given id.
        /// </summary>
        /// <returns>The sound effect.</returns>
        /// <param name="soundEffectId">Sound effect identifier.</param>
        public SoundEffect GetSoundEffect(string soundEffectId)
        {
            SoundEffect soundEffectGroup = null;
            bool tryGetValue = _soundEffectsDictionaty.TryGetValue(soundEffectId, out soundEffectGroup);
            if (!tryGetValue)
            {
                _soundEffectsDictionaty.Add(soundEffectId, soundEffectGroup);
            }

            return soundEffectGroup;
        }            
    }
}
