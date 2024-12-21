using System.Collections.Generic;
using PureFsm;
using VContainer.Unity;

namespace Kiro.Application.GameLoops
{
    /// <summary>
    ///     ゲームシーン全体のループ処理
    /// </summary>
    public class GameLoopStateMachine : Fsm<GameLoopStateMachine>, IStartable
    {
        public GameLoopStateMachine(IEnumerable<IState<GameLoopStateMachine>> states) : base(states)
        {
            AddTransition<StateInitialize, StatePlayerCanInteract>((int)GameLoopStateEvent.InitializeFinished);
            AddTransition<StatePlayerCanInteract, StatePlaySimulation>((int)GameLoopStateEvent.PlaySimulation);
        }

        public void Start()
        {
            _ = Run<StateInitialize>();
        }
    }

    public enum GameLoopStateEvent
    {
        InitializeFinished,
        PlaySimulation
    }
}