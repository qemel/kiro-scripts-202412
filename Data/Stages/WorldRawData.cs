using VYaml.Annotations;

namespace Kiro.Data
{
    [YamlObject]
    public partial record struct WorldRawData
    {
        public int[] StageIds { get; init; }
    }
}