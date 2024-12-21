using System;
using UnityEngine;

namespace Kiro.Data
{
    [Serializable]
    public class StageEventOfWorldInfo
    {
        [SerializeField] StageEventDataOfWorldSO _stageEventDataOfWorldSO;
        public StageEventDataOfWorldSO StageEventDataOfWorldSO => _stageEventDataOfWorldSO;
    }
}