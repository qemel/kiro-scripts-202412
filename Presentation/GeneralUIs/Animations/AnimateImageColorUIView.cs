using System.Threading;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Kiro.Presentation
{
    /// <summary>
    ///     Imageの色をアニメーションで変更する
    ///     ButtonUIView経由でTryGetComponentしてもOK
    /// </summary>
    public sealed class AnimateImageColorUIView : MonoBehaviour
    {
        [SerializeField] [Required] Image _image;
        [SerializeField] float _duration;
        [SerializeField] Color _colorTo;
        [SerializeField] Ease _ease;

        Color _colorFrom;

        void Awake()
        {
            _colorFrom = _image.color;
        }

        /// <summary>
        ///     色変更(From -> To)
        /// </summary>
        public async UniTask ChangeColorToAsync(CancellationToken token)
        {
            var color = _image.color;
            await LMotion.Create(color, _colorTo, _duration)
                         .WithEase(_ease)
                         .BindToColor(_image)
                         .ToUniTask(token);
        }

        /// <summary>
        ///     色変更(To -> From)
        ///     逆再生
        /// </summary>
        public async UniTask ChangeColorFromAsync(CancellationToken token)
        {
            var color = _image.color;
            await LMotion.Create(color, _colorFrom, _duration)
                         .WithEase(_ease)
                         .BindToColor(_image)
                         .ToUniTask(token);
        }
    }
}