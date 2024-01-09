using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kameffee.Extensions;
using UniRx;
using Unity1week202312.PlaceObjects;
using VContainer.Unity;

namespace Unity1week202312.InGame
{
    public class GuidancePresenter : Presenter, IInitializable, IGuidancePerformer
    {
        private readonly GuidanceView _view;
        private readonly InGameUserState _inGameUserState;

        public GuidancePresenter(GuidanceView view, InGameUserState inGameUserState)
        {
            _view = view;
            _inGameUserState = inGameUserState;
        }

        public void Initialize()
        {
            _inGameUserState.State
                .Subscribe(type => ChangeState(type))
                .AddTo(this);
        }
        
        private void ChangeState(InGameUserStateType stateType)
        {
            switch (stateType)
            {
                case InGameUserStateType.Ready:
                    _view.Set(GuidanceView.StateType.NotSelected);
                    break;
                case InGameUserStateType.Grabbing:
                    _view.Set(GuidanceView.StateType.Selected);
                    break;
                case InGameUserStateType.Packable:
                    _view.Set(GuidanceView.StateType.NotPlaceable);
                    break;
                case InGameUserStateType.Packing:
                    _view.Set(GuidanceView.StateType.None);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stateType), stateType, null);
            }
        }

        public async UniTask ShowAsync(GuidanceView.StateType stateType, CancellationToken cancellationToken = default)
        {
            await _view.ShowAsync(stateType, cancellationToken);
        }

        public async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            await _view.HideAsync(cancellationToken);
        }
    }
}