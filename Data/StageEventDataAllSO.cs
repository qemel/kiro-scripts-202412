using Kiro.Domain;
using UnityEngine;

namespace Kiro.Data
{
    [CreateAssetMenu(fileName = "StageEventDataAllSO", menuName = "u1w202408/StageEventDataAllSO")]
    public sealed class StageEventDataAllSO : ScriptableObject
    {
        [SerializeField] StageEventOfWorldInfo[] _stageEventWorldInfos;
        public StageEventOfWorldInfo[] StageEventWorldInfos => _stageEventWorldInfos;

        public StageEventDataOfWorldSO OfWorldId(WorldId worldId)
        {
            foreach (var stageEventWorldInfo in _stageEventWorldInfos)
            {
                if (stageEventWorldInfo.StageEventDataOfWorldSO.WorldId == worldId)
                    return stageEventWorldInfo.StageEventDataOfWorldSO;
            }

            return null;
        }
    }
}