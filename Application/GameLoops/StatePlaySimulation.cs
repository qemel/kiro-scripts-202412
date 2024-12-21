using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kiro.Data;
using Kiro.Domain;
using Kiro.Presentation;
using PureFsm;
using UnityEngine;

namespace Kiro.Application.GameLoops
{
    /// <summary>
    ///     Pathのシミュレーションをプレイするステート
    /// </summary>
    public sealed class StatePlaySimulation : IState<GameLoopStateMachine>
    {
        readonly PathAnimationPlayer _pathAnimationPlayer;
        readonly ILevelSaver _levelSaver;
        readonly ILevelLoader _levelLoader;
        readonly PathResultModel _pathResultModel;
        readonly GameSceneLoader _gameSceneLoader;
        readonly StageViewRegistry _stageViewRegistry;
        readonly DialoguePlayer _dialoguePlayer;
        readonly PathView _pathView;

        public StatePlaySimulation(
            PathAnimationPlayer pathAnimationPlayer, ILevelSaver levelSaver,
            PathResultModel pathResultModel, GameSceneLoader gameSceneLoader, StageViewRegistry stageViewRegistry,
            DialoguePlayer dialoguePlayer, ILevelLoader levelLoader, PathView pathView
        )
        {
            _pathAnimationPlayer = pathAnimationPlayer;
            _levelSaver = levelSaver;
            _pathResultModel = pathResultModel;
            _gameSceneLoader = gameSceneLoader;
            _stageViewRegistry = stageViewRegistry;
            _dialoguePlayer = dialoguePlayer;
            _levelLoader = levelLoader;
            _pathView = pathView;
        }

        public async UniTask<int> EnterAsync(CancellationToken token)
        {
            if (_pathResultModel.Value is not HasPath hasPath)
                throw new InvalidOperationException("HasPath以外のPathResultModelが設定されています");

            _pathView.ShowCurrentPath(hasPath, _stageViewRegistry.Value.Map.Origin);

            await _pathAnimationPlayer.ExecuteAsync(hasPath.Path, token);

            _pathView.HidePath();

            // クリア判定
            if (hasPath is HasPathClear)
            {
                GameLog.Execute("クリア", this, Color.green);

                // クリア状況を保存
                var stageId = _levelLoader.Load();

                await _pathAnimationPlayer.ExecuteClearAnimationAsync(token);

                await _stageViewRegistry.Value.AnimateExit(token);

                // とりあえず次のステージに進む（いずれ変える）
                _levelSaver.Save(stageId with { Level = stageId.Level.Next() });

                await _dialoguePlayer.PlayIfPossible(new DialogueSetId(stageId, DialoguePlayTiming.OnClear), token);

                _gameSceneLoader.LoadAsync(SceneName.InGame).Forget();
            }
            else
            {
                GameLog.Execute("失敗", this, Color.red);

                _gameSceneLoader.LoadAsync(SceneName.InGame).Forget();
            }

            return -1;
        }
    }
}