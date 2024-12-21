using Kiro.Application;
using Kiro.Application.GameLoops;
using Kiro.Data;
using Kiro.Data.EditMode;
using Kiro.Domain;
using Kiro.Presentation;
using Kiro.Presentation.Menus;
using PureFsm;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;
using VitalRouter.VContainer;

// ReSharper disable RedundantTypeArgumentsOfMethod

namespace Kiro.Installer
{
    public sealed class InGameLifetimeScope : LifetimeScope
    {
        [SerializeField] StageCamera _stageCamera;

        [FormerlySerializedAs("_stageHolderView")] [SerializeField]
        StageViewRegistry _stageViewRegistry;
        [SerializeField] InGameUIView _inGameUIView;
        [SerializeField] DialogueUIView _dialogueUIView;

        [Header("Path")]
        [SerializeField] PathView _pathView;

        [Header("Data")]
        [SerializeField] AllWorldsDataSO _allWorldsDataSO;
        [SerializeField] CellTypePrefabSO _cellTypePrefabSO;

        [Header("Item")]
        [SerializeField] CellView _cellViewPrefab;
        [SerializeField] StageView _stageViewPrefab;
        [SerializeField] PanelView _panelViewPrefab;
        [SerializeField] PanelCellView _panelCellPrefab;

        [Header("UI")]
        [SerializeField] MenuUIView _menuUIView;

        protected override void Configure(IContainerBuilder builder)
        {
            GameLog.Execute("DI", this, Color.green);

            builder.RegisterVitalRouter(
                routing => { routing.Map<CellFlipController>(); }
            );

            // builder.Register<GameLoopStateMachine>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GameLoopStateMachine>();
            builder.RegisterEntryPoint<InGameUIPresenter>();
            builder.RegisterEntryPoint<GameInputPresenter>();
            builder.RegisterEntryPoint<MenuUIPresenter>();

            builder.Register<StateInitialize>(Lifetime.Singleton).As<IState<GameLoopStateMachine>>();
            builder.Register<StatePlayerCanInteract>(Lifetime.Singleton).As<IState<GameLoopStateMachine>>();
            builder.Register<StatePlaySimulation>(Lifetime.Singleton).As<IState<GameLoopStateMachine>>();

            builder.Register<GameInitializer>(Lifetime.Singleton);
            builder.Register<StageMapModel>(Lifetime.Singleton);
            builder.Register<PanelDragDrop>(Lifetime.Singleton);

            builder.Register<StageFactory>(Lifetime.Singleton);
            builder.Register<CellViewFactory>(Lifetime.Singleton);
            builder.Register<PanelRootFactory>(Lifetime.Singleton);
            builder.Register<PathAnimationPlayer>(Lifetime.Singleton);
            builder.Register<PathResultModel>(Lifetime.Singleton);
            builder.Register<PanelArrangementRegistry>(Lifetime.Singleton);
            builder.Register<UndoPanel>(Lifetime.Singleton);
            builder.Register<PanelPutter>(Lifetime.Singleton);
            builder.Register<InGamePanelContainer>(Lifetime.Singleton);
            builder.Register<DialoguePlayer>(Lifetime.Singleton);

            builder.RegisterInstance<StageCamera>(_stageCamera);
            builder.RegisterInstance<StageViewRegistry>(_stageViewRegistry);
            builder.RegisterInstance<InGameUIView>(_inGameUIView);
            builder.RegisterInstance<PathView>(_pathView);
            builder.RegisterInstance<AllWorldsDataSO>(_allWorldsDataSO);
            builder.RegisterInstance<CellTypePrefabSO>(_cellTypePrefabSO);
            builder.RegisterInstance<CellView>(_cellViewPrefab);
            builder.RegisterInstance<StageView>(_stageViewPrefab);
            builder.RegisterInstance<PanelView>(_panelViewPrefab);
            builder.RegisterInstance<PanelCellView>(_panelCellPrefab);
            builder.RegisterInstance(_dialogueUIView);
            builder.RegisterInstance(_menuUIView);
        }
    }
}