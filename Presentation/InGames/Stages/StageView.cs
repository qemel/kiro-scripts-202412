using System.Threading;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Kiro.Domain;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kiro.Presentation
{
    [RequireComponent(typeof(IStageEnterAnimator), typeof(IStageExitAnimator))]
    public sealed class StageView : MonoBehaviour
    {
        [HelpBox(
            $"{nameof(IStageEnterAnimator)}, {nameof(IStageExitAnimator)}を実装したクラスを付ける必要があります（既についている場合は大丈夫）",
            HelpBoxMessageType.Warning
        )]
        [SerializeField] PanelGroupView _panelGroupView;
        [SerializeField] StageMapView _stageMapView;
        [SerializeField] StageItemGroupView _stageItemGroupView;

        IStageEnterAnimator _stageEnterAnimator;
        IStageExitAnimator _stageExitAnimator;

        public PanelGroupView PanelGroup => _panelGroupView;
        public StageMapView Map => _stageMapView;
        public StageItemGroupView ItemGroup => _stageItemGroupView;

        public void Init()
        {
            _panelGroupView.Init();

            _stageEnterAnimator = GetComponentInChildren<IStageEnterAnimator>(true);
            _stageExitAnimator = GetComponentInChildren<IStageExitAnimator>(true);
        }

        public void Flip(Vector2Int position, CellColor color)
        {
            Map.Flip(position, color);
        }

        public void SetParent(StageViewRegistry parent)
        {
            transform.SetParent(parent.transform, false);
        }

        public async UniTask AnimateEnter(CancellationToken token) => await _stageEnterAnimator.AnimateEnter(token);

        public async UniTask AnimateExit(CancellationToken token) => await _stageExitAnimator.AnimateExit(token);
    }
}