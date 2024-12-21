using Kiro.Domain;

namespace Kiro.Data
{
    public sealed class StageEventDataRepository
    {
        readonly StageEventDataAllSO _stageEventDataAllSO;

        public StageEventDataRepository(StageEventDataAllSO stageEventDataAllSO)
        {
            _stageEventDataAllSO = stageEventDataAllSO;
        }

        public StageEventOfLevelInfo GetBy(StageId stageId)
        {
            var worldId = stageId.WorldId;
            var level = stageId.Level;

            var stageEventOfLevelInfo = _stageEventDataAllSO.OfWorldId(worldId).OfLevel(level);

            return stageEventOfLevelInfo;
        }
    }
}