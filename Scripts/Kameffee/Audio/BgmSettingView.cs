using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Kameffee.Audio
{
    public class BgmSettingView : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;

        public void SetVolume(AudioVolume volume) => _slider.value = volume.Value;

        public IObservable<float> OnChangeVolumeAsObservable() => _slider.OnValueChangedAsObservable();
    }
}