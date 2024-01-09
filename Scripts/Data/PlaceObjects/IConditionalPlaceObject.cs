using System.Collections.Generic;

namespace Unity1week202312.PlaceObjects
{
    public interface IConditionalPlaceObject
    {
        PlaceObjectId Id { get; }

        IEnumerable<PlaceObjectMasterData> ConditionalTargets { get; }
    }
}