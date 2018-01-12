using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.SceneManagement
{
    public abstract class LoadingScreenCanvas : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _loadingGizmo = null;

        [SerializeField]
        private Slider _loadingProgressSlide = null;

        [SerializeField]
        private float _rotationSpeed = 5f;

        public float LoadingProgress
        {
            get { return _loadingProgressSlide.value; }
            set { _loadingProgressSlide.value = value; }
        }

        private void Update()
        {
            Vector3 rotation = _loadingGizmo.transform.rotation.eulerAngles;
            rotation.z += _rotationSpeed * Time.deltaTime;
            _loadingGizmo.transform.rotation = Quaternion.Euler(rotation);
        }
    }
}