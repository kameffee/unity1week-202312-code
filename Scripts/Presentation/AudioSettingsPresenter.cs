using Kameffee.Audio;
using Kameffee.Extensions;
using UniRx;
using VContainer.Unity;

namespace Unity1week202312
{
    public class AudioSettingsPresenter : Presenter, IInitializable
    {
        private readonly AudioSettingView _view;
        private readonly AudioSettingsService _audioSettingsService;
        private readonly AudioPlayer _audioPlayer;
        private readonly CreateAudioSettingViewModelUseCase _createAudioSettingViewModelUseCase;

        public AudioSettingsPresenter(
            AudioSettingView view,
            AudioSettingsService audioSettingsService,
            AudioPlayer audioPlayer,
            CreateAudioSettingViewModelUseCase createAudioSettingViewModelUseCase)
        {
            _view = view;
            _audioSettingsService = audioSettingsService;
            _audioPlayer = audioPlayer;
            _createAudioSettingViewModelUseCase = createAudioSettingViewModelUseCase;
        }

        public void Initialize()
        {
            var viewModel = _createAudioSettingViewModelUseCase.Create();
            _view.ApplyViewModel(viewModel);

            Subscribe();
        }

        private void Subscribe()
        {
            _view.OnChangeBgmVolumeAsObservable()
                .Subscribe(volume => _audioSettingsService.SetBgmVolume(volume))
                .AddTo(this);

            _view.OnChangeSeVolumeAsObservable()
                .Subscribe(volume => _audioSettingsService.SetSeVolume(volume))
                .AddTo(this);

            _view.OnPointerUpSeVolumeAsObservable()
                .Subscribe(_ => _audioPlayer.PlaySe("AudioSettings/OnChangeSeVolume"))
                .AddTo(this);

            _audioSettingsService.BgmVolume
                .Subscribe(volume => _view.SetBgmVolume(volume))
                .AddTo(this);

            _audioSettingsService.SeVolume
                .Subscribe(volume => _view.SetSfxVolume(volume))
                .AddTo(this);
        }
    }
}