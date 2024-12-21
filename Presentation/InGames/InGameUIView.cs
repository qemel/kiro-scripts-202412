using Alchemy.Inspector;
using Kiro.Domain;
using R3;
using UnityEngine;

namespace Kiro.Presentation
{
    /// <summary>
    ///     ゲーム中のUIを保持する
    /// </summary>
    public sealed class InGameUIView : MonoBehaviour
    {
        [SerializeField] [Required] ButtonHolderUIView _buttonHolderUIView;
        [SerializeField] [Required] StageNameTextUIView _stageNameTextUIView;

        public Observable<Unit> ButtonClickedAsObservable(ButtonType type) =>
            _buttonHolderUIView.ClickedAsObservable(type);

        public Observable<Unit> ButtonPointerEnterAsObservable(ButtonType type) =>
            _buttonHolderUIView.PointerEnterAsObservable(type);

        public Observable<Unit> ButtonPointerExitAsObservable(ButtonType type) =>
            _buttonHolderUIView.PointerExitAsObservable(type);

        public void SetButtonInteractable(ButtonType type, bool isInteractable) =>
            _buttonHolderUIView.SetInteractable(type, isInteractable);

        public void SetStageName(StageId id) => _stageNameTextUIView.Set(id);
    }
}