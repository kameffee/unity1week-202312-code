using System.Threading;
using Cysharp.Threading.Tasks;
using Unity1week202312.Stage;

namespace Unity1week202312.ChapterTitle
{
    public interface IChapterTitlePerformer
    {
        UniTask PlayAsync(StageId stageId, CancellationToken cancellationToken = default);
    }
}