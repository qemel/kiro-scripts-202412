using System.Collections.Generic;
using Kiro.Domain;
using UnityEngine;

namespace Kiro.Data
{
    [CreateAssetMenu(fileName = "CellTypePrefabSO", menuName = "CellTypePrefabSO")]
    public sealed class CellTypePrefabSO : ScriptableObject
    {
        [SerializeField] List<CellTypePrefabInfo> _cellTypePrefabInfos;

        public CellTypePrefabResult GetPrefab(CellType cellType)
        {
            return _cellTypePrefabInfos.Find(info => info.CellType == cellType) is { } found
                ? new FoundCellTypePrefab(found.Prefab)
                : new NotFoundCellTypePrefab();
        }
    }
}