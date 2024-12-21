using System;
using System.Collections.Generic;
using Kiro.Data.EditMode;
using R3;
using UnityEngine.UIElements;

namespace Kiro.Editor.StageEditor.View.EditView
{
    /// <summary>
    ///     パネル追加の管理とか
    /// </summary>
    public class FloatingMenuView : VisualElement, IDisposable
    {
        readonly CompositeDisposable _disposable = new();

        readonly Subject<Unit> _onAddPanel = new();

        public FloatingMenuView()
        {
            UIToolkitUtil.AddElement(this);

            Initialize();
        }

        public Observable<Unit> OnAddPanel => _onAddPanel;

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        public void SetCurrent(IEnumerable<PanelInfo> panelInfos)
        {
            // TODO: Implement 

            // var container = this.Q<VisualElement>("panels-container");
            //
            // container.Clear();
            //
            // foreach (var panelInfo in panelInfos)
            // {
            //     container.AddToClassList("positions-container");
            //     var positionsContainer = container.Q<VisualElement>("positions-container");
            //
            //     foreach (var position in panelInfo.RelativePositions)
            //     {
            //         var positionElement = new VisualElement();
            //         positionElement.AddToClassList("vector2int-field");
            //
            //         var xField = new IntegerField("X") { value = position.x };
            //         var yField = new IntegerField("Y") { value = position.y };
            //
            //         // xField.RegisterValueChangedCallback(evt =>  );
            //         // yField.RegisterValueChangedCallback(evt =>  );
            //
            //         positionElement.Add(xField);
            //         positionElement.Add(yField);
            //         positionsContainer.Add(positionElement);
            //     }
            // }
        }

        void Initialize()
        {
            var buttonAdd = this.Q<Button>("add-panel-button");

            buttonAdd.clicked += () => _onAddPanel.OnNext(Unit.Default);

            _onAddPanel.AddTo(_disposable);
        }
    }
}