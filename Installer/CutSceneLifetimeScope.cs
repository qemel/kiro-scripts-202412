using Kiro.Application;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Kiro.Installer
{
    public sealed class CutSceneLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            GameLog.Execute("DI", this, Color.green);

            builder.RegisterEntryPoint<CutSceneLoop>();
        }
    }
}