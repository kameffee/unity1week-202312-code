using System.Linq;
using Kameffee.Audio;
using Unity1week202312.GridCell;
using Unity1week202312.InGame;
using Unity1week202312.PlaceObjects;
using UnityEngine;
using VContainer.Unity;

namespace Unity1week202312.PlaceBoard
{
    public class InGameUpdater : ITickable
    {
        private readonly SelectPlaceObjectUseCase _selectPlaceObjectUseCase;
        private readonly PlaceUseCase _placeUseCase;
        private readonly NextPlaceObjectUseCase _nextPlaceObjectUseCase;
        private readonly RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];
        private readonly GridPresenter _gridPresenter;
        private readonly AudioPlayer _audioPlayer;
        private readonly InGameUIView _inGameUIView;
        private readonly PlaceableSpaceCalculator _placeableSpaceCalculator;
        private readonly InGameUserState _inGameUserState;
        private readonly PlaceBoardPresenter _placeBoardPresenter;

        public InGameUpdater(
            SelectPlaceObjectUseCase selectPlaceObjectUseCase,
            PlaceUseCase placeUseCase,
            NextPlaceObjectUseCase nextPlaceObjectUseCase,
            GridPresenter gridPresenter,
            AudioPlayer audioPlayer,
            InGameUIView inGameUIView,
            PlaceableSpaceCalculator placeableSpaceCalculator,
            InGameUserState inGameUserState,
            PlaceBoardPresenter placeBoardPresenter)
        {
            _selectPlaceObjectUseCase = selectPlaceObjectUseCase;
            _placeUseCase = placeUseCase;
            _nextPlaceObjectUseCase = nextPlaceObjectUseCase;
            _gridPresenter = gridPresenter;
            _audioPlayer = audioPlayer;
            _inGameUIView = inGameUIView;
            _placeableSpaceCalculator = placeableSpaceCalculator;
            _inGameUserState = inGameUserState;
            _placeBoardPresenter = placeBoardPresenter;
        }

        public void Tick()
        {
            _gridPresenter.UnFocusAll();

            if (!_selectPlaceObjectUseCase.IsSelected) return;

            if (TryGetPlaceObjectView(Input.mousePosition, out var gridCellView))
            {
                var originPosition = gridCellView.Position;
                var selectedOperationPlaceObject = _selectPlaceObjectUseCase.SelectedPlaceObject.Value;
                var cellSizeMasterData = selectedOperationPlaceObject.TargetPlaceObjectView.CellSizeMasterData;

                var relativePositions = cellSizeMasterData.GetCellPositions();
                var canPlace = _placeUseCase.CanPlace(originPosition, relativePositions);

                if (Input.GetMouseButtonDown(0))
                {
                    if (!canPlace) return;

                    _audioPlayer.PlaySe("PlaceBoard/PlaceObject-Place");

                    _placeBoardPresenter.Add(
                        selectedOperationPlaceObject.TargetPlaceObjectView,
                        gridCellView.transform.position);

                    Place(originPosition, _nextPlaceObjectUseCase.Get(selectedOperationPlaceObject.HolderIndex));

                    var holderIndex = selectedOperationPlaceObject.HolderIndex;
                    // 今回使った配置可能オブジェクトを削除する
                    _nextPlaceObjectUseCase.Submit(holderIndex);
                    // すべてを再抽選する
                    _nextPlaceObjectUseCase.SetAll();

                    // パッキングボタンの表示判定
                    if (!_placeableSpaceCalculator.CalculatePlaceable() && !_placeUseCase.IsFullPacking())
                    {
                        _inGameUIView.ShowPackingButton();
                        _inGameUserState.SetState(InGameUserStateType.Packable);
                    }
                }
                else
                {
                    _gridPresenter.Focus(canPlace, relativePositions.Select(i => originPosition + i).ToArray());
                }
            }

            // キャンセル処理
            if (Input.GetMouseButtonDown(1))
            {
                _selectPlaceObjectUseCase.Cancel();
                return;
            }

            if (!_selectPlaceObjectUseCase.IsSelected) return;

            var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;
            var operationPlaceObject = _selectPlaceObjectUseCase.SelectedPlaceObject.Value;
            operationPlaceObject.TargetPlaceObjectView.transform.position = position;
        }

        private void Place(Vector2Int originPosition, PlaceObject placeObject)
        {
            _placeUseCase.Place(originPosition, placeObject);
            _selectPlaceObjectUseCase.Submit();
            _inGameUserState.SetState(InGameUserStateType.Ready);
        }

        private bool TryGetPlaceObjectView(Vector3 screenPosition, out GridCellView result)
        {
            var position = Camera.main.ScreenToWorldPoint(screenPosition);

            var hitCount = Physics2D.RaycastNonAlloc(position, Vector2.zero, _hitBuffer);
            if (hitCount == 0)
            {
                result = null;
                return false;
            }

            for (var i = 0; i < hitCount; i++)
            {
                if (_hitBuffer[i].collider.TryGetComponent(out GridCellView placeObjectView))
                {
                    result = placeObjectView;
                    return true;
                }
            }

            result = null;
            return false;
        }
    }
}