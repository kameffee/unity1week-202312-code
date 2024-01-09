using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kameffee.Audio;
using Kameffee.Extensions;
using Kameffee.License;
using Kameffee.Scenes;
using UniRx;
using VContainer.Unity;

namespace Unity1week202312.Title
{
    public class TitleEntryPoint : Presenter, IInitializable, IStartable
    {
        private readonly TitleCanvasView _canvasView;
        private readonly LicensePresenter _licensePresenter;
        private readonly SceneLoader _sceneLoader;
        private readonly AudioPlayer _audioPlayer;
        private readonly StartPerformer _startPerformer;

        public TitleEntryPoint(
            TitleCanvasView canvasView,
            LicensePresenter licensePresenter,
            SceneLoader sceneLoader,
            AudioPlayer audioPlayer,
            StartPerformer startPerformer)
        {
            _canvasView = canvasView;
            _licensePresenter = licensePresenter;
            _sceneLoader = sceneLoader;
            _audioPlayer = audioPlayer;
            _startPerformer = startPerformer;
        }

        void IInitializable.Initialize()
        {
            _canvasView.OnClickStartAsObservable()
                .Take(1)
                .SelectMany(_ => StartGame().ToObservable())
                .Subscribe()
                .AddTo(this);

            _canvasView.OnClickLicenseAsObservable()
                .Subscribe(_ => _licensePresenter.ShowAsync().Forget())
                .AddTo(this);
        }

        void IStartable.Start()
        {
            _audioPlayer.PlayBgm("Main");
        }

        private async UniTask StartGame(CancellationToken cancellationToken = default)
        {
            _canvasView.SetAvailable(false);

            await _startPerformer.PlayAsync(cancellationToken);

            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellationToken);

            await _sceneLoader.LoadAsync(SceneDefine.InGame, cancellationToken: cancellationToken);
        }
    }
}