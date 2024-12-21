using UnityEngine;

namespace Kiro.Presentation
{
    public sealed class StageItemGroupView : MonoBehaviour
    {
        PlayerRootView _player;
        StageItemView[] _items;
        public PlayerRootView Player => _player ??= GetComponentInChildren<PlayerRootView>(true);
        public StageItemView[] Items => _items ??= GetComponentsInChildren<StageItemView>(true);
    }
}