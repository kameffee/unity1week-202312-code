using Kameffee.Extensions;
using VContainer.Unity;
using UniRx;

namespace Unity1week202312
{
    public class InGameUIPresenter : Presenter, IInitializable
    {
        private readonly PlaceUseCase _placeUseCase;
        private readonly InGameUIView _view;
        
        public InGameUIPresenter(InGameUIView view)
        {
            _view = view;
        }

        void IInitializable.Initialize()
        {
            _placeUseCase.OnFullPackingAsObservable()
                .Subscribe(_ => _view.ShowPackingButton())
                .AddTo(this);
        }
    }
}