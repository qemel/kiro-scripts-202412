using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace Kiro.Presentation
{
    /// <summary>
    ///     ステージ内のパネルの集合
    /// </summary>
    public sealed class PanelGroupView : MonoBehaviour
    {
        readonly Subject<Guid> _onPanelViewClicked = new();

        PanelView[] _panelViews;

        public void Init()
        {
            _panelViews = GetComponentsInChildren<PanelView>();

            foreach (var panelView in _panelViews)
            {
                panelView.OnPanelViewClicked
                         .Subscribe(_onPanelViewClicked.OnNext)
                         .AddTo(gameObject);
            }
        }

        public async UniTask<Guid> WaitSelectAsync(CancellationToken token) =>
            await _onPanelViewClicked.FirstAsync(token);

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public List<Vector2> GetInitialPositions()
        {
            var positions = new List<Vector2>();

            foreach (var panelView in _panelViews)
            {
                // パネルの初期位置を取得
                // （(0, 0)のパネルセルは先頭だという前提）
                // TODO: 多分今これが破綻しているので修正する
                positions.Add(panelView.transform.position);
            }

            return positions;
        }
    }
}