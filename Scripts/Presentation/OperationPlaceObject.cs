using Unity1week202312.PlaceObjects;

namespace Unity1week202312
{
    /// <summary>
    /// 現在操作中の配置オブジェクト
    /// </summary>
    public class OperationPlaceObject
    {
        public int HolderIndex => _holderIndex;
        public PlaceObjectView TargetPlaceObjectView => _targetPlaceObjectView;

        private readonly int _holderIndex;
        private readonly PlaceObjectView _targetPlaceObjectView;

        public OperationPlaceObject(int holderIndex, PlaceObjectView targetPlaceObjectView)
        {
            _holderIndex = holderIndex;
            _targetPlaceObjectView = targetPlaceObjectView;
        }
    }
}