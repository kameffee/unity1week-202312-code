using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202312.Title
{
    public class TitleCanvasView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private Button _startButton;

        [SerializeField]
        private Button _licenseButton;

        public IObservable<Unit> OnClickStartAsObservable() => _startButton.OnClickAsObservable();
        public IObservable<Unit> OnClickLicenseAsObservable() => _licenseButton.OnClickAsObservable();

        public void SetAvailable(bool isAvailable)
        {
            _canvasGroup.blocksRaycasts = isAvailable;
        }
    }
}