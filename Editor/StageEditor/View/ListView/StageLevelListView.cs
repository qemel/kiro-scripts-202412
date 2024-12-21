using System;
using System.Collections.Generic;
using System.Linq;
using Kiro.Application;
using Kiro.Data.EditMode;
using Kiro.Domain;
using R3;
using UnityEngine.UIElements;

namespace Kiro.Editor.StageEditor.View.ListView
{
    /// <summary>
    ///     ステージデータ群のIDリストビュー
    /// </summary>
    public sealed class StageLevelListView : VisualElement, IDisposable
    {
        readonly Subject<LevelInWorldId> _stageLevelChanged = new();

        public StageLevelListView()
        {
            UIToolkitUtil.AddElement(this);
        }

        public Observable<LevelInWorldId> StageLevelChanged => _stageLevelChanged;

        public void Dispose()
        {
            _stageLevelChanged?.Dispose();
        }

        UnityEngine.UIElements.ListView _currentListView;

        public void SetCurrent(WorldId worldId)
        {
            if (_currentListView != null)
            {
                _currentListView.selectionChanged -= OnSelectionChanged;
                _currentListView.Clear();
            }

            var worlds = UIToolkitUtil.GetAssetByType<AllWorldsDataSO>();
            var dataList = worlds.GetWorldDataHolderSO(worldId).StageDataList.ToList();

            GameLog.Create()
                   .WithMessage($"ワールドID: {worldId}")
                   .WithLocation(this)
                   .Log();

            GameLog.Create()
                   .WithMessage($"ステージの数: {dataList.Count}")
                   .WithLocation(this)
                   .Log();

            _currentListView = this.Q<UnityEngine.UIElements.ListView>();
            _currentListView.makeItem = () => new Label();
            _currentListView.bindItem = (element, index) => { ((Label)element).text = "St." + (index + 1); }; // テキトーコード
            _currentListView.itemsSource = dataList;
            _currentListView.selectionType = SelectionType.Single;

            _currentListView.selectionChanged += OnSelectionChanged;
        }

        void OnSelectionChanged(IEnumerable<object> selection)
        {
            _stageLevelChanged.OnNext(((StageData)selection.First()).ActualId);
        }
    }
}