using System;
using Kiro.Domain;
using R3;

namespace Kiro.Editor.StageEditor.Model
{
    public sealed class StageEditorStageIdModel : IDisposable
    {
        readonly ReactiveProperty<StageId> _current = new();
        public ReadOnlyReactiveProperty<StageId> Current => _current;

        public void Dispose()
        {
            _current?.Dispose();
        }

        public void Set(StageId stageId)
        {
            _current.Value = stageId;
        }

        public void SetLevel(LevelInWorldId level)
        {
            _current.Value = _current.Value with { Level = level };
        }

        public void SetWorld(WorldId worldId)
        {
            _current.Value = new StageId(worldId, new LevelInWorldId(1));
        }
    }
}