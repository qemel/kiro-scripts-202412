using System;

namespace Kiro.Domain
{
    /// <summary>
    ///     パスの結果に関する全ての基底クラス
    /// </summary>
    public abstract record PathResult;

    /// <summary>
    ///     パスがある
    /// </summary>
    /// <param name="Path"></param>
    public abstract record HasPath(Path Path) : PathResult;

    /// <summary>
    ///     経路が存在しない
    /// </summary>
    public sealed record HasNoPath : PathResult;

    /// <summary>
    ///     クリアできる経路がある
    /// </summary>
    public sealed record HasPathClear : HasPath
    {
        public HasPathClear(Path path) : base(path)
        {
            if (path.IsEmpty) throw new ArgumentException("経路が空です。HasNoPathを使ってください。");
        }
    }

    /// <summary>
    ///     クリアできない経路がある
    /// </summary>
    public sealed record HasPathFail : HasPath
    {
        public HasPathFail(Path path) : base(path)
        {
            if (path.IsEmpty) throw new ArgumentException("経路が空です。HasNoPathを使ってください。");
        }
    }
}