using Kiro.Domain;

namespace Kiro.Data
{
    public readonly record struct DialogueSetId(StageId StageId, DialoguePlayTiming DialoguePlayTiming);

    /// <summary>
    ///     Dialogueの再生タイミング
    /// </summary>
    public enum DialoguePlayTiming
    {
        OnInitializeStart,
        OnInitializeFinish,
        OnClear
    }
}