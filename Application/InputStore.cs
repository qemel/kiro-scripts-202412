using R3;
using UnityEngine;

namespace Kiro.Application
{
    /// <summary>
    ///     Inputの状態管理
    /// </summary>
    public sealed class InputStore
    {
        public Vector2 Position { get; private set; }

        public Observable<Unit> Select => _select;
        readonly Subject<Unit> _select = new();

        public Observable<Unit> Retry => _retry;
        readonly Subject<Unit> _retry = new();

        public Observable<Unit> Undo => _undo;
        readonly Subject<Unit> _undo = new();

        public Observable<Unit> Escape => _escape;
        readonly Subject<Unit> _escape = new();

        public void SetPosition(Vector2 position) => Position = position;

        public void PublishSelect()
        {
            _select.OnNext(Unit.Default);
            GameLog.Execute("Input Select", this);
        }

        public void PublishRetry()
        {
            _retry.OnNext(Unit.Default);
            GameLog.Execute("Input Retry", this);
        }

        public void PublishUndo()
        {
            _undo.OnNext(Unit.Default);
            GameLog.Execute("Input Undo", this);
        }

        public void PublishEscape()
        {
            _escape.OnNext(Unit.Default);
            GameLog.Execute("Input Escape", this);
        }
    }
}