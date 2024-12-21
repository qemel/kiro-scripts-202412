using UnityEngine;

namespace Kiro.Presentation
{
    /// <summary>
    ///     アニメーション用
    /// </summary>
    public sealed class StageItemView : MonoBehaviour
    {
        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void SetParent(StageItemGroupView parent)
        {
            transform.SetParent(parent.transform);
        }
    }
}