using UnityEngine;

namespace Unity1week202312.GridCell
{
    public class GridCellView : MonoBehaviour
    {
        public Vector2Int Position { get; private set; }

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private Color _placeableColor;
        
        [SerializeField]
        private Color _notPlaceableColor;
        
        private Color _defaultColor;
        private bool _isSelected;
        private bool _isFocused;

        private void Awake()
        {
            _defaultColor = _spriteRenderer.color;
        }

        public void ApplyViewModel(ViewModel viewModel)
        {
            Position = viewModel.GridPosition;
        }

        public void SetSelected(bool isSelected)
        {
            _isSelected = isSelected;
            _spriteRenderer.color = isSelected ? Color.clear : Color.white;
        }

        public void Focus(bool  isPlaceable)
        {
            _isFocused = true;
            if (_isSelected) return;
            _spriteRenderer.color = isPlaceable ? _placeableColor : _notPlaceableColor;
        }

        public void Unfocus()
        {
            _isFocused = false;
            if (_isSelected) return;
            _spriteRenderer.color = _defaultColor;
        }

        public class ViewModel
        {
            public int Id { get; }
            public Vector2Int GridPosition { get; }

            public ViewModel(int id, Vector2Int gridPosition)
            {
                Id = id;
                GridPosition = gridPosition;
            }
        }
    }
}