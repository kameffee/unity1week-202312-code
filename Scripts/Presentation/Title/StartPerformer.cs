using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Kameffee.Audio;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1week202312.Title
{
    public class StartPerformer : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _boxRoot;

        [SerializeField]
        private CanvasGroup _titleCanvasGroup;

        [SerializeField]
        private GameObject _coverBottom;

        [SerializeField]
        private GameObject _tape;

        [Inject]
        private readonly AudioPlayer _audioPlayer;

        private void Awake()
        {
            _titleCanvasGroup.alpha = 1f;
            _coverBottom.SetActive(false);
            _tape.SetActive(false);
        }

        private void Start()
        {
            var lifetimeScope = LifetimeScope.Find<LifetimeScope>();
            lifetimeScope.Container.Inject(this);
        }

        public async UniTask PlayAsync(CancellationToken cancellationToken = default)
        {
            await _boxRoot.DOScale(Vector3.one * 0.9f, 0.2f)
                .SetEase(Ease.OutBack)
                .WithCancellation(cancellationToken);

            await _titleCanvasGroup.DOFade(0, .2f)
                .WithCancellation(cancellationToken);

            await UniTask.Delay(TimeSpan.FromSeconds(0.6f), cancellationToken: cancellationToken);

            _coverBottom.SetActive(true);

            _audioPlayer.PlaySe("Title/CloseCover");

            await UniTask.Delay(TimeSpan.FromSeconds(0.6), cancellationToken: cancellationToken);

            _tape.SetActive(true);
            _audioPlayer.PlaySe("Title/CloseCover");
        }
    }
}