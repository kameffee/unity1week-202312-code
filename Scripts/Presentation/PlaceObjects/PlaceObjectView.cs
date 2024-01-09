using UnityEngine;

namespace Unity1week202312.PlaceObjects
{
    public class PlaceObjectView : MonoBehaviour
    {
        public PlaceObjectId Id => _viewModel.Id;
        public Vector2Int Size => _cellSizeMasterData.Size;
        public CellSizeMasterData CellSizeMasterData => _cellSizeMasterData;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private Color _normalColor = Color.white;

        [SerializeField]
        private Color _selectedColor = new Color(1f, 0.5f, 0.5f, 1f);

        [SerializeField]
        private CellSizeMasterData _cellSizeMasterData;

        private ViewModel _viewModel;

        public void ApplyViewModel(ViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void SetSelected(bool isSelected)
        {
            var toColor = isSelected ? _selectedColor : _normalColor;
            _spriteRenderer.color = toColor;
        }

        public void SetMiniScale()
        {
            // 長辺を1にする
            var maxLength = Mathf.Max(_cellSizeMasterData.Size.x, _cellSizeMasterData.Size.y);
            var scale = 1f / maxLength;
            transform.localScale = Vector3.one * scale;
        }

        public void SetDefaultScale()
        {
            transform.localScale = Vector3.one;
        }

        private void OnDrawGizmos()
        {
            if (_cellSizeMasterData == null) return;

            var spacing = Vector2.one * 0.1f;
            var cellSize = _cellSizeMasterData.Size;
            var cellScale = Vector2Int.one * 2;
            for (int y = 0; y < cellSize.y; y++)
            {
                for (int x = 0; x < cellSize.x; x++)
                {
                    Gizmos.color = _cellSizeMasterData.IsExist(x, y) ? Color.green : Color.gray;
                    var position = new Vector3(
                        (cellScale.x + spacing.x) * x * transform.localScale.x,
                        (cellScale.y + spacing.y) * -y * transform.localScale.y,
                        0);
                    var size = new Vector3(2, 2, 0) * transform.localScale.x;
                    Gizmos.DrawWireCube(transform.position + position, size);
                }
            }
        }

        public class ViewModel
        {
            public PlaceObjectId Id { get; }

            public ViewModel(PlaceObjectId id)
            {
                Id = id;
            }
        }
    }
}