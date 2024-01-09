using UniRx;

namespace Unity1week202312.InGame
{
    public class InGameUserState
    {
        public IReadOnlyReactiveProperty<InGameUserStateType> State => _state;
        private readonly ReactiveProperty<InGameUserStateType> _state = new(InGameUserStateType.Ready);
        
        public void SetState(InGameUserStateType state)
        {
            _state.Value = state;
        }
    }
}