using System.Collections.Generic;
using System.Linq;
using Alchemy.Inspector;
using Kiro.Domain;
using UnityEngine;

namespace Kiro.Data.EditMode
{
    [CreateAssetMenu(fileName = "AllWorldsDataSO", menuName = "u1w202408/AllWorldsDataSO")]
    public sealed class AllWorldsDataSO : ScriptableObject
    {
        [ValidateInput("Validate", "WorldIdが重複している要素があります。")]
        [SerializeField] WorldDataHolderSO[] _worldDataHolderSOs;

        public IEnumerable<WorldDataHolderSO> WorldDataHolderSOs => _worldDataHolderSOs;

        public WorldDataHolderSO GetWorldDataHolderSO(WorldId worldId)
        {
            return _worldDataHolderSOs.FirstOrDefault(data => data.WorldId == worldId);
        }

        /// <summary>
        ///     IDから対応するステージデータを取得
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        public StageData GetStageData(StageId stageId) =>
            GetWorldDataHolderSO(stageId.WorldId)?.GetStageData(stageId.Level);

        static bool Validate(WorldDataHolderSO[] worldDataHolderSOs)
        {
            // WorldIdの重複チェック
            var worldIds = new HashSet<WorldId>();
            foreach (var worldData in worldDataHolderSOs)
            {
                if (!worldIds.Add(worldData.WorldId))
                {
                    Debug.LogError($"WorldId is duplicated: {worldData.WorldId}");
                    return false;
                }
            }

            return true;
        }
    }
}