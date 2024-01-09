using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Kameffee.Audio;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1week202312
{
    public class PackingBoxPerformer : MonoBehaviour
    {
        [SerializeField]
        private Transform _root;

        [SerializeField]
        private GameObject _cover;

        [SerializeField]
        private GameObject _coverRight;

        [SerializeField]
        private GameObject _coverLeft;

        [SerializeField]
        private GameObject _tape;
        
        [SerializeField]
        private TapePerformer _tapePerformer;

        [SerializeField]
        private Vector3 _entryPosition;

        [SerializeField]
        private Vector3 _hidePosition;

        [Inject]
        private readonly AudioPlayer _audioPlayer;

        private Vector3 _defaultPosition;

        private void Awake()
        {
            _defaultPosition = _root.transform.position;

            _root.transform.position = _hidePosition;
            _root.transform.localScale = Vector3.one;
            _coverRight.SetActive(false);
            _coverLeft.SetActive(false);
            // _tape.SetActive(false);
        }

        private void Start()
        {
            var lifetimeScope = LifetimeScope.Find<LifetimeScope>();
            lifetimeScope.Container.Inject(this);
        }

        public async UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            _coverRight.SetActive(false);
            _coverLeft.SetActive(false);
            _tapePerformer.Hide();
            _root.transform.position = _entryPosition;
            _root.transform.localScale = Vector3.one;

            var sequence = DOTween.Sequence();
            sequence.Join(_root.DOMove(_defaultPosition, 0.5f).SetEase(Ease.OutCubic));
            sequence.SetLink(gameObject);
            sequence.WithCancellation(cancellationToken);

            await sequence.Play();
        }

        public async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_root.DOMove(_hidePosition, 0.3f).SetEase(Ease.InOutCubic));
            sequence.SetLink(gameObject);
            sequence.WithCancellation(cancellationToken);

            await sequence.Play();
        }

        public async UniTask CloseCoverRightAsync(CancellationToken cancellationToken = default)
        {
            var sequence = DOTween.Sequence();
            sequence.Join(_root.DOScale(0.95f, 0.2f).SetEase(Ease.InOutCubic));
            sequence.AppendCallback(() =>
            {
                _coverRight.SetActive(true);
                _audioPlayer.PlaySe("InGame/PackingBox-Close");
            });
            sequence.Append(_root.DOScale(1f, 0.3f).SetEase(Ease.InOutCubic));
            sequence.SetLink(gameObject);
            sequence.WithCancellation(cancellationToken);

            await sequence.Play();
        }

        public async UniTask CloseCoverLeftAsync(CancellationToken cancellationToken = default)
        {
            var sequence = DOTween.Sequence();
            sequence.Join(_root.DOScale(0.95f, 0.2f).SetEase(Ease.InOutCubic));
            sequence.AppendCallback(() =>
            {
                _coverLeft.SetActive(true);
                _audioPlayer.PlaySe("InGame/PackingBox-Close");
            });
            sequence.Append(_root.DOScale(1f, 0.3f).SetEase(Ease.InOutCubic));
            sequence.SetLink(gameObject);
            sequence.WithCancellation(cancellationToken);

            await sequence.Play();
        }

        public async UniTask TapeAsync(CancellationToken cancellationToken = default)
        {
            await _tapePerformer.PlayAsync(cancellationToken);
        }

        public async UniTask PackingAsync(CancellationToken cancellationToken = default)
        {
            await CloseCoverRightAsync(cancellationToken);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: cancellationToken);
            await CloseCoverLeftAsync(cancellationToken);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: cancellationToken);
            await TapeAsync(cancellationToken);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: cancellationToken);
        }
    }
}