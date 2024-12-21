using AnnulusGames.SceneSystem;
using Kiro.Application;
using Kiro.Data;
using Kiro.Presentation;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Kiro.Installer
{
    /// <summary>
    ///     プロジェクト全体のライフタイムスコープ
    ///     sceneが遷移しても破棄されないのでゲーム中ずっと生きている
    /// </summary>
    public sealed class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] LoadingScreen _loadingScreen;
        [SerializeField] StageEventDataAllSO _stageEventDataAllSO;
        [SerializeField] DialogueAllDataSO _dialogueAllDataSO;
        [SerializeField] PlayerInput _playerInput;

        protected override void Configure(IContainerBuilder builder)
        {
            GameLog.Execute("DI", this, Color.green);

            builder.RegisterEntryPoint<InputUpdater>();

            builder.Register<GameSceneLoader>(Lifetime.Singleton);
            builder.Register<AudioVolumeModel>(Lifetime.Singleton);
            builder.Register<UserSettingRepository>(Lifetime.Singleton);
            builder.Register<DialogueRepository>(Lifetime.Singleton);
            builder.Register<StageEventDataRepository>(Lifetime.Singleton);
            builder.Register<LevelLoaderPlayerPrefs>(Lifetime.Singleton).As<ILevelLoader>();
            builder.Register<LevelSaverPlayerPrefs>(Lifetime.Singleton).As<ILevelSaver>();
            builder.Register<InputStore>(Lifetime.Singleton);

            builder.RegisterInstance(_loadingScreen);
            builder.RegisterInstance(_stageEventDataAllSO);
            builder.RegisterInstance(_dialogueAllDataSO);
            builder.RegisterInstance(_playerInput);
        }
    }
}