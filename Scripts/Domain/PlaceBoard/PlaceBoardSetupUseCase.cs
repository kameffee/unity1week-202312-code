using UnityEngine;

namespace Unity1week202312.PlaceBoard
{
    public class PlaceBoardSetupUseCase
    {
        private readonly PlaceBoardFactory _placeBoardFactory;
        private readonly PlaceBoardService _placeBoardService;
        
        public PlaceBoardSetupUseCase(
            PlaceBoardFactory placeBoardFactory,
            PlaceBoardService placeBoardService)
        {
            _placeBoardFactory = placeBoardFactory;
            _placeBoardService = placeBoardService;
        }
        
        public void Setup()
        {
            var placeBoard = _placeBoardFactory.Create(new Vector2Int(4, 4));
            _placeBoardService.SetMap(placeBoard);
        }
    }
}