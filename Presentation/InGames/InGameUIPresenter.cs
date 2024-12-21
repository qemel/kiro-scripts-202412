using Kiro.Application;
using Kiro.Domain;
using R3;
using UnityEngine;
using VContainer.Unity;

namespace Kiro.Presentation
{
    public sealed class InGameUIPresenter : ControllerBase, IStartable
    {
        readonly PathResultModel _pathResultModel;
        readonly StageViewRegistry _stageViewRegistry;
        readonly InGameUIView _inGameUIView;
        readonly UndoPanel _undoPanel;
        readonly PanelArrangementRegistry _panelArrangementRegistry;
        readonly GameSceneLoader _gameSceneLoader;
        readonly InputStore _inputStore;

        public InGameUIPresenter(
            PathResultModel pathResultModel, StageViewRegistry stageViewRegistry,
            InGameUIView inGameUIView, UndoPanel undoPanel,
            PanelArrangementRegistry panelArrangementRegistry, GameSceneLoader gameSceneLoader, InputStore inputStore
        )
        {
            _pathResultModel = pathResultModel;
            _stageViewRegistry = stageViewRegistry;
            _inGameUIView = inGameUIView;
            _undoPanel = undoPanel;
            _panelArrangementRegistry = panelArrangementRegistry;
            _gameSceneLoader = gameSceneLoader;
            _inputStore = inputStore;
        }

        public void Start()
        {
            ModelToView();
            ViewToModel();
        }

        void ViewToModel()
        {
            _inGameUIView
                .ButtonClickedAsObservable(ButtonType.Retry)
                .Subscribe(_ => _inputStore.PublishRetry())
                .AddTo(this);

            _inGameUIView
                .ButtonClickedAsObservable(ButtonType.Undo)
                .Subscribe(_ => _inputStore.PublishUndo())
                .AddTo(this);

            _inGameUIView
                .ButtonClickedAsObservable(ButtonType.Escape)
                .Subscribe(_ => _inputStore.PublishEscape())
                .AddTo(this);
        }

        void ModelToView()
        {
            _pathResultModel
                .ReactiveProperty
                .Where(x => _stageViewRegistry.Value != null) // Todo: もうちょいいい方法考える
                .Subscribe(
                    pathResult => { GameLog.Execute("PathResultModelが更新されました:" + pathResult, this, Color.yellow); }
                )
                .AddTo(this);

            // PlayPathボタンの表示タイミングの制御
            _pathResultModel
                .ReactiveProperty
                .Subscribe(
                    pathResult =>
                    {
                        _inGameUIView.SetButtonInteractable(ButtonType.PlayPathAnimation, pathResult is HasPath);
                    }
                )
                .AddTo(this);

            // Undo, Retryボタンの表示制御
            _panelArrangementRegistry
                .CountReactiveProperty
                .Subscribe(
                    count =>
                    {
                        _inGameUIView.SetButtonInteractable(ButtonType.Undo, count > 0);
                        _inGameUIView.SetButtonInteractable(ButtonType.Retry, count > 0);
                    }
                )
                .AddTo(this);
        }
    }
}