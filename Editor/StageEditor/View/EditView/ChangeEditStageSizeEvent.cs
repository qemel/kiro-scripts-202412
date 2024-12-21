namespace Kiro.Editor.StageEditor.View.EditView
{
    /// <summary>
    ///     ステージのサイズ変更イベント
    /// </summary>
    /// <param name="IsRow">行か列か。trueなら行、falseなら列</param>
    /// <param name="IsAtFirst">先頭か末尾か。trueなら先頭、falseなら末尾</param>
    /// <param name="IsAdd">追加か削除か。trueなら追加、falseなら削除</param>
    public readonly record struct ChangeEditStageSizeEvent(bool IsRow, bool IsAtFirst, bool IsAdd);
}