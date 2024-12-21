using System.Collections.Generic;
using VYaml.Annotations;

namespace Kiro.Data
{
    [YamlObject]
    public partial record struct StageRawData
    {
        public int Id { get; init; }

        public int Height { get; init; }
        public int Width { get; init; }

        /// <summary>
        ///     Height * Width個のセルデータ
        /// </summary>
        public CellRawData[,] Cells { get; init; }

        public List<PanelRawData> Panels { get; init; }

        /// <summary>
        ///     次のステージのノードをグラフとして繋ぐためのIDリスト(クリア時にこのステージが解放され得る)
        /// </summary>
        public List<int> NextStageIds { get; init; }
    }
}