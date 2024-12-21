using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Kiro.Presentation
{
    public sealed class CrowAnimationView : MonoBehaviour
    {
        [SerializeField] Image _image;

        public void FlyAnimation()
        {
            LMotion.Create(1f, 0f, 1f)
                   .WithEase(Ease.OutCubic)
                   .BindToColorA(_image)
                   .AddTo(gameObject);
        }
    }
}