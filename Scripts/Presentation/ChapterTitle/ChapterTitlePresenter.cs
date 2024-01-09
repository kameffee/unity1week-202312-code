using System.Threading;
using Cysharp.Threading.Tasks;
using Kameffee.Extensions;
using Unity1week202312.Stage;

namespace Unity1week202312.ChapterTitle
{
    public class ChapterTitlePresenter : Presenter, IChapterTitlePerformer
    {
        private readonly ChapterTitleView _view;
        private readonly ChapterTitleContentRepository _contentRepository;

        public ChapterTitlePresenter(ChapterTitleView view, ChapterTitleContentRepository contentRepository)
        {
            _view = view;
            _contentRepository = contentRepository;
        }

        public async UniTask PlayAsync(StageId stageId, CancellationToken cancellationToken = default)
        {
            var content = _contentRepository.Get(stageId.Value);
            _view.ApplyViewModel(new ChapterTitleView.ViewModel(content.Title, content.Content));
            await _view.PlayAsync(cancellationToken);
        }
    }
}