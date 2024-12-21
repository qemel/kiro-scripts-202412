using System;

namespace Kiro.Domain
{
    /// <summary>
    ///     World内でのレベル
    /// </summary>
    public readonly record struct LevelInWorldId
    {
        public LevelInWorldId(int value)
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value), "レベルは0以上である必要があります");

            Value = value;
        }

        public int Value { get; }

        public LevelInWorldId Next() => new(Value + 1);
    }
}