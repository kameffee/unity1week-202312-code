using UnityEngine;

namespace Unity1week202312.GridCell
{
    public class GridCellViewFactory
    {
        private readonly GridCellView _prefab;
        private readonly Transform _parent;

        public GridCellViewFactory(GridCellView prefab, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
        }

        public GridCellView Create()
        {
            var instance = Object.Instantiate(_prefab, _parent);
            return instance;
        }
    }
}