using Kiro.Application;
using Kiro.Domain;
using R3;
using VContainer.Unity;

namespace Kiro.Presentation
{
    /// <summary>
    ///     MapのSprite等の管理
    /// </summary>
    public sealed class MapPresenter : ControllerBase, IStartable
    {
        readonly GameSceneLoader _gameSceneLoader;
        readonly ILevelSaver _levelSaver;
        readonly BookView _bookView;
        readonly BookPageModel _bookPageModel;

        public MapPresenter(
            GameSceneLoader gameSceneLoader, ILevelSaver levelSaver, BookView bookView, BookPageModel bookPageModel
        )
        {
            _gameSceneLoader = gameSceneLoader;
            _levelSaver = levelSaver;
            _bookView = bookView;
            _bookPageModel = bookPageModel;
        }

        public void Start()
        {
            // Model -> View
            _bookPageModel
                .CurrentLeftPage
                .SubscribeAwait(
                    async (leftPage, ct) => { await _bookView.MovePage(leftPage, ct); }
                )
                .AddTo(this);

            // View -> Model
            _bookView
                .OnPageChangeAsObservable
                .Subscribe(
                    pageChangeCommand => _bookPageModel.Set(pageChangeCommand.Page)
                )
                .AddTo(this);

            _bookView
                .OnSelectStageAsObservable
                .SubscribeAwait(
                    async (stageId, _) =>
                    {
                        _levelSaver.Save(stageId);
                        await _gameSceneLoader.LoadAsync(SceneName.InGame);
                    },
                    AwaitOperation.Drop
                )
                .AddTo(this);
        }
    }
}