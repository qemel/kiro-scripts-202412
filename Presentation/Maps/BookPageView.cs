using Kiro.Application;
using Kiro.Domain;
using R3;
using UnityEngine;

namespace Kiro.Presentation
{
    /// <summary>
    ///     本のページを表す
    /// </summary>
    [RequireComponent(typeof(SpriteButtonView))]
    public sealed class BookPageView : MonoBehaviour
    {
        [SerializeField] int _pageId;
        
        PageLevelButtonsView _pageLevelButtonsView;
        SpriteButtonView _pageChangeSpriteButtonView;

        public Page Page => new(_pageId);

        /// <summary>
        ///     ページの左側のページ変更イベント
        /// </summary>
        public Observable<PageChangeCommand> OnLeftPageChangeAsObservable =>
            _pageChangeSpriteButtonView.OnClickAsObservable.Select(
                _ => new PageChangeCommand(new Page(Mathf.Max(_pageId + (_pageId % 2 == 0 ? -2 : 1), 0)))
            );

        public Observable<StageId> OnSelectStageAsObservable => _pageLevelButtonsView.OnClickAsObservable;

        void Awake()
        {
            _pageChangeSpriteButtonView = GetComponent<SpriteButtonView>();
            _pageLevelButtonsView = GetComponentInChildren<PageLevelButtonsView>(true);
        }

        void Start()
        {
            // Log
            OnLeftPageChangeAsObservable
                .Subscribe(pageChangeCommand => GameLog.Execute(pageChangeCommand.ToString(), this))
                .AddTo(this);
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

#if UNITY_EDITOR
        /// <summary>
        ///     Editor用。ページIDを設定する
        /// </summary>
        /// <param name="pageId"></param>
        public void SetEditorPageId(int pageId)
        {
            _pageId = pageId;
        }
#endif
    }
}