using System;
using System.Linq;
using Kiro.Data.EditMode;
using Kiro.Domain;
using R3;
using UnityEngine.UIElements;

namespace Kiro.Editor.StageEditor.View.ListView
{
    public sealed class WorldListView : VisualElement, IDisposable
    {
        readonly Subject<WorldId> _worldIdChanged = new();

        public WorldListView()
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
            var worldsData = UIToolkitUtil.GetAssetByType<AllWorldsDataSO>();
            var worldData = worldsData.WorldDataHolderSOs.ToList();

            var listView = this.Q<UnityEngine.UIElements.ListView>();
            listView.makeItem = () => new Label();
            listView.bindItem = (element, index) => { ((Label)element).text = "World." + new WorldId(index).Value; };
            listView.itemsSource = worldData;
            listView.selectionType = SelectionType.Single;

            listView.selectionChanged +=
                objects => _worldIdChanged.OnNext(new WorldId(listView.selectedIndex));
        }
    }
}