using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kiro.Domain;
using Kiro.Presentation;
using PureFsm;
using R3;
using UnityEngine;

namespace Kiro.Application.GameLoops
{
    /// <summary>
    ///     プレイヤーがパネルを設置するステート
    /// </summary>
    public sealed class StatePlayerCanInteract : IState<GameLoopStateMachine>, IDisposable
    {
        readonly StageViewRegistry _stageViewRegistry;
        readonly StageMapModel _stageMapModel;
        readonly PathResultModel _pathResultModel;
        readonly PanelDragDrop _panelDragDrop;
        readonly PanelPutter _panelPutter;
        readonly InGameUIView _inGameUIView;
        readonly PanelArrangementRegistry _panelArrangementRegistry;
        readonly InputStore _inputStore;

        readonly CancellationTokenSource _cts = new();

        public StatePlayerCanInteract(
            StageViewRegistry stageViewRegistry, StageMapModel stageMapModel, PathResultModel pathResultModel,
            PanelDragDrop panelDragDrop, InGameUIView inGameUIView, PanelPutter panelPutter,
            PanelArrangementRegistry panelArrangementRegistry, InputStore inputStore
        )
        {
            _stageViewRegistry = stageViewRegistry;
            _stageMapModel = stageMapModel;
            _pathResultModel = pathResultModel;
            _panelDragDrop = panelDragDrop;
            _inGameUIView = inGameUIView;
            _panelPutter = panelPutter;
            _panelArrangementRegistry = panelArrangementRegistry;
            _inputStore = inputStore;
        }

        public async UniTask<int> EnterAsync(CancellationToken token)
        {
            LoopAsync(_cts.Token).Forget();

            var res = await WaitClickAsync(token);

            _cts.Cancel();

            return res;
        }

        /// <summary>
        ///     パネルを設置するループ
        /// </summary>
        async UniTask LoopAsync(CancellationToken token)
        {
            var stageView = _stageViewRegistry.Value;

            while (!token.IsCancellationRequested)
            {
                // PathのModel更新
                _pathResultModel.Set(PathClearJudgeLogic.GetResult(_stageMapModel.Value));

                // パネルの選択
                var guid = await stageView.PanelGroup.WaitSelectAsync(token);

                // パネルの設置
                var dropPositions = await _panelDragDrop.GetDropPositionsAsync(guid, token);
                if (!dropPositions.Any())
                {
                    GameLog.Execute("パネルの設置に失敗しました", this, Color.red);
                    continue;
                }

                var arrangement = new PanelArrangement(dropPositions.First(), guid);
                var put = _panelPutter.Execute(arrangement);

                if (!put)
                {
                    GameLog.Execute("パネルの設置に失敗しました", this, Color.red);
                    continue;
                }

                _panelArrangementRegistry.Push(arrangement);
            }
        }

        /// <summary>
        ///     「進むボタン」クリックを待つ
        /// </summary>
        async UniTask<int> WaitClickAsync(CancellationToken token)
        {
            await _inGameUIView
                  .ButtonClickedAsObservable(ButtonType.PlayPathAnimation)
                  .FirstAsync(token)
                  .AsUniTask();

            return (int)GameLoopStateEvent.PlaySimulation;
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}