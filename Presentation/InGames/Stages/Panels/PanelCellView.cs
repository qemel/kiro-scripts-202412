using R3;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kiro.Presentation
{
    /// <summary>
    ///     パネルの中の1つ1つのセル
    /// </summary>
    public sealed class PanelCellView : MonoBehaviour, IPointerDownHandler
    {
        readonly Subject<Unit> _clicked = new();
        SpriteRenderer _spriteRenderer;

        public Observable<Unit> Clicked => _clicked;

        public SpriteRenderer SpriteRenderer => _spriteRenderer ??= GetComponent<SpriteRenderer>();

        public void OnPointerDown(PointerEventData eventData)
        {
            _clicked.OnNext(Unit.Default);
        }

        public void SetLocalPosition(Vector2 position)
        {
            transform.localPosition = new Vector3(position.x, position.y, 0);
        }

        public void SetParent(PanelView parent)
        {
            transform.SetParent(parent.transform, false);
        }
    }
}