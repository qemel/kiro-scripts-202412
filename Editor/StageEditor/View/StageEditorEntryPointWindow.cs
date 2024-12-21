using Kiro.Application;
using Kiro.Editor.StageEditor.Model;
using Kiro.Editor.StageEditor.Presenter;
using Kiro.Editor.StageEditor.View.EditView;
using Kiro.Editor.StageEditor.View.ListView;
using Kiro.Editor.StageEditor.View.ToolView;
using R3;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kiro.Editor.StageEditor.View
{
    /// <summary>
    ///     エントリポイント
    /// </summary>
    public sealed class StageEditorEntryPointWindow : EditorWindow
    {
        EditView.EditView _editView;
        FloatingMenuView _floatingMenuView;
        StageEditorStageIdModel _stageEditorStageIdModel;
        StageEditorPresenter _stageEditorPresenter;
        WorldListView _worldListView;
        StageLevelListView _stageLevelListView;
        ToolBarView _toolBarView;

        readonly CompositeDisposable _disposable = new();

        void OnDisable()
        {
            _disposable?.Dispose();
        }

        void CreateGUI()
        {
            var rootElement = rootVisualElement;

            // UXMLからUIを作成
            var windowTree = UIToolkitUtil.GetVisualTree(nameof(StageEditorEntryPointWindow));
            var windowElement = windowTree.Instantiate();
            rootElement.Add(windowElement);

            InjectDependency();


            // UXMLに紐づけ
            rootElement.Q<VisualElement>("world-list-view-container").Add(_worldListView);
            rootElement.Q<VisualElement>("list-view-container").Add(_stageLevelListView);
            rootElement.Q<VisualElement>("tool-view-container").Add(_toolBarView);
            rootElement.Q<VisualElement>("edit-view-container").Add(_editView);
            // rootElement.Q<VisualElement>("floating-menu-view-container").Add(_floatingMenuView);
        }

        /// <summary>
        ///     DI
        /// </summary>
        void InjectDependency()
        {
            _editView = new EditView.EditView();
            _stageLevelListView = new StageLevelListView();
            _toolBarView = new ToolBarView();
            _floatingMenuView = new FloatingMenuView();
            _worldListView = new WorldListView();

            var levelModel = new StageEditorStageIdModel();
            var stageDataModel = new StageDataModel();
            var toolBarModel = new ToolBarModel();
            var levelSaver = new LevelSaverPlayerPrefs();

            _stageEditorPresenter = new StageEditorPresenter(
                _editView,
                stageDataModel,
                levelModel,
                _stageLevelListView,
                toolBarModel,
                _toolBarView,
                _floatingMenuView,
                _worldListView,
                levelSaver
            );

            _stageEditorPresenter.AddTo(_disposable);

            levelModel.AddTo(_disposable);
            stageDataModel.AddTo(_disposable);
            _stageLevelListView.AddTo(_disposable);
        }

        [MenuItem("Window/StageEditorEntryPointWindow")]
        public static void Open()
        {
            var window = GetWindow<StageEditorEntryPointWindow>();
            window.titleContent = new GUIContent(nameof(StageEditorEntryPointWindow));
        }
    }
}