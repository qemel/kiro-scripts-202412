using R3;
using UnityEngine;

namespace Kiro.Presentation
{
    /// <summary>
    ///     タイトル画面のUIを保持する
    /// </summary>
    public sealed class TitleUIView : MonoBehaviour
    {
        [SerializeField] ButtonHolderUIView _buttonHolderUIView;

        public Observable<Unit> ButtonClickedAsObservable(ButtonType type) =>
            _buttonHolderUIView.ClickedAsObservable(type);

        public void SetButtonInteractable(ButtonType type, bool isInteractable) =>
            _buttonHolderUIView.SetInteractable(type, isInteractable);
    }
}