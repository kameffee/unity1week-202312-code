using Unity1week202312.Stage;
using Unity1week202312.PlaceBoard;
using Unity1week202312.PlaceObjects;
using UnityEngine;

namespace Unity1week202312
{
    public class InGameSetupUseCase
    {
        private readonly StageService _stageService;
        private readonly PlaceBoardService _placeBoardService;
        private readonly PlaceBoardFactory _placeBoardFactory;
        private readonly PlaceableObjectContainer _placeableObjectContainer;
        private readonly NextPlaceObjectUseCase _nextPlaceObjectUseCase;

        public InGameSetupUseCase(
            StageService stageService,
            PlaceBoardService placeBoardService,
            PlaceBoardFactory placeBoardFactory,
            PlaceableObjectContainer placeableObjectContainer,
            NextPlaceObjectUseCase nextPlaceObjectUseCase)
        {
            _stageService = stageService;
            _placeBoardService = placeBoardService;
            _placeBoardFactory = placeBoardFactory;
            _placeableObjectContainer = placeableObjectContainer;
            _nextPlaceObjectUseCase = nextPlaceObjectUseCase;
        }

        public void Setup(StageId stageId)
        {
            _stageService.SetCurrentStageId(stageId);

            _placeBoardService.ClearPlacedObjects();

            var boardSize = new Vector2Int(4, 4);
            var placeBoard = _placeBoardFactory.Create(boardSize);
            _placeBoardService.SetMap(placeBoard);

            _placeableObjectContainer.Clear();
            _nextPlaceObjectUseCase.Reset();
        }

        public void NextStage()
        {
            Setup(_stageService.CurrentStageId.Next());
        }
    }
}