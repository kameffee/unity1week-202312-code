using UnityEngine;

namespace Unity1week202312.Cell
{
    public readonly struct CellArea
    {
        public Vector2Int Size { get; }
        public int Count => Size.x * Size.y;
        public bool[] Map { get; }

        public CellArea(Vector2Int size, bool[] map)
        {
            if (size.x * size.y != map.Length)
            {
                throw new System.ArgumentException($"sizeとmapの要素数が一致しません.\n{nameof(size)}:{size.x}x{size.y}, {nameof(map)}:{map.Length}");
            }

            Size = size;
            Map = map;
        }
    }
}