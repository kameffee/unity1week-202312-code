using System.Threading;
using Cysharp.Threading.Tasks;

namespace Unity1week202312.InGame
{
    public interface IGuidancePerformer
    {
        UniTask ShowAsync(GuidanceView.StateType stateType, CancellationToken cancellationToken = default);
        UniTask HideAsync(CancellationToken cancellationToken = default);
    }
}