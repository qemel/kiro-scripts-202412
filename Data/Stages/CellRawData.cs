using Kiro.Domain;
using VYaml.Annotations;

namespace Kiro.Data
{
    [YamlObject]
    public partial record struct CellRawData
    {
        public CellVariationRawData Type { get; init; }
        public CellColor StartColor { get; init; }
    }
}