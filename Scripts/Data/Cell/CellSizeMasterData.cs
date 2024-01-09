using Unity1week202312.Cell;
using UnityEngine;

namespace Unity1week202312
{
    [CreateAssetMenu(fileName = "CellSizeMasterData", menuName = "CellSizeMasterData")]
    public class CellSizeMasterData : ScriptableObject
    {
        public Vector2Int Size => _size;

        [SerializeField]
        private Vector2Int _size;

        [SerializeField]
        private Vector2Int _pivot;

        [SerializeField]
        private bool[] _cellSizeMap = new bool[1];

        public CellArea GetCellArea()
        {
            return new CellArea(_size, _cellSizeMap);
        }

        public bool IsExist(int x, int y)
        {
            var index = y * _size.x + x;
            return _cellSizeMap[index];
        }

        /// <summary>
        ///  Pivotを考慮した座標の配列を返す
        /// </summary>
        public Vector2Int[] GetCellPositions()
        {
            var positions = new Vector2Int[_cellSizeMap.Length];
            for (int y = 0; y < _size.y; y++)
            {
                for (int x = 0; x < _size.x; x++)
                {
                    var index = y * _size.x + x;

                    // セルが無効なら無視
                    if (!_cellSizeMap[index])
                        continue;

                    // Pivotを考慮した座標に変換
                    var position = new Vector2Int(x, y) - _pivot;
                    positions[index] = position;
                }
            }

            return positions;
        }
    }
}