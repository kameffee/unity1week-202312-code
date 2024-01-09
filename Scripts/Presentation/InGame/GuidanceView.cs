using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Unity1week202312.InGame
{
    public class GuidanceView : MonoBehaviour
    {
        public enum StateType
        {
            None,
            NotSelected,
            Selected,
            NotPlaceable,
        }

        [Serializable]
        public class State
        {
            public StateType Type => _type;
            public GameObject Root => _root;

            [SerializeField]
            private StateType _type;

            [SerializeField]
            private GameObject _root;
        }

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private List<State> _states;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            Set(StateType.None);
        }

        public void Set(StateType stateType)
        {
            foreach (var x in _states)
            {
                x.Root.SetActive(x.Type == stateType);
            }
        }

        public async UniTask ShowAsync(StateType stateType, CancellationToken cancellationToken = default)
        {
            Set(stateType);

            await _canvasGroup.DOFade(1, 0.2f)
                .WithCancellation(cancellationToken);
        }

        public async UniTask HideAsync(CancellationToken cancellationToken)
        {
            await _canvasGroup.DOFade(0, 0.2f)
                .WithCancellation(cancellationToken);
        }
    }
}