using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202312.Result
{
    public class ResultCanvasView : MonoBehaviour
    {
        [SerializeField]
        private Button _goToTitleButton;
        
        [SerializeField]
        private RectTransform _contentRoot;

        private GameObject _content;
        
        public IObservable<Unit> OnClickGoToTitleButtonAsObservable() => _goToTitleButton.OnClickAsObservable();

        public void ApplyViewModel(ViewModel viewModel)
        {
            _content = Instantiate(viewModel.ContentPrefab, _contentRoot);
        }

        public class ViewModel
        {
            public GameObject ContentPrefab { get; }

            public ViewModel(GameObject contentPrefab)
            {
                ContentPrefab = contentPrefab;
            }
        }
    }
}