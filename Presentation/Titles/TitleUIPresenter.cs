using Kiro.Application;
using Kiro.Data;
using R3;
using VContainer.Unity;

namespace Kiro.Presentation
{
    public sealed class TitleUIPresenter : ControllerBase, IStartable
    {
        readonly TitleUIView _titleUIView;
        readonly GameSceneLoader _gameSceneLoader;
        readonly StageEventDataRepository _stageEventDataRepository;
        readonly ILevelLoader _levelLoader;

        public TitleUIPresenter(
            TitleUIView titleUIView, GameSceneLoader gameSceneLoader, StageEventDataRepository stageEventDataRepository,
            ILevelLoader levelLoader
        )
        {
            _titleUIView = titleUIView;
            _gameSceneLoader = gameSceneLoader;
            _stageEventDataRepository = stageEventDataRepository;
            _levelLoader = levelLoader;
        }

        public void Start()
        {
            _titleUIView
                .ButtonClickedAsObservable(ButtonType.MoveToInGameScene)
                .SubscribeAwait(
                    async (_, _) =>
                    {
                        var stageId = _levelLoader.Load();
                        var timeline = _stageEventDataRepository.GetBy(stageId).OnEnterTimelinePlayer;

                        if (timeline == null)
                            await _gameSceneLoader.LoadAsync(SceneName.InGame);
                        else
                            await _gameSceneLoader.LoadStageWithCutSceneAsync(timeline);
                    },
                    AwaitOperation.Drop
                )
                .AddTo(this);

            _titleUIView
                .ButtonClickedAsObservable(ButtonType.Setting)
                .SubscribeAwait(
                    async (_, _) => { await _gameSceneLoader.LoadSettingsAdditiveAsync(); },
                    AwaitOperation.Drop
                )
                .AddTo(this);
        }
    }
}