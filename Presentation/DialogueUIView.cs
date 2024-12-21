using System.Threading;
using Cysharp.Threading.Tasks;
using Kiro.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kiro.Presentation
{
    public sealed class DialogueUIView : MonoBehaviour
    {
        [SerializeField] Image _speakerIconImage;
        [SerializeField] TextMeshProUGUI _speakerText;
        [SerializeField] TextMeshProUGUI _messageText;

        public async UniTask ShowAsync(Dialogue dialogue, CancellationToken token)
        {
            gameObject.SetActive(true);

            _speakerIconImage.sprite = dialogue.SpeakerIcon;
            _speakerText.text = dialogue.Speaker;
            _messageText.text = dialogue.Message;

            // アニメーションとかしたくなったらここに
            await UniTask.Yield();
        }

        public void Hide()
        {
            gameObject.SetActive(false);

            _speakerText.text = "";
            _messageText.text = "";
        }
    }
}