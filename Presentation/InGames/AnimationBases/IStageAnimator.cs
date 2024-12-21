using System.Threading;
using Cysharp.Threading.Tasks;

namespace Kiro.Presentation
{
    public interface IStageAnimator
    {
    }

    public interface IStageEnterAnimator : IStageAnimator
    {
        UniTask AnimateEnter(CancellationToken token);
    }

    public interface IStageExitAnimator : IStageAnimator
    {
        UniTask AnimateExit(CancellationToken token);
    }
}