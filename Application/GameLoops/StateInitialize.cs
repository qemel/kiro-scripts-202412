using System.Threading;
using Cysharp.Threading.Tasks;
using Kiro.Data;
using Kiro.Presentation;
using PureFsm;

namespace Kiro.Application.GameLoops
{
    /// <summary>
    ///     ゲームの初期化ステート
    /// </summary>
    public sealed class StateInitialize : IState<GameLoopStateMachine>
    {
        readonly ILevelLoader _levelLoader;
        readonly GameInitializer _gameInitializer;
        readonly InGameUIView _inGameUIView;
        readonly StageViewRegistry _stageViewRegistry;
        readonly DialoguePlayer _dialoguePlayer;

        public StateInitialize(
            ILevelLoader levelLoader, GameInitializer gameInitializer, InGameUIView inGameUIView,
            StageViewRegistry stageViewRegistry, DialoguePlayer dialoguePlayer
        )
        {
            _levelLoader = levelLoader;
            _gameInitializer = gameInitializer;
            _inGameUIView = inGameUIView;
            _stageViewRegistry = stageViewRegistry;
            _dialoguePlayer = dialoguePlayer;
        }

        public async UniTask<int> EnterAsync(CancellationToken token)
        {
            var stageId = _levelLoader.Load();

            await _dialoguePlayer.PlayIfPossible(
                new DialogueSetId(stageId, DialoguePlayTiming.OnInitializeStart),
                token
            );

            _gameInitializer.Execute(stageId);
            _inGameUIView.SetStageName(stageId);

            await _stageViewRegistry.Value.AnimateEnter(token);

            await _dialoguePlayer.PlayIfPossible(
                new DialogueSetId(stageId, DialoguePlayTiming.OnInitializeFinish),
                token
            );

            // コンパイラに警告を出させないために入れてるだけなので問題になったら消していい（ならんと思うけど）
            await UniTask.Yield();

            return (int)GameLoopStateEvent.InitializeFinished;
        }
    }
}