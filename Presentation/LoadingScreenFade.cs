using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Kiro.Presentation
{
    public sealed class LoadingScreenFade : MonoBehaviour
    {
        [SerializeField] SpriteRenderer _spriteRenderer;
        [SerializeField] float _duration;

        public async UniTask FadeInAsync(CancellationToken token)
        {
            await LMotion.Create(0f, 1f, _duration)
                         .BindToColorA(_spriteRenderer)
                         .ToUniTask(token);
        }

        public async UniTask FadeOutAsync(CancellationToken token)
        {
            await LMotion.Create(1f, 0f, _duration)
                         .BindToColorA(_spriteRenderer)
                         .ToUniTask(token);
        }
    }
}