using System;

namespace Kiro.Domain
{
    /// <summary>
    ///     ページ番号
    /// </summary>
    /// <param name="Value"></param>
    public readonly record struct Page
    {
        public int Value { get; }

        public Page(int value)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), value, "ページ番号は0以上である必要があります");

            Value = value;
        }

        public Page Next() => new(Value + 1);
        public Page Previous() => new(Value - 1);

        /// <summary>
        ///     これ書かないとNextとかが一生呼ばれてStackOverflowExceptionが発生する
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Value.ToString();
    }
}