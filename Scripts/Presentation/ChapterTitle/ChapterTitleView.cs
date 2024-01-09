using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity1week202312.ChapterTitle
{
    public class ChapterTitleView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private CanvasGroup _titleCanvasGroup;

        [SerializeField]
        private TextMeshProUGUI _chapterTitleText;

        [SerializeField]
        private Transform _chapterTitleImageRoot;

        [SerializeField]
        private CanvasGroup _contentCanvasGroup;

        private GameObject _prefab;

        private void Awake()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _contentCanvasGroup.alpha = 0;
            _titleCanvasGroup.alpha = 0;
        }

        public void ApplyViewModel(ViewModel viewModel)
        {
            if (_prefab != null)
            {
                Destroy(_prefab);
            }

            _chapterTitleText.text = viewModel.ChapterTitle;
            Assert.IsNotNull(viewModel.ImagePrefab, "viewModel.ImagePrefab が null です。");
            _prefab = Instantiate(viewModel.ImagePrefab, _chapterTitleImageRoot);
        }

        public async UniTask PlayAsync(CancellationToken cancellationToken = default)
        {
            var sequence = DOTween.Sequence();
            sequence.Join(_canvasGroup.DOFade(1, 1));
            sequence.AppendInterval(0.5f);
            sequence.Append(_contentCanvasGroup.DOFade(1, 0.5f));
            sequence.Append(_titleCanvasGroup.DOFade(1, 0.5f));
            sequence.WithCancellation(cancellationToken);
            await sequence.Play();

            await UniTask.WhenAny(
                UniTask.Delay(TimeSpan.FromSeconds(3), cancellationToken: cancellationToken),
                UniTask.WaitUntil(() => Input.anyKeyDown, cancellationToken: cancellationToken),
                UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: cancellationToken)
            );

            await _titleCanvasGroup.DOFade(0, 0.5f)
                .WithCancellation(cancellationToken);

            await _canvasGroup.DOFade(0, 1)
                .WithCancellation(cancellationToken);
        }

        public class ViewModel
        {
            public string ChapterTitle { get; }

            public GameObject ImagePrefab { get; }

            public ViewModel(string chapterTitle, GameObject imagePrefab)
            {
                ChapterTitle = chapterTitle;
                ImagePrefab = imagePrefab;
            }
        }
    }
}