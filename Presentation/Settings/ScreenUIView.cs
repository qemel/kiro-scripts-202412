using R3;
using UnityEngine;

namespace Kiro.Presentation
{
    public sealed class SettingScreenUIView : MonoBehaviour
    {
        [SerializeField] ButtonHolderUIView _buttonHolderUIView;

        public Observable<Unit> ButtonClickedAsObservable(ButtonType type) =>
            _buttonHolderUIView.ClickedAsObservable(type);

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}