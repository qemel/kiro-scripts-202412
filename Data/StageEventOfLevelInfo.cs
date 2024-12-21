using System;
using Kiro.Domain;
using Kiro.Presentation;
using UnityEngine;

namespace Kiro.Data
{
    [Serializable]
    public class StageEventOfLevelInfo
    {
        [SerializeField] int _level;
        public LevelInWorldId Level => new(_level);

        [SerializeField] TimelinePlayer _onEnterTimelinePlayer;
        [SerializeField] TimelinePlayer _onExitTimelinePlayer;

        public TimelinePlayer OnEnterTimelinePlayer => _onEnterTimelinePlayer;
        // public TimelinePlayer OnExitTimelinePlayer => _onExitTimelinePlayer;
    }
}