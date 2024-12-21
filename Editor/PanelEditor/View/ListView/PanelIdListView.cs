using System;
using System.Linq;
using Kiro.Data.EditMode;
using Kiro.Domain;
using R3;
using UnityEngine.UIElements;

namespace Kiro.Editor.PanelEditor.View.ListView
{
    public sealed class PanelIdListView : VisualElement, IDisposable
    {
        readonly Subject<WorldId> _worldIdChanged = new();

        public PanelIdListView()
        {
            UIToolkitUtil.AddElement(this);

            Initialize();
        }

        public Observable<WorldId> WorldIdChanged => _worldIdChanged;

        public void Dispose()
        {
            _worldIdChanged?.Dispose();
        }

        void Initialize()
        {
            var panelsData = UIToolkitUtil.GetAssetByType<AllPanelsDataSO>();
            var panelData = panelsData.WorldDataHolderSOs.ToList();

            var listView = this.Q<UnityEngine.UIElements.ListView>();
            listView.makeItem = () => new Label();
            listView.bindItem = (element, index) => { ((Label)element).text = "World." + new WorldId(index).Value; };
            listView.itemsSource = panelData;
            listView.selectionType = SelectionType.Single;

            listView.selectionChanged +=
                objects => _worldIdChanged.OnNext(new WorldId(listView.selectedIndex));
        }
    }
}