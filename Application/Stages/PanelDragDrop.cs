using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kiro.Domain.Utils;
using Kiro.Presentation;
using UnityEngine;

namespace Kiro.Application
{
    public abstract record DropPositionResult;

    public record DropPositionSuccess(Vector2Int Position) : DropPositionResult;

    public record DropPositionFailure : DropPositionResult;

    /// <summary>
    ///     パネルのドラッグアンドドロップをし、ドロップした位置を返す
    /// </summary>
    public sealed class PanelDragDrop
    {
        readonly InGamePanelContainer _inGamePanelContainer;

        public PanelDragDrop(InGamePanelContainer inGamePanelContainer)
        {
            _inGamePanelContainer = inGamePanelContainer;
        }

        /// <summary>
        ///     ドラッグドロップを実行する
        /// </summary>
        public async UniTask<IEnumerable<Vector2Int>> GetDropPositionsAsync(
            Guid id, CancellationToken token
        )
        {
            var panelRoot = _inGamePanelContainer.Get(id);
            var panelView = panelRoot.View;

            GameLog.Create()
                   .WithMessage($"ドラッグ開始: {panelRoot.View.InitialWorldPosition}")
                   .WithLocation(this)
                   .Log();

            var offsetVector = DragOffsetVector(panelView);

            GameLog.Create()
                   .WithMessage($"ドラッグオフセット: {offsetVector}")
                   .WithLocation(this)
                   .Log();

            var dropPosition = await DragAsync(panelView, offsetVector, token);

            var dropPositionOrigin = dropPosition - offsetVector;

            var dropResult = DropPositionToMapPosition(dropPositionOrigin);

            switch (dropResult)
            {
                case DropPositionFailure:
                    panelView.ResetPosition();
                    return Enumerable.Empty<Vector2Int>();
                case DropPositionSuccess success:
                    panelView.SetPosition(dropPositionOrigin);
                    var panelMapRelativePositions = panelView.MapRelativePositionToCell.Keys;
                    return panelMapRelativePositions.Select(relPos => success.Position + relPos);
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     ドラッグ中の処理
        /// </summary>
        static async UniTask<Vector2> DragAsync(PanelView panelView, Vector2 offsetVector, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // TODO: inputsystemに変更
                if (!Input.GetMouseButton(0)) break;

                panelView.SetPosition(Input.mousePosition.ToWorldPosition() - offsetVector);

                await UniTask.Yield(token);
            }

            return Input.mousePosition.ToWorldPosition();
        }

        /// <summary>
        ///     ドラッグ開始時のパネルの位置とドラッグ中のマウス位置からオフセットを計算
        /// </summary>
        static Vector2 DragOffsetVector(PanelView panelView)
        {
            var mousePosition = Input.mousePosition.ToWorldPosition();
            var panelPosition = (Vector2)panelView.transform.position;
            return mousePosition - panelPosition;
        }

        /// <summary>
        ///     ドロップした位置をマップ上の位置に変換
        /// </summary>
        static DropPositionResult DropPositionToMapPosition(Vector2 dropPosition)
        {
            var results = new RaycastHit2D[4];
            Physics2D.RaycastNonAlloc(dropPosition, Vector2.zero, results);

            results = results.Where(x => x.collider != null).ToArray();

            // ドロップした位置にCellViewがない場合はドロップをキャンセル
            if (!results.Any(x => x.collider.TryGetComponent(out CellView _))) return new DropPositionFailure();

            // ドロップした位置のCellViewを取得(座標は(0, 0)のCellView)
            var cellViewZero = results.First(x => x.collider.TryGetComponent(out CellView _))
                                      .collider.GetComponent<CellView>();

            return new DropPositionSuccess(cellViewZero.MapPosition);
        }
    }
}