using System;
using Kiro.Domain;
using UnityEngine;

namespace Kiro.Data.EditMode
{
    [Serializable]
    public record struct CellInfo
    {
        [SerializeField] CellColor _color;
        [SerializeField] CellType _type;

        public CellInfo(CellColor color, CellType type)
        {
            _color = color;
            _type = type;
        }

        public CellInfo() : this(CellColor.Empty, CellType.Normal)
        {
        }

        public CellColor Color => _color;
        public CellType Type => _type;

        public Cell Create() => new(_color, _type);
    }
}