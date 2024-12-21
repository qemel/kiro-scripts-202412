using UnityEngine;

namespace Kiro.Presentation
{
    /// <summary>
    ///     Playerの集約クラス
    /// </summary>
    public sealed class PlayerRootView : MonoBehaviour
    {
        [SerializeField] PlayerAnimationView _animation;
        public PlayerAnimationView Animation => _animation;
    }
}