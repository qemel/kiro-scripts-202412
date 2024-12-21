using System.Threading;
using Cysharp.Threading.Tasks;
using Kiro.Data;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Kiro.Application
{
    /// <summary>
    ///     カットシーンのループ処理
    /// </summary>
    public sealed class CutSceneLoop : IAsyncStartable
    {
        readonly GameSceneLoader _gameSceneLoader;
        readonly StageEventDataAllSO _stageEventDataAllSO;
        readonly ILevelLoader _levelLoader;

        public CutSceneLoop(
            GameSceneLoader gameSceneLoader, StageEventDataAllSO stageEventDataAllSO,
            ILevelLoader levelLoader
        )
        {
            _gameSceneLoader = gameSceneLoader;
            _stageEventDataAllSO = stageEventDataAllSO;
            _levelLoader = levelLoader;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            var stageId = _levelLoader.Load();
            var stageEventOfLevelInfo = _stageEventDataAllSO
                                        .OfWorldId(stageId.WorldId)
                                        .OfLevel(stageId.Level);

            var timeline = Object.Instantiate(stageEventOfLevelInfo.OnEnterTimelinePlayer);
            await timeline.PlayAsync(cancellation);

            await _gameSceneLoader.LoadAsync(SceneName.InGame);
        }
    }
}