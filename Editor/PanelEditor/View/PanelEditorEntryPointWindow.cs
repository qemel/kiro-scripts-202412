using Kiro.Application;
using Kiro.Editor.PanelEditor.Presenter;
using Kiro.Editor.PanelEditor.View.EditView;
using Kiro.Editor.PanelEditor.View.ListView;
using Kiro.Editor.StageEditor.Model;
using Kiro.Editor.StageEditor.View.EditView;
using Kiro.Editor.StageEditor.View.ListView;
using Kiro.Editor.StageEditor.View.ToolView;
using R3;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kiro.Editor.PanelEditor.View
{
    /// <summary>
    ///     エントリポイント
    /// </summary>
    public sealed class PanelEditorEntryPointWindow : EditorWindow
    {
        PanelEditView _panelEditView;
        FloatingMenuView _floatingMenuView;
        StageEditorStageIdModel _stageEditorStageIdModel;
        PanelEditorPresenter _panelEditorPresenter;
        PanelIdListView _panelIdListView;
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
            var windowTree = UIToolkitUtil.GetVisualTree(nameof(PanelEditorEntryPointWindow));
            var windowElement = windowTree.Instantiate();
            rootElement.Add(windowElement);

            InjectDependency();


            // UXMLに紐づけ
            rootElement.Q<VisualElement>("world-list-view-container").Add(_panelIdListView);
            rootElement.Q<VisualElement>("list-view-container").Add(_stageLevelListView);
            rootElement.Q<VisualElement>("tool-view-container").Add(_toolBarView);
            rootElement.Q<VisualElement>("edit-view-container").Add(_panelEditView);
            // rootElement.Q<VisualElement>("floating-menu-view-container").Add(_floatingMenuView);
        }

        /// <summary>
        ///     DI
        /// </summary>
        void InjectDependency()
        {
            _panelEditView = new PanelEditView();
            _stageLevelListView = new StageLevelListView();
            _toolBarView = new ToolBarView();
            _floatingMenuView = new FloatingMenuView();
            _panelIdListView = new PanelIdListView();

            var levelModel = new StageEditorStageIdModel();
            var stageDataModel = new StageDataModel();
            var toolBarModel = new ToolBarModel();
            var levelSaver = new LevelSaverPlayerPrefs();

            _panelEditorPresenter = new PanelEditorPresenter(
                _panelEditView,
                stageDataModel,
                levelModel,
                _stageLevelListView,
                toolBarModel,
                _toolBarView,
                _floatingMenuView,
                _panelIdListView,
                levelSaver
            );

            _panelEditorPresenter.AddTo(_disposable);

            levelModel.AddTo(_disposable);
            stageDataModel.AddTo(_disposable);
            _stageLevelListView.AddTo(_disposable);
        }

        [MenuItem("Window/PanelEditorEntryPointWindow")]
        public static void Open()
        {
            var window = GetWindow<PanelEditorEntryPointWindow>();
            window.titleContent = new GUIContent(nameof(PanelEditorEntryPointWindow));
        }
    }
}