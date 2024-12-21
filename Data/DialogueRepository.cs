namespace Kiro.Data
{
    public sealed class DialogueRepository
    {
        readonly DialogueAllDataSO _dialogueAllDataSO;

        public DialogueRepository(DialogueAllDataSO dialogueAllDataSO)
        {
            _dialogueAllDataSO = dialogueAllDataSO;
        }

        public DialogueEventSet Get(DialogueSetId setId) => _dialogueAllDataSO.Get(setId);
    }
}