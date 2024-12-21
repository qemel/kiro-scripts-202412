using System.Threading;
using Cysharp.Threading.Tasks;
using Kiro.Data;
using Kiro.Presentation;
using UnityEngine;

namespace Kiro.Application
{
    public sealed class DialoguePlayer
    {
        readonly DialogueRepository _dialogueRepository;
        readonly DialogueUIView _dialogueUIView;

        public DialoguePlayer(DialogueRepository dialogueRepository, DialogueUIView dialogueUIView)
        {
            _dialogueRepository = dialogueRepository;
            _dialogueUIView = dialogueUIView;
        }

        /// <summary>
        ///     対話イベントセットを再生する
        ///     対話イベントセットが存在しない場合は何もせずにfalseを返す
        /// </summary>
        public async UniTask<bool> PlayIfPossible(DialogueSetId id, CancellationToken token)
        {
            var dialogueEventSet = _dialogueRepository.Get(id);
            if (dialogueEventSet == null) return false;

            foreach (var dialogue in dialogueEventSet.Dialogues)
            {
                await _dialogueUIView.ShowAsync(dialogue, token);
                await UniTask.WaitUntil(() => Input.anyKeyDown, cancellationToken: token);
            }

            _dialogueUIView.Hide();

            return true;
        }
    }
}