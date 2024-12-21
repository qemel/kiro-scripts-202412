using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Kiro.Presentation
{
    /// <summary>
    ///     アニメーション無し
    /// </summary>
    public sealed class StageExitAnimatorNone : MonoBehaviour, IStageExitAnimator
    {
        public UniTask AnimateExit(CancellationToken token) => UniTask.CompletedTask;
    }
}