using System;
using AnnulusGames.LucidTools.Audio;
using R3;

namespace Kiro.Presentation
{
    public sealed class AudioVolumeModel : IDisposable
    {
        public ReadOnlyReactiveProperty<float> BgmVolumeReactiveProperty => _bgmVolumeReactiveProperty;
        readonly ReactiveProperty<float> _bgmVolumeReactiveProperty = new(0);
        public ReadOnlyReactiveProperty<float> SfxVolumeReactiveProperty => _sfxVolumeReactiveProperty;
        readonly ReactiveProperty<float> _sfxVolumeReactiveProperty = new(0);

        public void SetBgmVolume(float volume)
        {
            _bgmVolumeReactiveProperty.Value = volume;
            LucidAudio.BGMVolume = volume;
        }

        public void SetSfxVolume(float volume)
        {
            _sfxVolumeReactiveProperty.Value = volume;
            LucidAudio.SEVolume = volume;
        }

        public void Dispose()
        {
            _bgmVolumeReactiveProperty.Dispose();
            _sfxVolumeReactiveProperty.Dispose();
        }
    }
}