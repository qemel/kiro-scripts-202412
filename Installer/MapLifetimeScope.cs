using Kiro.Application;
using Kiro.Domain;
using Kiro.Presentation;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace Kiro.Installer
{
    public sealed class MapLifetimeScope : LifetimeScope
    {
        [FormerlySerializedAs("_pageLevelHolderView")] [SerializeField]
        PageLevelButtonsView _pageLevelButtonsView;
        [SerializeField] BookView _bookView;

        protected override void Configure(IContainerBuilder builder)
        {
            GameLog.Execute("DI", this, Color.green);

            builder.RegisterEntryPoint<MapPresenter>();

            builder.Register<BookPageModel>(Lifetime.Singleton);

            builder.RegisterInstance(_pageLevelButtonsView);
            builder.RegisterInstance(_bookView);
        }
    }
}