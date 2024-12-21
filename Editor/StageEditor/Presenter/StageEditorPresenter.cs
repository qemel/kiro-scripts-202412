using System;
using Kiro.Application;
using Kiro.Data.EditMode;
using Kiro.Editor.StageEditor.Model;
using Kiro.Editor.StageEditor.View.EditView;
using Kiro.Editor.StageEditor.View.ListView;
using Kiro.Editor.StageEditor.View.ToolView;
using R3;
using UnityEditor;

namespace Kiro.Editor.StageEditor.Presenter
{
    public sealed class StageEditorPresenter : IDisposable
    {
        readonly EditView _editView;
        readonly FloatingMenuView _floatingMenuView;
        readonly StageDataModel _stageDataModel;
        readonly StageEditorStageIdModel _stageEditorStageIdModel;
        readonly StageLevelListView _stageLevelListView;
        readonly ToolBarModel _toolBarModel;
        readonly ToolBarView _toolBarView;
        readonly WorldListView _worldListView;
        readonly ILevelSaver _levelSaver;

        readonly CompositeDisposable _disposable = new();

        public StageEditorPresenter(
            EditView editView,
            StageDataModel stageDataModel,
            StageEditorStageIdModel stageEditorStageIdModel,
            StageLevelListView stageLevelListView,
            ToolBarModel toolBarModel,
            ToolBarView toolBarView, FloatingMenuView floatingMenuView, WorldListView worldListView,
            ILevelSaver levelSaver
        )
        {
            _editView = editView;
            _stageDataModel = stageDataModel;
            _stageEditorStageIdModel = stageEditorStageIdModel;
            _stageLevelListView = stageLevelListView;
            _toolBarModel = toolBarModel;
            _toolBarView = toolBarView;
            _floatingMenuView = floatingMenuView;
            _worldListView = worldListView;
            _levelSaver = levelSaver;

            Init();
        }

        public void Dispose()
        {
            _stageEditorStageIdModel?.Dispose();
            _stageDataModel?.Dispose();
            _disposable?.Dispose();
        }

        void Init()
        {
            ModelToView();
            ViewToModel();
        }

        void ViewToModel()
        {
            _worldListView
                .WorldIdChanged
                .Subscribe(worldId => _stageEditorStageIdModel.SetWorld(worldId))
                .AddTo(_disposable);

            _stageLevelListView
                .StageLevelChanged
                .Subscribe(level => _stageEditorStageIdModel.SetLevel(level))
                .AddTo(_disposable);

            _editView
                .CellClicked
                .Subscribe(
                    pos =>
                    {
                        GameLog.Create()
                               .WithMessage("CellClicked")
                               .WithLocation(this)
                               .Log();

                        _stageDataModel.SetType(pos, _toolBarModel.Selecting.CurrentValue);
                    }
                )
                .AddTo(_disposable);

            _editView
                .ChangedEditStageSize
                .Subscribe(
                    changeEditStageSize =>
                    {
                        GameLog.Create()
                               .WithMessage("ChangeEditStageSizeEvent")
                               .WithLocation(this)
                               .Log();

                        _stageDataModel.ChangeSizeByOne(changeEditStageSize);
                    }
                )
                .AddTo(_disposable);

            _editView
                .PlayButtonClicked
                .Subscribe(
                    _ =>
                    {
                        GameLog.Create()
                               .WithMessage("PlayButtonClicked")
                               .WithLocation(this)
                               .Log();

                        _levelSaver.Save(_stageEditorStageIdModel.Current.CurrentValue);
                        EditorApplication.isPlaying = true;
                    }
                )
                .AddTo(_disposable);

            _toolBarView
                .OnClick
                .Subscribe(x => { _toolBarModel.SetCurrent(x); })
                .AddTo(_disposable);

            _floatingMenuView
                .OnAddPanel
                .Subscribe(
                    _ =>
                    {
                        GameLog.Create()
                               .WithMessage("OnAddPanel")
                               .WithLocation(this)
                               .Log();

                        _stageDataModel.AddPanel();
                    }
                )
                .AddTo(_disposable);
        }

        void ModelToView()
        {
            _stageEditorStageIdModel
                .Current
                .Subscribe(
                    id =>
                    {
                        GameLog
                            .Create()
                            .WithMessage($"StageEditorStageIdModel.Value: {id}")
                            .WithLocation(this)
                            .Log();

                        _editView.ShowCurrent(id);
                        _stageLevelListView.SetCurrent(id.WorldId);

                        var stageData = UIToolkitUtil.GetAssetByType<AllWorldsDataSO>().GetStageData(id);
                        _stageDataModel.Set(stageData);
                    }
                )
                .AddTo(_disposable);

            _stageDataModel
                .Changed
                .Subscribe(
                    stageData =>
                    {
                        GameLog
                            .Create()
                            .WithMessage($"StageEditorStageIdModel.Value: {stageData}")
                            .WithLocation(this)
                            .Log();

                        _editView.ShowCurrent(stageData);
                        _floatingMenuView.SetCurrent(stageData.GetPanelInfos());
                        StageEditSaver.Execute();
                    }
                )
                .AddTo(_disposable);

            _toolBarModel
                .Selecting
                .Subscribe(
                    cellType =>
                    {
                        GameLog
                            .Create()
                            .WithMessage($"ToolBarModel.Selecting: {cellType}")
                            .WithLocation(this)
                            .Log();

                        _toolBarView.ActivateSelected(cellType);
                    }
                )
                .AddTo(_disposable);
        }
    }
}