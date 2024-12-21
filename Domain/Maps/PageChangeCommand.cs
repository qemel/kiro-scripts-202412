namespace Kiro.Domain
{
    /// <summary>
    ///     ページ変更コマンド
    /// </summary>
    /// <param name="Page">
    ///     変更後のページID
    /// </param>
    public readonly record struct PageChangeCommand(Page Page);
}