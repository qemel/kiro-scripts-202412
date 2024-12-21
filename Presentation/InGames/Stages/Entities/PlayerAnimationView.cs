using System;
using System.Threading;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Kiro.Application;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.Assertions;

namespace Kiro.Presentation
{
    public sealed class PlayerAnimationView : MonoBehaviour
    {
        [SerializeField] Animator _animator;

        [LabelText("1ステップの移動時間(sec)")]
        [SerializeField] float _movePerStepDuration = 0.3f;

        public void Play(PlayerAnimationType type)
        {
            _animator.Play(type.Name);
        }

        public async UniTask PlayStageClearAnimationAsync(CancellationToken token)
        {
            Play(PlayerAnimationType.Clear);
            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
            Play(PlayerAnimationType.Idle);
        }

        public async UniTask PlayMoveWithAnimationAsync(Vector2 movement, CancellationToken token)
        {
            Assert.AreEqual(movement.magnitude, 1);

            GameLog.Create()
                   .WithMessage($"移動方向: {movement}")
                   .WithLocation(this)
                   .Log();

            var type = movement.ToPlayerAnimationType();

            Play(type);

            var position = transform.position;

            await LMotion.Create(position, position + (Vector3)movement, _movePerStepDuration)
                         .WithEase(Ease.Linear)
                         .BindToPosition(transform)
                         .ToUniTask(token);
        }
    }
}