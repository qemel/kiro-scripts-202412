using System.Threading;
using Cysharp.Threading.Tasks;

namespace Kiro.Presentation
{
    public interface IButtonHoverEnterAnimationUIView
    {
        UniTask PlayHoverEnterAnimation(CancellationToken token);
    }
}