using Kiro.Domain;

namespace Kiro.Application
{
    /// <summary>
    ///     レベルをロードする
    /// </summary>
    public interface ILevelLoader
    {
        /// <summary>
        ///     レベルをロードする
        /// </summary>
        StageId Load();
    }
}