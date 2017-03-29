using UnityEngine;
using System.Collections;
namespace BaseGameLogic.TimeManagment
{
    /// <summary>
    /// Time manager.
    /// The purpose of this class is a time management.
    /// </summary>
    public class TimeManager : MonoBehaviour 
    {
        private const float _maximumDeltaTimeFactor = 3;

        private float _defaultTimeScale = 0f;
        private float _defaultFixedDeltaTime = 0f;
    	
        private TimeManagerModeEnum _currentMode = TimeManagerModeEnum.SlowMotion;
        /// <summary>
        /// The TimeManager mode.
        /// Determines how manager works.
        /// </summary>
        [SerializeField]
        private TimeManagerModeEnum _mode = TimeManagerModeEnum.SlowMotion;
        public TimeManagerModeEnum Mode
        {
            get { return this._mode; }
            set
            {
                _currentMode = _mode = value;
                SetNewTimeScale(_mode, _factor);
            }
        }

        private float _currentFactor = 0f;
        /// <summary>
        /// How much time will be accelerated or slow down.
        /// </summary>
        [SerializeField, Range(0f, 100f)]
        private float _factor = 1f;
        public float Factor {
            get { return _factor; }
            set
            {
                _currentFactor = _factor = value;
                SetNewTimeScale(_mode, _factor);
            }
        }
			
        protected void CacheDefaultTimeValues()
        {
            _defaultTimeScale = Time.timeScale;
            _defaultFixedDeltaTime = Time.fixedDeltaTime;
        }

        private void SetNewTimeScale(TimeManagerModeEnum mode, float factor)
        {
            switch (mode)
            {
                case TimeManagerModeEnum.FastMotion:
                    Time.timeScale = _defaultTimeScale * factor;
                    Time.maximumDeltaTime = Time.timeScale / _maximumDeltaTimeFactor; 
                    break;

                case TimeManagerModeEnum.SlowMotion:
                    if (factor != 0f)
                    {
                        Time.timeScale = _defaultTimeScale / factor;
                    }
                    else
                    {
                        Time.timeScale = 0f;
                    }
                    Time.maximumDeltaTime = 1f / _maximumDeltaTimeFactor; 
                    break;
            }

            Time.fixedDeltaTime = _defaultFixedDeltaTime * Time.timeScale;
        }

        protected void Awake()
        {
            CacheDefaultTimeValues();
            SetNewTimeScale(_mode, _factor);

            _currentFactor = _factor;
        }

        private void Update()
        {
            if (_currentFactor != _factor)
            {
                SetNewTimeScale(_mode, _factor);
                _currentFactor = _factor;
            }

            if (_currentMode != _mode)
            {
                SetNewTimeScale(_mode, _factor);
                _currentMode = _mode;
            }
        }
    }
}