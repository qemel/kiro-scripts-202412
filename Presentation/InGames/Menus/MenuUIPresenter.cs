using Cysharp.Threading.Tasks;
using Kiro.Application;
using R3;
using VContainer.Unity;

namespace Kiro.Presentation.Menus
{
    public sealed class MenuUIPresenter : ControllerBase, IStartable
    {
        readonly MenuUIView _menuUIView;
        readonly GameSceneLoader _gameSceneLoader;

        public MenuUIPresenter(MenuUIView menuUIView, GameSceneLoader gameSceneLoader)
        {
            _menuUIView = menuUIView;
            _gameSceneLoader = gameSceneLoader;
        }

        public void Start()
        {
            _menuUIView
                .ButtonClickedAsObservable(ButtonType.MoveToMapScene)
                .Subscribe(_ => _gameSceneLoader.LoadAsync(SceneName.Map).Forget())
                .AddTo(this);

            _menuUIView
                .ButtonClickedAsObservable(ButtonType.Back)
                .Subscribe(_ => _menuUIView.Deactivate())
                .AddTo(this);
        }
    }
}