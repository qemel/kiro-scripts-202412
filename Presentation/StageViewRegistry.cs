using UnityEngine;

namespace Kiro.Presentation
{
    public sealed class StageViewRegistry : MonoBehaviour
    {
        StageView _value;

        /// <summary>
        ///     Stage生成時にStageViewが生成され、取得できるようになる
        /// </summary>
        public StageView Value => _value ??= GetComponentInChildren<StageView>();
    }
}