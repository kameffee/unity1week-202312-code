using System.Threading;
using Cysharp.Threading.Tasks;
using Kameffee.Extensions;
using Kameffee.Scenes;
using UniRx;
using VContainer.Unity;

namespace Unity1week202312.Result
{
    public class ResultEntryPoint : Presenter, IInitializable, IAsyncStartable
    {
        private readonly ResultCanvasView _view;
        private readonly SceneDataContainer _sceneDataContainer;
        private readonly SceneLoader _sceneLoader;
        private readonly CreateResultContentViewModelUseCase _createResultContentViewModelUseCase;

        public ResultEntryPoint(
            ResultCanvasView view,
            SceneDataContainer sceneDataContainer,
            SceneLoader sceneLoader,
            CreateResultContentViewModelUseCase createResultContentViewModelUseCase)
        {
            _view = view;
            _sceneDataContainer = sceneDataContainer;
            _sceneLoader = sceneLoader;
            _createResultContentViewModelUseCase = createResultContentViewModelUseCase;
        }

        void IInitializable.Initialize()
        {
            _view.OnClickGoToTitleButtonAsObservable()
                .Subscribe(_ => _sceneLoader.LoadAsync(SceneDefine.Title).Forget())
                .AddTo(this);
        }

        public UniTask StartAsync(CancellationToken cancellation)
        {
            var sceneData = _sceneDataContainer.Get<ResultSceneData>();
            var viewModel = _createResultContentViewModelUseCase.Create(sceneData.ResultType);
            _view.ApplyViewModel(viewModel);

            _sceneDataContainer.Remove<ResultSceneData>();

            return UniTask.CompletedTask;
        }
    }
}