using Kiro.Domain;
using VYaml.Annotations;

namespace Kiro.Data
{
    [YamlObject]
    public partial record struct CellVariationRawData
    {
        public CellType Type { get; init; }
        public int SkinId { get; init; }
    }
}