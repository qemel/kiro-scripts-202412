using System.Collections.Generic;
using UnityEngine;
using VYaml.Annotations;

namespace Kiro.Data
{
    [YamlObject]
    public partial record struct PanelRawData
    {
        public int Id { get; init; }
        public List<Vector2Int> Coordinates { get; init; }
    }
}