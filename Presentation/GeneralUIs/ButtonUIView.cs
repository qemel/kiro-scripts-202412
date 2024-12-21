using AnnulusGames.LucidTools.Audio;
using Kiro.Application;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Kiro.Presentation
{
    /// <summary>
    ///     UI.Buttonを利用し、より使いやすいObservableとしてイベントを提供する
    ///     また、Animationを再生する実行部も持つ
    /// </summary>
    [RequireComponent(typeof(Button), typeof(ButtonAnimationUIView))]
    public sealed class ButtonUIView : MonoBehaviour
    {
        /// <summary>
        ///     Buttonの種類をInspectorから設定する
        /// </summary>
        [SerializeField] ButtonType _buttonType;

        /// <summary>
        ///     クリックされた時のSE
        /// </summary>
        [SerializeField] AudioClip _audioClip;

        Button _button;
        ButtonAnimationUIView _buttonAnimationUIView;

        public ButtonType ButtonType => _buttonType;

        public Observable<ButtonType> OnClickAsObservable => _onClickAsObservable;
        readonly Subject<ButtonType> _onClickAsObservable = new();

        public Observable<ButtonType> OnPointerEnterAsObservable => _onPointerEnterAsObservable;
        readonly Subject<ButtonType> _onPointerEnterAsObservable = new();

        public Observable<ButtonType> OnPointerExitAsObservable => _onPointerExitAsObservable;
        readonly Subject<ButtonType> _onPointerExitAsObservable = new();

        void Awake()
        {
            _button = GetComponent<Button>();
            _buttonAnimationUIView = GetComponent<ButtonAnimationUIView>();
        }

        void Start()
        {
            _button.OnClickAsObservable()
                   .SubscribeAwait(
                       async (x, token) =>
                       {
                           GameLog.Execute($"Button Clicked : {_buttonType}", this);
                           _onClickAsObservable.OnNext(_buttonType);

                           await _buttonAnimationUIView.PlayClickAnimation(token);
                       },
                       AwaitOperation.Switch
                   )
                   .AddTo(this);

            _button.OnPointerEnterAsObservable()
                   .SubscribeAwait(
                       async (x, token) =>
                       {
                           GameLog.Execute($"Button Hover : {_buttonType}", this);
                           _onPointerEnterAsObservable.OnNext(_buttonType);

                           await _buttonAnimationUIView.PlayHoverEnterAnimation(token);
                       },
                       AwaitOperation.Switch
                   )
                   .AddTo(this);

            _button.OnPointerExitAsObservable()
                   .SubscribeAwait(
                       async (x, token) =>
                       {
                           GameLog.Execute($"Button Hover Exit : {_buttonType}", this);
                           _onPointerExitAsObservable.OnNext(_buttonType);

                           await _buttonAnimationUIView.PlayHoverEndAnimation(token);
                       },
                       AwaitOperation.Switch
                   )
                   .AddTo(this);

            // SE再生(雑コード)
            _onClickAsObservable
                .Subscribe(
                    _ => { LucidAudio.PlaySE(_audioClip); }
                )
                .AddTo(this);

            _onClickAsObservable.AddTo(this);
            _onPointerEnterAsObservable.AddTo(this);
            _onPointerExitAsObservable.AddTo(this);
        }

        public void SetInteractable(bool isInteractable)
        {
            _button.interactable = isInteractable;
            GameLog.Execute(isInteractable.ToString(), this);
        }
    }
}