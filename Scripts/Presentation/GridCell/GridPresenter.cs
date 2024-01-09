using System.Collections.Generic;
using Kameffee.Extensions;
using UniRx;
using Unity1week202312.PlaceBoard;
using UnityEngine;
using VContainer.Unity;

namespace Unity1week202312.GridCell
{
    public class GridPresenter : Presenter, IInitializable
    {
        private readonly GridCellViewFactory _gridCellViewFactory;
        private readonly PlaceBoardService _placeBoardService;
        private readonly PlaceUseCase _placeUseCase;

        private readonly List<GridCellView> _gridCellViews = new();

        public GridPresenter(
            GridCellViewFactory gridCellViewFactory,
            PlaceBoardService placeBoardService,
            PlaceUseCase placeUseCase)
        {
            _gridCellViewFactory = gridCellViewFactory;
            _placeBoardService = placeBoardService;
            _placeUseCase = placeUseCase;
        }

        void IInitializable.Initialize()
        {
            _placeUseCase.OnChangePlaceStatusAsObservable()
                .Subscribe(placeStatus => ChangeStatus(placeStatus))
                .AddTo(this);

            _placeBoardService.CurrentPlaceableMap
                .Where(map => map != null)
                .Subscribe(placeableMap => Create(placeableMap))
                .AddTo(this);
        }

        private void Create(PlaceableMap placeableMap)
        {
            Clear();
            Create(placeableMap.Size, new Vector2(2, 2), interval: Vector2.one * 1.1f);
        }

        private void Clear()
        {
            foreach (var gridCellView in _gridCellViews)
            {
                Object.Destroy(gridCellView.gameObject);
            }

            _gridCellViews.Clear();
        }

        private void ChangeStatus(PlaceStatus[] placeStatuses)
        {
            foreach (var placeStatus in placeStatuses)
            {
                if (!Valid(placeStatus.Position)) continue;

                var gridCellView = GetCellView(placeStatus.Position);
                gridCellView.SetSelected(placeStatus.IsPlaced);
            }
        }

        private bool Valid(Vector2Int gridPosition)
        {
            var placeBoard = _placeBoardService.GetMap();
            return placeBoard.Valid(gridPosition);
        }

        private GridCellView GetCellView(Vector2Int gridPosition)
        {
            var placeBoard = _placeBoardService.GetMap();

            return _gridCellViews[gridPosition.y * placeBoard.Size.x + gridPosition.x];
        }

        private void Create(Vector2Int gridSize, Vector2 cellSize, Vector2 offset = default, Vector2 interval = default)
        {
            for (var y = 0; y < gridSize.y; y++)
            for (var x = 0; x < gridSize.x; x++)
            {
                var gridCellView = _gridCellViewFactory.Create();
                var position = new Vector2(x, -y);
                var hafCellSize = cellSize * 0.5f;
                gridCellView.transform.localPosition = position + new Vector2(hafCellSize.x, -hafCellSize.y) +
                                                       position * interval + offset;
                gridCellView.gameObject.name = $"GridCellView ({position.x}, {position.y})";

                var id = y * gridSize.x + x;
                var viewModel = new GridCellView.ViewModel(id, new Vector2Int(x, y));
                gridCellView.ApplyViewModel(viewModel);
                _gridCellViews.Add(gridCellView);
            }
        }

        public void Focus(bool placeable, params Vector2Int[] positions)
        {
            foreach (var position in positions)
            {
                if (!Valid(position)) continue;

                var gridCellView = GetCellView(position);
                gridCellView.Focus(placeable);
            }
        }

        public void UnFocusAll()
        {
            foreach (var gridCellView in _gridCellViews)
            {
                gridCellView.Unfocus();
            }
        }
    }
}