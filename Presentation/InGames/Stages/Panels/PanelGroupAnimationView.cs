using UnityEngine;

namespace Kiro.Presentation
{
    public sealed class PanelGroupAnimationView : MonoBehaviour
    {
        [SerializeField] PanelView[] _panelViews;

        public PanelView[] PanelViews => _panelViews;
    }
}