namespace Kiro.Presentation
{
    /// <summary>
    ///     Buttonの種類を表す
    ///     順番変更NG（Unity内の順番も変わるから）
    /// </summary>
    public enum ButtonType
    {
        // General(0~999)
        None = 0,
        Escape,

        // Title(1000~1999)
        MoveToInGameScene = 1000,
        Setting,
        MoveToMapScene,

        // InGame(2000~2999)
        PlayPathAnimation = 2000,
        Undo,
        Retry,

        // Settings(3000~3999)
        Back = 3000
    }
}