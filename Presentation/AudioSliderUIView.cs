using AnnulusGames.LucidTools.Audio;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Kiro.Presentation
{
    public sealed class AudioSliderUIView : MonoBehaviour
    {
        [SerializeField] Slider _sliderBgm;
        [SerializeField] Slider _sliderSfx;
        [SerializeField] AudioClip _sfxTester;

        public Observable<float> BgmVolumeAsObservable => _sliderBgm.OnValueChangedAsObservable();
        public Observable<float> SfxVolumeAsObservable => _sliderSfx.OnValueChangedAsObservable();

        public void SetBgmVolume(float volume)
        {
            _sliderBgm.value = volume;
        }

        public void SetSfxVolume(float volume)
        {
            _sliderSfx.value = volume;
        }

        public void PlayTesterSfx()
        {
            LucidAudio.PlaySE(_sfxTester);
        }
    }
}