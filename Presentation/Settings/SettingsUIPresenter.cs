using Cysharp.Threading.Tasks;
using Kiro.Application;
using R3;
using VContainer.Unity;

namespace Kiro.Presentation
{
    public sealed class SettingsUIPresenter : ControllerBase, IInitializable
    {
        readonly SettingScreenUIView _settingScreenUIView;
        readonly GameSceneLoader _gameSceneLoader;
        readonly AudioSliderUIView _audioSliderUIView;
        readonly AudioVolumeModel _audioVolumeModel;
        readonly UserSettingRepository _userSettingRepository;

        public SettingsUIPresenter(
            SettingScreenUIView settingScreenUIView, GameSceneLoader gameSceneLoader,
            AudioSliderUIView audioSliderUIView, AudioVolumeModel audioVolumeModel,
            UserSettingRepository userSettingRepository
        )
        {
            _settingScreenUIView = settingScreenUIView;
            _gameSceneLoader = gameSceneLoader;
            _audioSliderUIView = audioSliderUIView;
            _audioVolumeModel = audioVolumeModel;
            _userSettingRepository = userSettingRepository;
        }

        public void Initialize()
        {
            var bgm = _userSettingRepository.GetBgmVolume();
            var sfx = _userSettingRepository.GetSfxVolume();

            _audioSliderUIView.SetBgmVolume(bgm);
            _audioSliderUIView.SetSfxVolume(sfx);

            ViewToModel();
            ModelToView();
        }

        void ViewToModel()
        {
            _settingScreenUIView
                .ButtonClickedAsObservable(ButtonType.Back)
                .Subscribe(_ => _gameSceneLoader.UnloadSettingsAsync().Forget())
                .AddTo(this);

            _audioSliderUIView
                .BgmVolumeAsObservable
                .Subscribe(
                    bgmVolume => { _audioVolumeModel.SetBgmVolume(bgmVolume); }
                )
                .AddTo(this);

            _audioSliderUIView
                .SfxVolumeAsObservable
                .Subscribe(
                    sfxVolume => { _audioVolumeModel.SetSfxVolume(sfxVolume); }
                )
                .AddTo(this);
        }

        void ModelToView()
        {
            _audioVolumeModel
                .BgmVolumeReactiveProperty
                .Subscribe(
                    bgmVolume =>
                    {
                        _audioSliderUIView.SetBgmVolume(bgmVolume);
                        _userSettingRepository.SetBgmVolume(bgmVolume);
                    }
                )
                .AddTo(this);

            _audioVolumeModel
                .SfxVolumeReactiveProperty
                .Subscribe(
                    sfxVolume =>
                    {
                        _audioSliderUIView.SetSfxVolume(sfxVolume);
                        _userSettingRepository.SetSfxVolume(sfxVolume);
                    }
                )
                .AddTo(this);
        }
    }
}