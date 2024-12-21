using System.Threading;
using Cysharp.Threading.Tasks;
using Kiro.Application;
using UnityEngine;

namespace Kiro.Presentation
{
    public sealed class ButtonAnimationUIView : MonoBehaviour
    {
        IButtonHoverEnterAnimationUIView _buttonHoverEnterAnimationUIView;
        IButtonHoverEndAnimationUIView _buttonHoverEndAnimationUIView;
        IButtonClickAnimationUIView _buttonClickAnimationUIView;

        void Awake()
        {
            _buttonHoverEnterAnimationUIView = GetComponent<IButtonHoverEnterAnimationUIView>();
            _buttonHoverEndAnimationUIView = GetComponent<IButtonHoverEndAnimationUIView>();
            _buttonClickAnimationUIView = GetComponent<IButtonClickAnimationUIView>();
        }

        public async UniTask PlayHoverEnterAnimation(CancellationToken token)
        {
            if (_buttonHoverEnterAnimationUIView == null)
            {
                GameLog.Execute("IButtonHoverEnterAnimationUIView is null", this);
                return;
            }

            await _buttonHoverEnterAnimationUIView.PlayHoverEnterAnimation(token);
        }

        public async UniTask PlayHoverEndAnimation(CancellationToken token)
        {
            if (_buttonHoverEndAnimationUIView == null)
            {
                GameLog.Execute("IButtonHoverEndAnimationUIView is null", this);
                return;
            }

            await _buttonHoverEndAnimationUIView.PlayHoverEndAnimation(token);
        }

        public async UniTask PlayClickAnimation(CancellationToken token)
        {
            if (_buttonClickAnimationUIView == null)
            {
                GameLog.Execute("IButtonClickAnimationUIView is null", this);
                return;
            }

            await _buttonClickAnimationUIView.PlayClickAnimation(token);
        }
    }
}