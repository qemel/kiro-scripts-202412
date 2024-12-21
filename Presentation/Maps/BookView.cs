using System.Collections.Generic;
using System.Threading;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Kiro.Application;
using Kiro.Domain;
using R3;
using UnityEngine;

namespace Kiro.Presentation
{
    /// <summary>
    ///     本を表す
    ///     ページの集合体
    /// </summary>
    public sealed class BookView : MonoBehaviour
    {
        [SerializeField] [ReadOnly] BookPageView[] _bookPageViews;

        readonly Dictionary<Page, BookPageView> _bookPageViewDictionary = new();

        public Observable<PageChangeCommand> OnPageChangeAsObservable => _onPageChangeAsObservable;
        readonly Subject<PageChangeCommand> _onPageChangeAsObservable = new();

        public Observable<StageId> OnSelectStageAsObservable => _onSelectStageAsObservable;
        readonly Subject<StageId> _onSelectStageAsObservable = new();

        void Awake()
        {
            _bookPageViews = GetComponentsInChildren<BookPageView>(true);
        }

        void Start()
        {
            foreach (var bookPageView in _bookPageViews)
            {
                bookPageView
                    .OnLeftPageChangeAsObservable
                    .Subscribe(pageChangeCommand => _onPageChangeAsObservable.OnNext(pageChangeCommand))
                    .AddTo(this);

                bookPageView
                    .OnSelectStageAsObservable
                    .Subscribe(stageId => _onSelectStageAsObservable.OnNext(stageId))
                    .AddTo(this);

                _bookPageViewDictionary.Add(bookPageView.Page, bookPageView);
            }
        }

        /// <summary>
        ///     ページ移動
        /// </summary>
        /// <param name="leftPage"></param>
        /// <param name="token"></param>
        public async UniTask MovePage(Page leftPage, CancellationToken token)
        {
            if (leftPage.Value < 0 || leftPage.Value >= _bookPageViews.Length || leftPage.Value % 2 == 0)
                GameLog.ExecuteWarning($"ページが存在しないか、左ページではありません: {leftPage}", this, Color.red);

            await UniTask.DelayFrame(1, cancellationToken: token);

            foreach (var bookPageView in _bookPageViews)
            {
                bookPageView.SetActive(false);
            }

            // 表示する2ページ
            if (_bookPageViewDictionary.TryGetValue(leftPage, out var leftPageView)) leftPageView.SetActive(true);

            if (_bookPageViewDictionary.TryGetValue(new Page(leftPage.Value + 1), out var rightPageView))
                rightPageView.SetActive(true);
        }

#if UNITY_EDITOR
        /// <summary>
        ///     Editor用。ページIDを設定する
        ///     ページIDは0から始まる
        /// </summary>
        [Title("Editor")]
        [Button]
        public void SetEditorPageIds()
        {
            for (var i = 0; i < _bookPageViews.Length; i++)
            {
                _bookPageViews[i].SetEditorPageId(i);

                GameLog.Execute(
                    $"子オブジェクトのページ情報を変更しました: " +
                    $"{_bookPageViews[i].Page}, " +
                    $"ページ:{i}, " +
                    this,
                    Color.white
                );
            }

            GameLog.Execute($"ページ情報を{_bookPageViews.Length}件更新しました", this, Color.green);
        }
#endif
    }
}