using System;
using UnityEngine;

namespace Kiro.Data
{
    /// <summary>
    ///     対話データ
    /// </summary>
    [Serializable]
    public sealed record Dialogue
    {
        public string Speaker => _speaker;
        public Sprite SpeakerIcon => _speakerIcon;
        public string Message => _message;

        [SerializeField] string _speaker;
        [SerializeField] Sprite _speakerIcon;
        [SerializeField] string _message;
    }
}