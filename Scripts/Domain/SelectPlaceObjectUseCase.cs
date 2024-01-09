using System;
using UniRx;
using Unity1week202312.PlaceObjects;
using Object = UnityEngine.Object;

namespace Unity1week202312
{
    /// <summary>
    /// 配置オブジェクトの選択
    /// </summary>
    public class SelectPlaceObjectUseCase
    {
        public bool IsSelected => _selectedPlaceObject.Value != null;
        public int HolderIndex => _selectedPlaceObject.Value.HolderIndex;
        public IReadOnlyReactiveProperty<OperationPlaceObject> SelectedPlaceObject => _selectedPlaceObject;

        private readonly ReactiveProperty<OperationPlaceObject> _selectedPlaceObject = new(null);
        private readonly Subject<int> _onSubmit = new();
        private readonly Subject<OperationPlaceObject> _onCancel = new();

        public void Select(int holderIndex, PlaceObjectView placeObjectView)
        {
            _selectedPlaceObject.Value = new OperationPlaceObject(holderIndex, placeObjectView);
        }

        public void Cancel()
        {
            _onCancel.OnNext(_selectedPlaceObject.Value);
            _selectedPlaceObject.Value = null;
        }

        public void Submit()
        {
            var holderIndex = _selectedPlaceObject.Value.HolderIndex;
            Object.Destroy(_selectedPlaceObject.Value.TargetPlaceObjectView.gameObject);
            _selectedPlaceObject.Value = null;

            _onSubmit.OnNext(holderIndex);
        }

        public IObservable<int> OnSubmitAsObservable() => _onSubmit;

        public IObservable<OperationPlaceObject> OnCancelAsObservable() => _onCancel;
    }
}