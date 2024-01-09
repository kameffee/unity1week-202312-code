using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity1week202312.PlaceBoard;
using UnityEngine;

namespace Unity1week202312.PlaceObjects
{
    public class PlaceableSpaceCalculator
    {
        private readonly PlaceBoardService _placeBoardService;
        private readonly PlaceableObjectContainer _placeableObjectContainer;
        private readonly PlaceUseCase _placeUseCase;

        public PlaceableSpaceCalculator(
            PlaceBoardService placeBoardService,
            PlaceableObjectContainer placeableObjectContainer,
            PlaceUseCase placeUseCase)
        {
            _placeBoardService = placeBoardService;
            _placeableObjectContainer = placeableObjectContainer;
            _placeUseCase = placeUseCase;
        }

        /// <summary>
        /// 現在の配置可能マップと配置済みオブジェクトから配置可能かどうかを計算
        /// </summary>
        /// <returns></returns>
        public bool CalculatePlaceable()
        {
            var placeablePositionsMap = new Dictionary<PlaceObject, Vector2Int[]>();

            var placeableMap = _placeBoardService.GetMap();
            var emptyPositions = placeableMap.GetEmptyPositions().ToArray();
            var placeObjects = _placeableObjectContainer.GetAll();
            foreach (var placeObject in placeObjects)
            {
                var placeablePositions = CalculatePlaceablePositions(placeObject, emptyPositions);
                placeablePositionsMap.Add(placeObject, placeablePositions);
            }

            // log
            StringBuilder sb = new();
            sb.AppendLine("PlaceableSpaceCalculator.CalculatePlaceable");
            foreach (var placeObject in placeablePositionsMap.Keys)
            {
                sb.AppendLine($"Id:{placeObject.Id}: {string.Join(",",placeablePositionsMap[placeObject].Select(i => i.ToString()))}");
            }

            Debug.Log(sb.ToString());

            return placeablePositionsMap.Any(x => x.Value.Length > 0);
        }

        private Vector2Int[] CalculatePlaceablePositions(PlaceObject placeObject, Vector2Int[] emptyPositions)
        {
            var placeablePositions = new List<Vector2Int>();
            foreach (var emptyPosition in emptyPositions)
            {
                var canPlace = _placeUseCase.CanPlace(emptyPosition, placeObject);
                if (canPlace)
                {
                    placeablePositions.Add(emptyPosition);
                }
            }

            return placeablePositions.ToArray();
        }
    }
}