using Kiro.Domain;
using TMPro;
using UnityEngine;

namespace Kiro.Presentation
{
    public sealed class StageNameTextUIView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _titleLabel;

        public void Set(StageId id)
        {
            _titleLabel.text = $"帰路 {id.WorldId.Value} - {id.Level.Value}";
        }
    }
}