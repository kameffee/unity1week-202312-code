namespace Unity1week202312.Result
{
    using ViewModel = ResultCanvasView.ViewModel;

    public class CreateResultContentViewModelUseCase
    {
        private readonly ResultContentRepository _resultContentRepository;

        public CreateResultContentViewModelUseCase(ResultContentRepository resultContentRepository)
        {
            _resultContentRepository = resultContentRepository;
        }

        public ViewModel Create(ResultType type)
        {
            var resultContent = _resultContentRepository.Get(type);
            return new ViewModel(resultContent.Content);
        }
    }
}