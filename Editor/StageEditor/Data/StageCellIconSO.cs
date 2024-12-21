using System.Collections.Generic;
using System.Linq;
using Kiro.Domain;
using UnityEngine;

namespace Kiro.Editor.StageEditor.Data
{
    [CreateAssetMenu(fileName = "StageCellIconSO", menuName = "StageCellIconSO")]
    public sealed class StageCellIconSO : ScriptableObject
    {
        [SerializeField] StageToolBarTypeIcon[] _typeIcons;
        [SerializeField] StageToolBarColorIcon[] _colorIcons;
        [SerializeField] StageToolBarToolIcon[] _toolIcons;

        public IEnumerable<StageToolBarTypeIcon> TypeIcons => _typeIcons;
        public IEnumerable<StageToolBarColorIcon> ColorIcons => _colorIcons;
        public IEnumerable<StageToolBarToolIcon> ToolIcons => _toolIcons;

        public Sprite GetIcon(CellType type) =>
            (from icon in _typeIcons where icon.Type == type select icon.Icon).FirstOrDefault();

        public Sprite GetIcon(CellColor color) =>
            (from icon in _colorIcons where icon.Color == color select icon.Icon).FirstOrDefault();
    }
}