using Kiro.Domain;

namespace Kiro.Application
{
    /// <summary>
    ///     常に1を返すレベルセーブローダー
    /// </summary>
    public sealed class LevelLoaderAlways1 : ILevelLoader
    {
        public StageId Load() => new(new WorldId(0), new LevelInWorldId(1));
    }
}