using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Unity1week202312.PlaceObjects
{
    /// <summary>
    /// 次に配置可能オブジェクトを保持
    /// </summary>
    public class PlaceableObjectContainer
    {
        public int Size { get; } = 4;
        
        private readonly Dictionary<int, PlaceObject> _placeObjects = new();
        private readonly Subject<(int holderIndex, PlaceObject placeObject)> _onSetSubject = new();
        private readonly Subject<(int HolderIndex, PlaceObject PlaceObject)> _onRemoveSubject = new();

        public void Set(int index, PlaceObject placeObject)
        {
            if (_placeObjects.TryAdd(index, placeObject))
            {
                _onSetSubject.OnNext(ValueTuple.Create(index, placeObject));
            }
        }

        public PlaceObject Get(int index) => _placeObjects[index];
        
        public bool TryGet(int index, out PlaceObject placeObject) => _placeObjects.TryGetValue(index, out placeObject);

        public PlaceObject[] GetAll() => _placeObjects.Values.ToArray();

        public bool Remove(int index)
        {
            if (_placeObjects.Remove(index, out var placeObject))
            {
                _onRemoveSubject.OnNext(ValueTuple.Create(index, placeObject));
                return true;
            }

            return false;
        }

        public void Clear()
        {
            foreach ((int key, PlaceObject _) in _placeObjects.ToArray())
            {
                if (_placeObjects.Remove(key, out var placeObject))
                {
                    _onRemoveSubject.OnNext(ValueTuple.Create(key, placeObject));
                }
            }

            _placeObjects.Clear();
        }

        public IObservable<(int holderIndex, PlaceObject placeObject)> OnSetAsObservable() => _onSetSubject;
        public IObservable<(int HolderIndex, PlaceObject PlaceObject)> OnRemoveAsObservable() => _onRemoveSubject;
    }
}