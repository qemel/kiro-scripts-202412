using Alchemy.Inspector;
using Kiro.Domain;
using R3;
using UnityEngine;

namespace Kiro.Presentation
{
    public sealed class PageLevelButtonsView : MonoBehaviour
    {
        [SerializeField] [ReadOnly] PageContentLevelView[] _pageContentLevelViews;

        public Observable<StageId> OnClickAsObservable => _onClickAsObservable;
        readonly Subject<StageId> _onClickAsObservable = new();

        void Awake()
        {
            _pageContentLevelViews = GetComponentsInChildren<PageContentLevelView>(true);
        }

        void Start()
        {
            foreach (var buttonLevelSpriteView in _pageContentLevelViews)
            {
                buttonLevelSpriteView
                    .OnClickAsObservable
                    .Subscribe(stageId => _onClickAsObservable.OnNext(stageId))
                    .AddTo(this);
            }
        }
    }
}