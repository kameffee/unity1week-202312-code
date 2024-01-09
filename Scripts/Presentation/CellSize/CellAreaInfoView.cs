using System;
using System.Collections.Generic;
using Unity1week202312.Cell;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202312.CellSize
{
    public class CellAreaInfoView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private RectTransform _root;

        [SerializeField]
        private GridLayoutGroup _gridLayoutGroup;

        [SerializeField]
        private GameObject _cellPrefab;

        private readonly List<GameObject> _cells = new();

        private void Awake()
        {
            Hide();
        }

        public void ApplyViewModel(ViewModel viewModel)
        {
            Clear();

            _gridLayoutGroup.constraintCount = viewModel.CellSize.x;
            for (var index = 0; index < viewModel.Count; index++)
            {
                var cell = CreateCell();
                if (!viewModel.PlaceableMap[index])
                {
                    var image = cell.GetComponent<Image>();
                    image.color = Color.clear;
                }

                _cells.Add(cell);
            }
        }

        public void Show()
        {
            _canvasGroup.alpha = 1f;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0f;
        }

        private GameObject CreateCell()
        {
            return Instantiate(_cellPrefab, _root);
        }

        private void Clear()
        {
            foreach (var cell in _cells)
            {
                Destroy(cell);
            }

            _cells.Clear();
        }

        public class ViewModel
        {
            public Vector2Int CellSize { get; }
            public int Count { get; }
            public bool[] PlaceableMap { get; }

            public ViewModel(CellArea cellArea)
            {
                CellSize = cellArea.Size;
                Count = cellArea.Count;
                PlaceableMap = cellArea.Map;
            }
        }
    }
}