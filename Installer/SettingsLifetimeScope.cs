using Kiro.Application;
using Kiro.Presentation;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Kiro.Installer
{
    public sealed class SettingsLifetimeScope : LifetimeScope
    {
        [SerializeField] SettingScreenUIView _settingScreenUIView;
        [SerializeField] AudioSliderUIView _audioSliderUIView;

        protected override void Configure(IContainerBuilder builder)
        {
            GameLog.Execute("DI", this, Color.green);

            builder.RegisterEntryPoint<SettingsUIPresenter>();
            builder.RegisterInstance(_settingScreenUIView);
            builder.RegisterInstance(_audioSliderUIView);
        }
    }
}