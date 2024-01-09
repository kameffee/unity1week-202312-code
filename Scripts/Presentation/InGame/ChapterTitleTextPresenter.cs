using System.Threading;
using Cysharp.Threading.Tasks;
using Kameffee.Extensions;
using Unity1week202312.Stage;

namespace Unity1week202312.InGame
{
    public class ChapterTitleTextPresenter : Presenter
    {
        private readonly ChapterTitleTextView _view;
        private readonly ChapterTitleContentRepository _contentRepository;

        public ChapterTitleTextPresenter(
            ChapterTitleTextView view,
            ChapterTitleContentRepository contentRepository)
        {
            _view = view;
            _contentRepository = contentRepository;
        }

        public async UniTask ShowAsync(StageId stageId, CancellationToken cancellationToken = default)
        {
            var content = _contentRepository.Get(stageId.Value);
            _view.SetChapterText(content.Title);

            await _view.ShowAsync(cancellationToken);
        }

        public async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            await _view.HideAsync(cancellationToken);
        }
    }
}