using Kameffee.Audio;
using Kameffee.Extensions;
using UniRx;
using Unity1week202312.InGame;
using UnityEngine;
using VContainer.Unity;

namespace Unity1week202312.PlaceObjects
{
    public class PlaceObjectPresenter : Presenter, IInitializable
    {
        private readonly PlaceObjectViewFactory _placeObjectViewFactory;
        private readonly PlaceObjectHolderView _placeObjectHolderView;
        private readonly SelectPlaceObjectUseCase _selectPlaceObjectUseCase;
        private readonly NextPlaceObjectUseCase _nextPlaceObjectUseCase;
        private readonly DraggingPlaceObjectFactory _draggingPlaceObjectFactory;
        private readonly AudioPlayer _audioPlayer;
        private readonly InGameUserState _inGameUserState;

        public PlaceObjectPresenter(
            PlaceObjectViewFactory placeObjectViewFactory,
            PlaceObjectHolderView placeObjectHolderView,
            SelectPlaceObjectUseCase selectPlaceObjectUseCase,
            NextPlaceObjectUseCase nextPlaceObjectUseCase,
            DraggingPlaceObjectFactory draggingPlaceObjectFactory,
            AudioPlayer audioPlayer,
            InGameUserState inGameUserState)
        {
            _placeObjectViewFactory = placeObjectViewFactory;
            _placeObjectHolderView = placeObjectHolderView;
            _selectPlaceObjectUseCase = selectPlaceObjectUseCase;
            _nextPlaceObjectUseCase = nextPlaceObjectUseCase;
            _draggingPlaceObjectFactory = draggingPlaceObjectFactory;
            _audioPlayer = audioPlayer;
            _inGameUserState = inGameUserState;
        }

        public void Initialize()
        {
            _nextPlaceObjectUseCase.OnSetAsObservable()
                .Subscribe(tuple => CreatePlaceObjectView(tuple.holderIndex, tuple.placeObject))
                .AddTo(this);

            _nextPlaceObjectUseCase.OnRemoveAsObservable()
                .Subscribe(tuple => _placeObjectHolderView.Remove(tuple.HolderIndex))
                .AddTo(this);

            _placeObjectHolderView.OnSelectAsObservable()
                .Subscribe(index => OnClickHolderElement(index, _placeObjectHolderView.Get(index)))
                .AddTo(this);

            _selectPlaceObjectUseCase.OnCancelAsObservable()
                .Subscribe(operationPlaceObject => Cancel(operationPlaceObject))
                .AddTo(this);

            _selectPlaceObjectUseCase.OnSubmitAsObservable()
                .Subscribe(index => _placeObjectHolderView.Deselect(index))
                .AddTo(this);

            _inGameUserState.State
                .Where(type => type is InGameUserStateType.Packing or InGameUserStateType.Grabbing)
                .Subscribe(_ => _placeObjectHolderView.SetAvailableAll(false))
                .AddTo(this);

            _inGameUserState.State
                .Where(type => type == InGameUserStateType.Ready)
                .Subscribe(_ => _placeObjectHolderView.SetAvailableAll(true))
                .AddTo(this);
        }

        private void OnClickHolderElement(int holderIndex, PlaceObjectView placeObjectView)
        {
            Select(holderIndex, placeObjectView);
        }

        private void Select(int holderIndex, PlaceObjectView placeObjectView)
        {
            _audioPlayer.PlaySe("InGame/PlaceObject-Select");

            // 選択状態にする
            _placeObjectHolderView.Select(holderIndex);

            // マウスの位置にオブジェクトを生成
            var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;

            var placeObjectId = placeObjectView.Id;
            var draggingPlaceObjectView = _draggingPlaceObjectFactory.Create(placeObjectId, position);

            _selectPlaceObjectUseCase.Select(holderIndex, draggingPlaceObjectView);
            _inGameUserState.SetState(InGameUserStateType.Grabbing);
        }

        private void Cancel(OperationPlaceObject operationPlaceObject)
        {
            _audioPlayer.PlaySe("InGame/PlaceObject-Cancel");
            Object.Destroy(operationPlaceObject.TargetPlaceObjectView.gameObject);
            _placeObjectHolderView.Deselect(operationPlaceObject.HolderIndex);
            _inGameUserState.SetState(InGameUserStateType.Ready);
        }

        private void CreatePlaceObjectView(int index, PlaceObject placeObject)
        {
            var placeObjectView = _placeObjectViewFactory.Create(placeObject.Id);
            var viewModel = new PlaceObjectView.ViewModel(placeObject.Id);
            placeObjectView.ApplyViewModel(viewModel);
            placeObjectView.SetMiniScale();
            _placeObjectHolderView.Set(index, placeObjectView);
        }
    }
}