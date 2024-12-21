using Alchemy.Inspector;
using R3;
using UnityEngine;

namespace Kiro.Presentation.Menus
{
    public sealed class MenuUIView : MonoBehaviour
    {
        [SerializeField] [Required] ButtonHolderUIView _buttonHolderUIView;

        public Observable<Unit> ButtonClickedAsObservable(ButtonType type) =>
            _buttonHolderUIView.ClickedAsObservable(type);

        public Observable<Unit> ButtonPointerEnterAsObservable(ButtonType type) =>
            _buttonHolderUIView.PointerEnterAsObservable(type);

        public Observable<Unit> ButtonPointerExitAsObservable(ButtonType type) =>
            _buttonHolderUIView.PointerExitAsObservable(type);

        public void SetButtonInteractable(ButtonType type, bool isInteractable) =>
            _buttonHolderUIView.SetInteractable(type, isInteractable);

        public bool IsActivated => gameObject.activeSelf;

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}