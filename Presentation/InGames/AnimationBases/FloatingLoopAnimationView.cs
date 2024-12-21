using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Kiro.Presentation
{
    public sealed class FloatingLoopAnimationView : MonoBehaviour
    {
        /// <summary>
        ///     localPosition.yの最大値
        /// </summary>
        [SerializeField] float _height;

        /// <summary>
        ///     Tweenの1往復にかかる時間
        /// </summary>
        [SerializeField] float _loopDuration;
        [SerializeField] Ease _ease;

        void Start()
        {
            var startPosition = transform.localPosition;
            LMotion.Create(startPosition.y, startPosition.y + _height, _loopDuration / 2f)
                   .WithLoops(-1, LoopType.Yoyo)
                   .WithEase(_ease)
                   .BindToLocalPositionY(transform)
                   .AddTo(gameObject);
        }
    }
}