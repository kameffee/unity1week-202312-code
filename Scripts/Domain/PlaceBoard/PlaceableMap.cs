using System.Collections.Generic;
using UnityEngine;

namespace Unity1week202312.PlaceBoard
{
    public class PlaceableMap
    {
        public Vector2Int Size { get; }

        private readonly bool[] _placeMap;

        public PlaceableMap(Vector2Int size)
        {
            Size = size;
            _placeMap = new bool[size.x * size.y];
        }

        public bool IsFull()
        {
            foreach (var placed in _placeMap)
            {
                if (!placed)
                {
                    return false;
                }
            }

            return true;
        }

        public bool CanPlace(params Vector2Int[] positions)
        {
            // すべての座標が配置可能か
            foreach (var vector2Int in positions)
            {
                if (!CanPlace(vector2Int))
                {
                    return false;
                }
            }

            return true;
        }

        public bool CanPlace(Vector2Int position)
        {
            // 範囲外チェック
            if (!Valid(position))
            {
                return false;
            }

            var index = position.y * Size.x + position.x;
            return !_placeMap[index];
        }

        public void Place(params Vector2Int[] positions)
        {
            foreach (var position in positions)
            {
                var index = position.y * Size.x + position.x;
                _placeMap[index] = true;
            }

            Log();
        }

        public void Remove(params Vector2Int[] positions)
        {
            foreach (var position in positions)
            {
                var index = position.y * Size.x + position.x;
                _placeMap[index] = false;
            }

            Log();
        }

        /// <summary>
        /// 空いている座標を取得
        /// </summary>
        public IEnumerable<Vector2Int> GetEmptyPositions()
        {
            for (var y = 0; y < Size.y; y++)
            {
                for (var x = 0; x < Size.x; x++)
                {
                    var index = y * Size.x + x;
                    if (!_placeMap[index])
                    {
                        yield return new Vector2Int(x, y);
                    }
                }
            }
        }

        private void Log()
        {
            Debug.Log(ToString());
        }

        public bool Valid(Vector2Int gridPosition)
        {
            // 範囲外チェック
            if (gridPosition.x < 0 || gridPosition.x >= Size.x || gridPosition.y < 0 || gridPosition.y >= Size.y)
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            var str = "";
            for (var y = 0; y < Size.y; y++)
            {
                for (var x = 0; x < Size.x; x++)
                {
                    var index = y * Size.x + x;
                    str += _placeMap[index] ? "■" : "□";
                }

                str += "\n";
            }

            return str;
        }
    }
}