using System;
using Kiro.Domain;
using R3;

namespace Kiro.Application
{
    /// <summary>
    ///     ゲーム中のパスの結果を保持するモデル
    /// </summary>
    public sealed class PathResultModel : IDisposable
    {
        public ReadOnlyReactiveProperty<PathResult> ReactiveProperty => _reactiveProperty;
        readonly ReactiveProperty<PathResult> _reactiveProperty = new(new HasNoPath());

        public PathResult Value => _reactiveProperty.Value;

        public void Set(PathResult pathResult)
        {
            _reactiveProperty.Value = pathResult;
        }

        public void Dispose()
        {
            _reactiveProperty?.Dispose();
        }
    }
}