using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using Unity1week202312.CellSize;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Unity1week202312.PlaceObjects
{
    public class PlaceObjectHolderElementView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
        IPointerClickHandler
    {
        public int Index => _index;
        public PlaceObjectView Element => _placeObjectView;
        public bool IsSelected { get; private set; }

        private bool IsEmpty => _placeObjectView == null;

        [SerializeField]
        private int _index;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [FormerlySerializedAs("_cellSizeInfoView")]
        [SerializeField]
        private CellAreaInfoView _cellAreaInfoView;

        private PlaceObjectView _placeObjectView;
        private Tween _tween;
        private bool _isAvailable = true;

        private readonly Subject<int> _onClick = new();

        private void Awake()
        {
            _spriteRenderer.DOFade(0, 0);
            transform.localScale = Vector3.zero;
        }

        public IObservable<int> OnClickAsObservable() => _onClick;

        public void Set(PlaceObjectView placeObjectView)
        {
            _placeObjectView = placeObjectView;
            _placeObjectView.transform.SetParent(transform);
            _placeObjectView.SetMiniScale();
            _placeObjectView.transform.localPosition = new Vector3(
                -(_placeObjectView.Size.x - 1) * 0.5f,
                (_placeObjectView.Size.y - 1) * 0.5f,
                0);

            var viewModel = new CellAreaInfoView.ViewModel(_placeObjectView.CellSizeMasterData.GetCellArea());
            _cellAreaInfoView.ApplyViewModel(viewModel);
            _cellAreaInfoView.Show();
        }

        public void Remove()
        {
            Destroy(_placeObjectView.gameObject);
            _placeObjectView = null;
            _cellAreaInfoView.Hide();
        }

        public void Select()
        {
            IsSelected = true;

            _tween?.Kill();
            _tween = _spriteRenderer.DOFade(0.5f, 1f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject);
        }

        public void Deselect()
        {
            IsSelected = false;

            _tween?.Kill();
            _tween = _spriteRenderer.DOFade(1f, 0.5f)
                .SetLink(gameObject);
        }

        public void SetAvailable(bool isAvailable)
        {
            _isAvailable = isAvailable;
        }

        public async UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            transform.localScale = Vector3.zero;
            _spriteRenderer.DOFade(0, 0);

            var sequence = DOTween.Sequence();
            sequence.Join(transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack));
            sequence.Join(_spriteRenderer.DOFade(1, 0.2f).SetEase(Ease.Linear));
            sequence.SetLink(gameObject);
            sequence.WithCancellation(cancellationToken);
            await sequence.Play();
        }

        public async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            var sequence = DOTween.Sequence();
            sequence.Join(transform.DOScale(0f, 0.2f).SetEase(Ease.InBack));
            sequence.Join(_spriteRenderer.DOFade(0, 0.2f).SetEase(Ease.Linear));
            sequence.SetLink(gameObject);
            sequence.WithCancellation(cancellationToken);
            await sequence.Play();
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (IsEmpty) return;
            if (!_isAvailable) return;

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _onClick.OnNext(_index);
            }
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (IsEmpty) return;
            if (!_isAvailable) return;

            transform.DOScale(Vector3.one * 1.05f, 0.2f)
                .SetEase(Ease.OutCubic);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (IsEmpty) return;
            if (!_isAvailable) return;

            transform.DOScale(Vector3.one, 0.2f)
                .SetEase(Ease.OutCubic);
        }
    }
}