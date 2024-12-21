using System;
using Alchemy.Inspector;
using Kiro.Application;
using Kiro.Domain;
using UnityEngine;

namespace Kiro.Data
{
    /// <summary>
    ///     ステージのイベントデータを保持するScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "StageEventDataOfWorldSO", menuName = "u1w202408/StageEventDataOfWorldSO")]
    public sealed class StageEventDataOfWorldSO : ScriptableObject
    {
        [SerializeField] int _worldId;
        public WorldId WorldId => new(_worldId);

        [ValidateInput("Validate", "StageDataListのインデックス管理が間違っています。重複したIDがないか確認してください。")]
        [SerializeField] StageEventOfLevelInfo[] _stageEventInfos;

        public StageEventOfLevelInfo OfLevel(LevelInWorldId level)
        {
            foreach (var stageEventInfo in _stageEventInfos)
            {
                if (stageEventInfo.Level == level) return stageEventInfo;
            }

            throw new Exception(
                GameLog.Create()
                       .WithFrame()
                       .WithLocation(this)
                       .WithMessage($"指定したレベルのイベントデータが見つかりませんでした。: {level}")
                       .ToString()
            );
        }

        /// <summary>
        ///     重複したIDがないか確認する
        /// </summary>
        public bool Validate(StageEventOfLevelInfo[] stageEventInfos)
        {
            for (var i = 0; i < stageEventInfos.Length; i++)
            {
                for (var j = i + 1; j < stageEventInfos.Length; j++)
                {
                    if (stageEventInfos[i].Level == stageEventInfos[j].Level) return false;
                }
            }

            return true;
        }
    }
}