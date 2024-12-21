using System;
using Kiro.Domain;
using UnityEngine;

namespace Kiro.Editor.StageEditor.Data
{
    public abstract record StageToolBarIcon;

    [Serializable]
    public sealed record StageToolBarTypeIcon : StageToolBarIcon
    {
        [SerializeField] CellType _type;
        [SerializeField] Sprite _icon;
        public CellType Type => _type;
        public Sprite Icon => _icon;
    }

    [Serializable]
    public sealed record StageToolBarColorIcon : StageToolBarIcon
    {
        [SerializeField] CellColor _color;
        [SerializeField] Sprite _icon;
        public CellColor Color => _color;
        public Sprite Icon => _icon;
    }

    [Serializable]
    public sealed record StageToolBarToolIcon : StageToolBarIcon
    {
        [SerializeField] ToolType _tool;
        [SerializeField] Sprite _icon;
        public ToolType Tool => _tool;
        public Sprite Icon => _icon;
    }
}