using Kiro.Application;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kiro.Presentation
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class SpriteButtonView : MonoBehaviour,
        IPointerClickHandler,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        /// <summary>
        ///     ボタンがクリックされた時、そのボタンの種類を通知する
        /// </summary>
        public Observable<Unit> OnClickAsObservable => _onClickAsObservable;
        readonly Subject<Unit> _onClickAsObservable = new();

        /// <summary>
        ///     ボタンがホバーされた時、そのボタンの種類を通知する
        /// </summary>
        public Observable<Unit> OnPointerEnterAsObservable => _onPointerEnterAsObservable;
        readonly Subject<Unit> _onPointerEnterAsObservable = new();

        /// <summary>
        ///     ボタンがホバーから外れた時、そのボタンの種類を通知する
        /// </summary>
        public Observable<Unit> OnPointerExitAsObservable => _onPointerExitAsObservable;
        readonly Subject<Unit> _onPointerExitAsObservable = new();

        void Start()
        {
            _onClickAsObservable.AddTo(this);
            _onPointerEnterAsObservable.AddTo(this);
            _onPointerExitAsObservable.AddTo(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            GameLog.Execute("Button Clicked", this);
            _onClickAsObservable.OnNext(Unit.Default);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            GameLog.Execute("Button Hover", this);
            _onPointerEnterAsObservable.OnNext(Unit.Default);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GameLog.Execute("Button Hover Exit", this);
            _onPointerExitAsObservable.OnNext(Unit.Default);
        }
    }
}