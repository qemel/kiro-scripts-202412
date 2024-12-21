using System;
using System.Collections.Generic;
using System.Linq;
using Kiro.Domain;
using R3;

namespace Kiro.Application
{
    /// <summary>
    ///     ユーザのパネルの配置状況を記録しておく
    /// </summary>
    public sealed class PanelArrangementRegistry : IDisposable
    {
        readonly Stack<PanelArrangement> _panelArrangements = new();

        public ReadOnlyReactiveProperty<int> CountReactiveProperty => _countReactiveProperty;
        readonly ReactiveProperty<int> _countReactiveProperty = new(0);

        /// <summary>
        ///     スタックに追加
        /// </summary>
        public void Push(PanelArrangement panelArrangement)
        {
            _panelArrangements.Push(panelArrangement);

            GameLog.Create()
                   .WithMessage(ToString())
                   .WithLocation(this)
                   .Log();

            _countReactiveProperty.Value = _panelArrangements.Count;
        }

        /// <summary>
        ///     スタックから取り出す
        /// </summary>
        public PanelArrangement Pop()
        {
            var pop = _panelArrangements.Pop();

            GameLog.Create()
                   .WithMessage(ToString())
                   .WithLocation(this)
                   .Log();

            _countReactiveProperty.Value = _panelArrangements.Count;

            return pop;
        }

        /// <summary>
        ///     スタックの先頭を取得
        /// </summary>
        public PanelArrangement Peek() => _panelArrangements.Peek();

        /// <summary>
        ///     スタックが空でない
        /// </summary>
        public bool Any => _panelArrangements.Any();

        public override string ToString()
        {
            return _panelArrangements.Aggregate(
                "",
                (current, panelPlacement) => current + (panelPlacement.PutOriginPosition + " ")
            );
        }

        public void Dispose()
        {
            _countReactiveProperty?.Dispose();
        }
    }
}