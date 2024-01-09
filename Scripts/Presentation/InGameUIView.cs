using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202312
{
    public class InGameUIView : MonoBehaviour
    {
        [SerializeField]
        private Button _packingButton;

        private void Awake()
        {
            _packingButton.gameObject.SetActive(false);
        }

        public IObservable<Unit> OnClickPackingButtonAsObservable() => _packingButton.OnClickAsObservable();

        public void ShowPackingButton() => _packingButton.gameObject.SetActive(true);
        public void HidePackingButton() => _packingButton.gameObject.SetActive(false);
    }
}