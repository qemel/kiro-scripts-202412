using Kiro.Domain;

namespace Kiro.Application
{
    /// <summary>
    ///     レベルをセーブする
    /// </summary>
    public interface ILevelSaver
    {
        /// <summary>
        ///     レベルをセーブする
        /// </summary>
        void Save(StageId id);
    }
}