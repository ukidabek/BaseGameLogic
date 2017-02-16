using UnityEngine;
using System.Collections;

namespace BaseGameLogic.Audio
{   
    public class OnCollisionSoundEffectManager : BaseSoundEffectManager 
    {
        [SerializeField, Header("Enable trigger & colliders events sound effects.")]
        private bool _useOnTriggerEnterSoundEffect = false; 
        [SerializeField]
        private bool _useOnTriggerStayEffect = false; 
        [SerializeField]
        private bool _useOnTriggerExitEffect = false; 
        [SerializeField]
        private bool _useOnCollisionEnterSoundEffect = false;
        [SerializeField]
        private bool _useOnCollisionStaySoundEffect = false; 
        [SerializeField]
        private bool _useOnCollisionExitSoundEffect = false; 

        //  Triggers sound effect
        [SerializeField, Header("Trigger & colliders events sound effects.")]
        private SoundEffect _onTriggerEnterSoundEffect = new SoundEffect();

        [SerializeField]
        private SoundEffect _onTriggerStaySoundEffect = new SoundEffect();

        [SerializeField]
        private SoundEffect _onTriggerExitSoundEffect = new SoundEffect();

        //  Collider sound effect
        [SerializeField]
        private SoundEffect _onCollisionEnterSoundEffect = new SoundEffect();

        [SerializeField]
        private SoundEffect _onCollisionStaySoundEffect = new SoundEffect();

        [SerializeField]
        private SoundEffect _onCollisionExitSoundEffect = new SoundEffect();

        public override void Initialize()
        {
            base.Initialize();

            _onTriggerEnterSoundEffect.Initialize(objectWithSoundEffects);
            _onTriggerStaySoundEffect.Initialize(objectWithSoundEffects);
            _onTriggerExitSoundEffect.Initialize(objectWithSoundEffects);

            _onCollisionEnterSoundEffect.Initialize(objectWithSoundEffects);
            _onCollisionStaySoundEffect.Initialize(objectWithSoundEffects);
            _onCollisionExitSoundEffect.Initialize(objectWithSoundEffects);
        }

        public virtual void OnTriggerEnter(Collider collision)
        {
            if (!_useOnTriggerEnterSoundEffect)
                return;

            _onTriggerEnterSoundEffect.Play();
        }

        public virtual void OnTriggerExit(Collider collision) 
        {
            if (!_useOnTriggerExitEffect)
                return;

            _onTriggerExitSoundEffect.Play();
        }

        public virtual void OnTriggerStay(Collider collision)
        {
            if (!_useOnTriggerStayEffect)
                return;

            _onTriggerStaySoundEffect.Play();
        }

        public virtual void OnCollisionEnter(Collision collision)
        {
            if (!_useOnCollisionEnterSoundEffect)
                return;

            _onCollisionEnterSoundEffect.Play();
        }

        public virtual void OnCollisionStay(Collision collision)
        {
            if (!_useOnCollisionStaySoundEffect)
                return;

            _onCollisionStaySoundEffect.Play();
        }

        public virtual void OnCollisionExit(Collision collision)
        {
            if (!_useOnCollisionExitSoundEffect)
                return;

            _onCollisionExitSoundEffect.Play();
        }
    }
}