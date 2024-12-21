using Kiro.Domain;
using UnityEngine;

namespace Kiro.Application
{
    public sealed class LevelSaverPlayerPrefs : ILevelSaver
    {
        const string WorldKey = "CurrentWorld";
        const string LevelKey = "CurrentLevel";

        public void Save(StageId id)
        {
            PlayerPrefs.SetInt(WorldKey, id.WorldId.Value);
            PlayerPrefs.SetInt(LevelKey, id.Level.Value);

            GameLog.Execute($"レベルのセーブ(PlayerPrefs): {id.WorldId.Value}-{id.Level.Value}", this, Color.cyan);
        }
    }
}