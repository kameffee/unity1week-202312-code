using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Unity1week202312
{
    public class TapePerformer : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer _lineRenderer;

        [SerializeField]
        private int _targetIndex = 1;

        [SerializeField]
        private Vector3 _fromPosition;

        [SerializeField]
        private Vector3 _toPosition;

        [SerializeField]
        private float _duration = 1f;

        private Vector3 _currentPosition;

        private void Awake()
        {
            _lineRenderer.SetPosition(_targetIndex, _fromPosition);
        }

        public async UniTask PlayAsync(CancellationToken cancellationToken = default)
        {
            _currentPosition = _fromPosition;
            _lineRenderer.SetPosition(_targetIndex, _currentPosition);

            await DOTween.To(
                    () => _currentPosition,
                    x =>
                    {
                        _currentPosition = x;
                        _lineRenderer.SetPosition(_targetIndex, _currentPosition);
                    },
                    _toPosition,
                    _duration)
                .SetEase(Ease.InOutCirc);
        }

        public void Hide()
        {
            _lineRenderer.SetPosition(_targetIndex, _fromPosition);
        }
    }
}