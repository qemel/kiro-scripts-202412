using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Kiro.Presentation
{
    public sealed class ButtonHoverEndAnimateImageColorUIView : MonoBehaviour, IButtonHoverEndAnimationUIView
    {
        AnimateImageColorUIView _animateImageColorUIView;

        void Awake()
        {
            _animateImageColorUIView = GetComponent<AnimateImageColorUIView>();
        }

        public async UniTask PlayHoverEndAnimation(CancellationToken token)
        {
            await _animateImageColorUIView.ChangeColorFromAsync(token);
        }
    }
}