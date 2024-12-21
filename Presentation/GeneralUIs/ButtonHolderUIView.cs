using System;
using System.Linq;
using Alchemy.Inspector;
using R3;
using UnityEngine;

namespace Kiro.Presentation
{
    /// <summary>
    ///     ボタンをまとめて保持するUIView
    /// </summary>
    public sealed class ButtonHolderUIView : MonoBehaviour
    {
        [SerializeField] ButtonUIView[] _buttonUIViews;

        /// <summary>
        ///     ボタンがクリックされた時、そのボタンの種類を通知する
        /// </summary>
        readonly Subject<ButtonType> _onClickAsObservable = new();

        /// <summary>
        ///     ボタンがホバーされた時、そのボタンの種類を通知する
        /// </summary>
        readonly Subject<ButtonType> _onPointerEnterAsObservable = new();

        /// <summary>
        ///     ボタンがホバーから外れた時、そのボタンの種類を通知する
        /// </summary>
        readonly Subject<ButtonType> _onPointerExitAsObservable = new();

        /// <summary>
        ///     特定のボタンがクリックされたことを通知する
        /// </summary>
        public Observable<Unit> ClickedAsObservable(ButtonType type) =>
            _onClickAsObservable.Where(x => x == type).Select(_ => Unit.Default);

        /// <summary>
        ///     特定のボタンがホバーされたことを通知する
        /// </summary>
        public Observable<Unit> PointerEnterAsObservable(ButtonType type) =>
            _onPointerEnterAsObservable.Where(x => x == type).Select(_ => Unit.Default);

        /// <summary>
        ///     特定のボタンがホバーから外れたことを通知する
        /// </summary>
        public Observable<Unit> PointerExitAsObservable(ButtonType type) =>
            _onPointerExitAsObservable.Where(x => x == type).Select(_ => Unit.Default);

        void Start()
        {
            foreach (var view in _buttonUIViews)
            {
                view.OnClickAsObservable.Subscribe(x => _onClickAsObservable.OnNext(x)).AddTo(gameObject);
                view.OnPointerEnterAsObservable.Subscribe(x => _onPointerEnterAsObservable.OnNext(x)).AddTo(gameObject);
                view.OnPointerExitAsObservable.Subscribe(x => _onPointerExitAsObservable.OnNext(x)).AddTo(gameObject);
            }

            _onClickAsObservable.AddTo(gameObject);
            _onPointerEnterAsObservable.AddTo(gameObject);
            _onPointerExitAsObservable.AddTo(gameObject);
        }

        public void ActivateAll()
        {
            gameObject.SetActive(true);

            foreach (var button in _buttonUIViews)
            {
                button.gameObject.SetActive(true);
                button.SetInteractable(true);
            }
        }

        public void Activate(ButtonType type)
        {
            GetButton(type).gameObject.SetActive(true);
        }

        public void DeactivateAll()
        {
            gameObject.SetActive(false);

            foreach (var button in _buttonUIViews)
            {
                button.gameObject.SetActive(false);
                button.SetInteractable(false);
            }
        }

        public void Deactivate(ButtonType type)
        {
            GetButton(type).gameObject.SetActive(false);
        }

        public void SetInteractable(ButtonType type, bool isInteractable)
        {
            GetButton(type).SetInteractable(isInteractable);
        }

        ButtonUIView GetButton(ButtonType type)
        {
            var button = _buttonUIViews.FirstOrDefault(x => x.ButtonType == type);

            if (button == null) throw new InvalidOperationException($"ButtonUIView not found. type: {type}");

            return button;
        }

        [Title("Debug")]
        [Button]
        public void AddAllButtonsInChildren()
        {
            _buttonUIViews = GetComponentsInChildren<ButtonUIView>(true);
        }
    }
}