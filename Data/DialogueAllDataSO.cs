using System.Linq;
using UnityEngine;

namespace Kiro.Data
{
    [CreateAssetMenu(fileName = "DialogueData", menuName = "u1w202408/DialogueData")]
    public sealed class DialogueAllDataSO : ScriptableObject
    {
        [SerializeField] DialogueEventSet[] _dialogueEventSets;

        /// <summary>
        ///     対話イベントセットをIDから取得する
        /// </summary>
        public DialogueEventSet Get(DialogueSetId setId)
        {
            return _dialogueEventSets.FirstOrDefault(set => set.Id == setId);
        }
    }
}