using System.Collections.Generic;
using Unity1week202312.PlaceObjects;
using Unity1week202312.Stage;

namespace Unity1week202312.PlaceBoard.History
{
    public class PlaceBoardRecord
    {
        public StageId StageId { get; }
        public PlaceableMap PlaceableMap { get; }
        public PackedPlaceObject[] PackedPlaceObjects { get; }

        public PlaceBoardRecord(
            StageId stageId,
            PlaceableMap placeableMap,
            PackedPlaceObject[] packedPlaceObjects)
        {
            StageId = stageId;
            PlaceableMap = placeableMap;
            PackedPlaceObjects = packedPlaceObjects;
        }

        public bool Exists(PlaceObjectId placeObjectId)
        {
            foreach (var packedPlaceObject in PackedPlaceObjects)
            {
                if (packedPlaceObject.Id == placeObjectId)
                {
                    return true;
                }
            }

            return false;
        }
    }
}