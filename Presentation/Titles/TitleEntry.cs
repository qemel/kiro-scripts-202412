using AnnulusGames.LucidTools.Audio;
using Kiro.Application;
using UnityEngine;
using VContainer.Unity;

namespace Kiro.Presentation
{
    public sealed class TitleEntry : IInitializable
    {
        readonly AudioVolumeModel _audioVolumeModel;
        readonly UserSettingRepository _userSettingRepository;
        readonly AudioClip _bgm;

        public TitleEntry(AudioVolumeModel audioVolumeModel, UserSettingRepository userSettingRepository, AudioClip bgm)
        {
            _audioVolumeModel = audioVolumeModel;
            _userSettingRepository = userSettingRepository;
            _bgm = bgm;
        }

        public void Initialize()
        {
            _audioVolumeModel.SetBgmVolume(_userSettingRepository.GetBgmVolume());
            _audioVolumeModel.SetSfxVolume(_userSettingRepository.GetSfxVolume());

            LucidAudio.PlayBGM(_bgm).SetLoop();
        }
    }
}