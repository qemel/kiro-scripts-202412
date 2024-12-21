using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Kiro.Presentation
{
    public sealed class StageExitAnimatorBasic : MonoBehaviour, IStageExitAnimator
    {
        StageMapView _stageMapView;

        [SerializeField] float _duration = 2f;
        [SerializeField] float _movementDelayPerCell = 0.05f;
        [SerializeField] float _toYHeight = 7f;
        [SerializeField] Ease _ease = Ease.OutCubic;

        void Awake()
        {
            _stageMapView = GetComponentInChildren<StageMapView>(true);
        }

        public async UniTask AnimateExit(CancellationToken token)
        {
            foreach (var cell in _stageMapView.Cells)
            {
                _ = LMotion.Create(cell.InitialLocalPosition.y, -_toYHeight, _duration)
                           .WithEase(_ease)
                           .BindToLocalPositionY(cell.transform)
                           .AddTo(gameObject);

                await UniTask.WaitForSeconds(_movementDelayPerCell, cancellationToken: token);
            }

            _ = LMotion.Create(0, -_toYHeight, _duration)
                       .WithEase(_ease)
                       .BindToLocalPositionY(_stageMapView.transform)
                       .AddTo(gameObject);

            await UniTask.Delay(1000, cancellationToken: token);
        }
    }
}