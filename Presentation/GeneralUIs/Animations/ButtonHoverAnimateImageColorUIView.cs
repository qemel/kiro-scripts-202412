using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Kiro.Presentation
{
    [RequireComponent(typeof(AnimateImageColorUIView))]
    public sealed class ButtonHoverAnimateImageColorUIView : MonoBehaviour, IButtonHoverEnterAnimationUIView
    {
        AnimateImageColorUIView _animateImageColorUIView;

        void Awake()
        {
            _animateImageColorUIView = GetComponent<AnimateImageColorUIView>();
        }

        public async UniTask PlayHoverEnterAnimation(CancellationToken token)
        {
            await _animateImageColorUIView.ChangeColorToAsync(token);
        }
    }
}