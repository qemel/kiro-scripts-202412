using Kiro.Presentation.Menus;
using R3;
using VContainer.Unity;

namespace Kiro.Application.GameLoops
{
    public sealed class GameInputPresenter : ControllerBase, IStartable
    {
        readonly InputStore _inputStore;
        readonly GameSceneLoader _gameSceneLoader;
        readonly UndoPanel _undoPanel;
        readonly MenuUIView _menuUIView;

        public GameInputPresenter(
            InputStore inputStore, GameSceneLoader gameSceneLoader, UndoPanel undoPanel, MenuUIView menuUIView
        )
        {
            _inputStore = inputStore;
            _gameSceneLoader = gameSceneLoader;
            _undoPanel = undoPanel;
            _menuUIView = menuUIView;
        }

        public void Start()
        {
            _inputStore.Retry.SubscribeAwait(
                           async (_, _) => await _gameSceneLoader.LoadAsync(SceneName.InGame),
                           AwaitOperation.Drop
                       )
                       .AddTo(this);

            _inputStore.Undo.Subscribe(_ => _undoPanel.Execute()).AddTo(this);

            _inputStore.Escape.Subscribe(
                           _ =>
                           {
                               if (_menuUIView.IsActivated)
                                   _menuUIView.Deactivate();
                               else
                                   _menuUIView.Activate();
                           }
                       )
                       .AddTo(this);
        }
    }
}