using System.Collections.Generic;
using System.Linq;
using Alchemy.Inspector;
using Kiro.Domain;
using UnityEngine;

namespace Kiro.Data.EditMode
{
    /// <summary>
    ///     ステージデータを保持するScriptableObject
    ///     基本的にAllWorldsDataSO以外は参照しない
    /// </summary>
    [CreateAssetMenu(menuName = "u1w202408/WorldDataHolderSO")]
    public sealed class WorldDataHolderSO : ScriptableObject
    {
        [SerializeField] int _worldId;

        [ValidateInput("Validate", "StageDataListのインデックス管理が間違っています。重複したIDがないか確認してください。")]
        [SerializeField] List<StageData> _stageDataList;

        public WorldId WorldId => new(_worldId);
        public IEnumerable<StageData> StageDataList => _stageDataList;

        public StageData GetStageData(LevelInWorldId id)
        {
            return _stageDataList.FirstOrDefault(data => data.ActualId == id);
        }

        bool Validate(List<StageData> stageDataList)
        {
            var idList = new List<int>();
            foreach (var stageData in stageDataList)
            {
                if (idList.Contains(stageData.ActualId.Value)) return false;
                idList.Add(stageData.ActualId.Value);
            }

            return true;
        }
    }
}