using System;
using System.Collections.Generic;
using System.Linq;
using Alchemy.Inspector;
using Kiro.Domain;
using UnityEngine;

namespace Kiro.Data.EditMode
{
    [Serializable]
    public sealed record PanelInfo
    {
        [ValidateInput("ValidatePositions", "RelativePositionsの要素が0、もしくは最初が(0,0)でない、もしくは重複しています。")]
        [HelpBox("RelativePositionsは(0,0)を含む相対位置のリストです。かならず先頭は(0,0)にしてください。")]
        [SerializeField] List<Vector2Int> _relativePositions;
        [SerializeField] Vector2 _worldInitPosition;

        public IEnumerable<Vector2Int> RelativePositions => _relativePositions;
        public Vector2 WorldInitPosition => _worldInitPosition;

        public Panel Create() => new(_relativePositions);

        static bool ValidatePositions(List<Vector2Int> list)
        {
            if (list.Count == 0) return false;
            if (list.First() != default) return false;
            if (list.Distinct().Count() != list.Count) return false;

            return true;
        }
    }
}