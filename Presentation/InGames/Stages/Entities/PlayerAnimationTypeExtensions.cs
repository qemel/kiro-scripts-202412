using UnityEngine;

namespace Kiro.Presentation
{
    public static class PlayerAnimationTypeExtensions
    {
        public static PlayerAnimationType ToPlayerAnimationType(this Vector2 movement)
        {
            return movement switch
            {
                { x: > 0 } => PlayerAnimationType.Right,
                { x: < 0 } => PlayerAnimationType.Left,
                { y: > 0 } => PlayerAnimationType.Up,
                { y: < 0 } => PlayerAnimationType.Down,
                _          => PlayerAnimationType.Idle
            };
        }
    }
}