using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Kiro.Presentation
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public sealed class TitleLogoAnimatorView : MonoBehaviour
    {
        [SerializeField] Image _logoLL;
        [SerializeField] Image _logoLR;
        [SerializeField] Image _logoRL;
        [SerializeField] Image _logoRR;
        [SerializeField] float _duration;
        [SerializeField] Ease _ease;

        Vector2 _logoLLInitialPosition;
        Vector2 _logoLRInitialPosition;
        Vector2 _logoRLInitialPosition;
        Vector2 _logoRRInitialPosition;

        void Awake()
        {
            _logoLLInitialPosition = _logoLL.rectTransform.anchoredPosition;
            _logoLRInitialPosition = _logoLR.rectTransform.anchoredPosition;
            _logoRLInitialPosition = _logoRL.rectTransform.anchoredPosition;
            _logoRRInitialPosition = _logoRR.rectTransform.anchoredPosition;
        }

        public void SetActive(bool value)
        {
            _logoLL.gameObject.SetActive(value);
            _logoLR.gameObject.SetActive(value);
            _logoRL.gameObject.SetActive(value);
            _logoRR.gameObject.SetActive(value);
        }

        public async UniTask PlayAnimationAsync(CancellationToken token)
        {
            SetActive(true);

            _ = LMotion.Create(800f, _logoLLInitialPosition.y, _duration)
                       .WithEase(_ease)
                       .BindToAnchoredPositionY(_logoLL.rectTransform)
                       .AddTo(gameObject);

            _ = LMotion.Create(-800f, _logoLRInitialPosition.y, _duration)
                       .WithEase(_ease)
                       .BindToAnchoredPositionY(_logoLR.rectTransform)
                       .AddTo(gameObject);

            _ = LMotion.Create(800f, _logoRLInitialPosition.y, _duration)
                       .WithEase(_ease)
                       .BindToAnchoredPositionY(_logoRL.rectTransform)
                       .AddTo(gameObject);

            await LMotion.Create(-800f, _logoRRInitialPosition.y, _duration)
                         .WithEase(_ease)
                         .BindToAnchoredPositionY(_logoRR.rectTransform)
                         .ToUniTask(token);
        }

        public async UniTask PlayFadeOutAnimationAsync(CancellationToken token)
        {
            _ = LMotion.Create(1f, 0f, _duration)
                       .WithEase(_ease)
                       .BindToColorA(_logoLL)
                       .AddTo(gameObject);

            _ = LMotion.Create(1f, 0f, _duration)
                       .WithEase(_ease)
                       .BindToColorA(_logoLR)
                       .AddTo(gameObject);

            _ = LMotion.Create(1f, 0f, _duration)
                       .WithEase(_ease)
                       .BindToColorA(_logoRL)
                       .AddTo(gameObject);

            await LMotion.Create(1f, 0f, _duration)
                         .WithEase(_ease)
                         .BindToColorA(_logoRR)
                         .ToUniTask(token);

            SetActive(false);
        }
    }
}