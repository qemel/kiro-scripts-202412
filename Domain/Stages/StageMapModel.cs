using System;
using UnityEngine;
using VitalRouter;

namespace Kiro.Domain
{
    /// <summary>
    ///     ステージ上のセルを管理（インゲームにて利用）
    /// </summary>
    public sealed class StageMapModel
    {
        /// <summary>
        ///     セルの状態の変更を通知するためのPublisher
        /// </summary>
        readonly ICommandPublisher _commandPublisher;

        public StageMapModel(ICommandPublisher commandPublisher)
        {
            _commandPublisher = commandPublisher;
        }

        /// <summary>
        ///     計算用のみに利用
        /// </summary>
        public StageMap Value { get; private set; }

        public void Set(StageMap stageMap)
        {
            Value = stageMap;
        }

        public bool Contains(Vector2Int position) => Value.FieldDictionary.ContainsKey(position);

        public void Flip(Vector2Int position)
        {
            if (!Contains(position)) throw new ArgumentException("position is not found");

            Value = Value.Flip(position);
            _commandPublisher.PublishAsync(new CellFlipCommand(position, Value.FieldDictionary[position].Color));
        }
    }
}