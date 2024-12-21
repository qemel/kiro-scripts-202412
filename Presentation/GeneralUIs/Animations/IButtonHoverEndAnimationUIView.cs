using System.Threading;
using Cysharp.Threading.Tasks;

namespace Kiro.Presentation
{
    public interface IButtonHoverEndAnimationUIView
    {
        UniTask PlayHoverEndAnimation(CancellationToken token);
    }
}