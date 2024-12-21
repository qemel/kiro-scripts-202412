using Kiro.Domain;
using Kiro.Presentation;
using VitalRouter;

namespace Kiro.Application
{
    /// <summary>
    ///     CellFlipCommandを受け取り、StageViewに反映する
    /// </summary>
    [Routes]
    public partial class CellFlipController
    {
        readonly StageViewRegistry _stageViewRegistry;

        public CellFlipController(StageViewRegistry stageViewRegistry)
        {
            _stageViewRegistry = stageViewRegistry;
        }

        [Route]
        void On(CellFlipCommand cmd)
        {
            _stageViewRegistry.Value.Flip(cmd.Position, cmd.Color);
        }
    }
}