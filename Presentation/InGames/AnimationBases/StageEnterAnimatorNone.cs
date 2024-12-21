using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Kiro.Presentation
{
    /// <summary>
    ///     アニメーション無し
    /// </summary>
    public sealed class StageEnterAnimatorNone : MonoBehaviour, IStageEnterAnimator
    {
        public UniTask AnimateEnter(CancellationToken token) => UniTask.CompletedTask;
    }
}