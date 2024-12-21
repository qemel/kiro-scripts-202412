using Kiro.Application;
using Kiro.Presentation;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Kiro.Installer
{
    public sealed class TitleLifetimeScope : LifetimeScope
    {
        [SerializeField] TitleUIView _titleUIView;
        [SerializeField] AudioClip _audioClip;

        protected override void Configure(IContainerBuilder builder)
        {
            GameLog.Execute("DI", this, Color.green);

            builder.RegisterEntryPoint<TitleUIPresenter>();
            builder.RegisterEntryPoint<TitleEntry>();
            builder.RegisterInstance(_titleUIView);
            builder.RegisterInstance(_audioClip);
        }
    }
}