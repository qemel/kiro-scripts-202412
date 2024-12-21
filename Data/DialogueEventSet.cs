using System;
using System.Collections.Generic;
using Kiro.Domain;
using UnityEngine;

namespace Kiro.Data
{
    /// <summary>
    ///     一連の対話
    /// </summary>
    [Serializable]
    public sealed class DialogueEventSet
    {
        public DialogueSetId Id => new(new StageId(new WorldId(_worldId), new LevelInWorldId(_levelId)), _playTiming);
        public IEnumerable<Dialogue> Dialogues => _dialogues;

        [SerializeField] int _worldId;
        [SerializeField] int _levelId;

        [SerializeField] DialoguePlayTiming _playTiming;
        [SerializeField] Dialogue[] _dialogues;
    }
}