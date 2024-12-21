using UnityEngine;

namespace Kiro.Presentation
{
    /// <summary>
    ///     Stageの背景オブジェクトを管理するクラス
    /// </summary>
    public sealed class StageBackgroundView : MonoBehaviour
    {
        public void SetParent(StageView parent)
        {
            transform.SetParent(parent.transform, false);
        }
    }
}