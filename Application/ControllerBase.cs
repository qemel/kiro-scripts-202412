using System;
using R3;

namespace Kiro.Application
{
    /// <summary>
    ///     Presenterの基底クラス
    ///     簡単にAddTo(this)でDisposeを管理できるようになる
    /// </summary>
    public class ControllerBase : IDisposable
    {
        readonly CompositeDisposable _disposable = new();

        public void Dispose() => _disposable.Dispose();

        public void AddDisposable(IDisposable item) => _disposable.Add(item);
    }

    public static class DisposableExtensions
    {
        public static T AddTo<T>(this T disposable, ControllerBase controller) where T : IDisposable
        {
            controller.AddDisposable(disposable);
            return disposable;
        }
    }
}