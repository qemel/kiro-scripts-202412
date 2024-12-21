using System;
using Alchemy.Inspector;
using Kiro.Domain;
using Kiro.Presentation;
using UnityEngine;

namespace Kiro.Data
{
    [Serializable]
    public sealed class CellTypePrefabInfo
    {
        [SerializeField] CellType _cellType;
        [SerializeField] [Required] StageItemView _prefab;
        public CellType CellType => _cellType;
        public StageItemView Prefab => _prefab;
    }
}