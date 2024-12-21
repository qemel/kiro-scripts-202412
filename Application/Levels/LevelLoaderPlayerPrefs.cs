using Kiro.Domain;
using UnityEngine;

namespace Kiro.Application
{
    /// <summary>
    ///     PlayerPrefsにレベルを保存する
    /// </summary>
    public sealed class LevelLoaderPlayerPrefs : ILevelLoader
    {
        const string WorldKey = "CurrentWorld";
        const string LevelKey = "CurrentLevel";

        public StageId Load()
        {
            var world = PlayerPrefs.GetInt(WorldKey, 0);
            var level = PlayerPrefs.GetInt(LevelKey, 1);

            GameLog.Execute($"レベルのロード(PlayerPrefs): {world}-{level}", this, Color.cyan);

            return new StageId(new WorldId(world), new LevelInWorldId(level));
        }
    }
}