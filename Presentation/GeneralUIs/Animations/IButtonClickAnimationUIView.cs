using System.Threading;
using Cysharp.Threading.Tasks;

namespace Kiro.Presentation
{
    public interface IButtonClickAnimationUIView
    {
        UniTask PlayClickAnimation(CancellationToken token);
    }
}