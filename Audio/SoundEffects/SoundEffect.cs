using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Audio
{
    /// <summary>
    /// Sound effect.
    /// Class to manage audio clips and sources.
    /// </summary>
    [Serializable]
    public class SoundEffect
    {
        [SerializeField, Tooltip("Unique sound effect ID.")]
        private string soundEffectId = string.Empty;
        public string SoundEffectId {
            get { return this.soundEffectId; }
        }

        /// <summary>
        /// Defines if audio clips with in SoundEffect will be played in sequence or at random.
        /// </summary>
        [SerializeField]
        private SoundEffectPlayBackModeEnum playBackMode = SoundEffectPlayBackModeEnum.InSequence;
        public SoundEffectPlayBackModeEnum PlayBackMode
        {
            get { return this.playBackMode; }
            set { playBackMode = value; }
        }

        [SerializeField, Tooltip("List of audio clips used in sound effects.")]
        private List<AudioClip> _clips = new List<AudioClip>();

        /// <summary>
        /// The index of the current audio source.
        /// </summary>
        private int currentAudioSourceIndex = 0;

        /// <summary>
        /// The audio sources.
        /// </summary>
        [SerializeField, HideInInspector]
        private List<AudioSource> _audioSources = new List<AudioSource>();

        /// <summary>
        /// Gets the audio sources list copy.
        /// </summary>
        /// <value>The audio sources.</value>
        public AudioSource[] AudioSources
        {
            get 
            { 
                AudioSource[] audioSourcesCopy = null;
                _audioSources.CopyTo(audioSourcesCopy);
                return audioSourcesCopy; 
            }
        }

        /// <summary>
        /// The current audio source. Selected base on play back mode.
        /// </summary>
        private AudioSource _currentAudioSource = null; 
        public AudioSource CurrentAudioSource 
        {
            get 
            {
                if (_currentAudioSource != null && _currentAudioSource.time != 0f)
                {
                    return _currentAudioSource;
                }

                if (_audioSources.Count == 1)
                {
                    currentAudioSourceIndex = 0;
                    _currentAudioSource = _audioSources[currentAudioSourceIndex];
                }
                else
                {
                    switch (playBackMode)
                    {
                        case SoundEffectPlayBackModeEnum.InSequence:
                            if (currentAudioSourceIndex > _audioSources.Count - 1)
                                currentAudioSourceIndex = 0;
                            _currentAudioSource = _audioSources[currentAudioSourceIndex++];
                            break;

                        case SoundEffectPlayBackModeEnum.AtRandom:
                            currentAudioSourceIndex = UnityEngine.Random.Range(0, _audioSources.Count - 1);
                            _currentAudioSource = _audioSources[currentAudioSourceIndex];
                            break;
                    }
                }
                    
                return _currentAudioSource; 
            }
        }

        /// <summary>
        /// Creates the audio source on a specified object using provided audio clip .
        /// </summary>
        /// <returns>The audio source.</returns>
        /// <param name="hostObject">Object to with new audio source will be attached to.</param>
        /// <param name="clip">Clip that will be used in new audio source.</param>
        private AudioSource CreateAudioSource(GameObject hostObject, AudioClip clip)
        {
            AudioSource newAudioSource = hostObject.AddComponent<AudioSource>();
            newAudioSource.clip = clip;
            newAudioSource.playOnAwake = false;

            return newAudioSource;
        }

        /// <summary>
        /// Initialize the sound effect on the provided object.
        /// </summary>
        /// <param name="hostObject">Host object.</param>
        public void Initialize(GameObject hostObject)
        {
            AudioSource newAudioSource = null;

            for (int i = 0; i < _clips.Count; i++)
            {
                if (i < _audioSources.Count)
                {
                    if (_audioSources[i] != null)
                        _audioSources[i].clip = _clips[i];
                    else
                    {
                        newAudioSource = CreateAudioSource(hostObject, _clips[i]);
                        _audioSources[i] = newAudioSource;
                    }
                }
                else
                {
                    newAudioSource = CreateAudioSource(hostObject, _clips[i]);
                    _audioSources.Add(newAudioSource);
                }
            }
        }

        /// <summary>
        /// Play this sound effect.
        /// </summary>
        public void Play()
        {
            AudioSource audioSource = null;
            audioSource = CurrentAudioSource;

            if(audioSource != null && audioSource.time == 0f)
                audioSource.Play();
        }
    }
}