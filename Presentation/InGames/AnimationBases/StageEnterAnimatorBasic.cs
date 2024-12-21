using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Kiro.Presentation
{
    public sealed class StageEnterAnimatorBasic : MonoBehaviour, IStageEnterAnimator
    {
        StageItemGroupView _stageItemGroupView;
        StageMapView _stageMapView;

        [SerializeField] float _duration = 2f;
        [SerializeField] float _movementDelayPerCell = 0.05f;
        [SerializeField] float _startYHeight = 7f;
        [SerializeField] Ease _ease = Ease.OutCubic;

        void Awake()
        {
            _stageItemGroupView = GetComponentInChildren<StageItemGroupView>(true);
            _stageMapView = GetComponentInChildren<StageMapView>(true);
        }

        public async UniTask AnimateEnter(CancellationToken token)
        {
            foreach (var cell in _stageMapView.Cells)
            {
                cell.transform.localPosition = new Vector3(cell.transform.localPosition.x, -_startYHeight, 0);
            }

            _ = LMotion.Create(-_startYHeight, 0, _duration)
                       .WithEase(_ease)
                       .BindToLocalPositionY(_stageMapView.transform)
                       .AddTo(gameObject);

            _ = LMotion.Create(_startYHeight, 0, _duration)
                       .WithEase(_ease)
                       .BindToLocalPositionY(_stageItemGroupView.transform)
                       .AddTo(gameObject);

            foreach (var cell in _stageMapView.Cells)
            {
                await UniTask.WaitForSeconds(_movementDelayPerCell, cancellationToken: token);

                // if last
                if (cell == _stageMapView.Cells.Last())
                {
                    await LMotion.Create(-_startYHeight, cell.InitialLocalPosition.y, _duration)
                                 .WithEase(_ease)
                                 .BindToLocalPositionY(cell.transform)
                                 .AddTo(gameObject);
                }
                else
                {
                    _ = LMotion.Create(-_startYHeight, cell.InitialLocalPosition.y, _duration)
                               .WithEase(_ease)
                               .BindToLocalPositionY(cell.transform)
                               .AddTo(gameObject);
                }
            }
        }
    }
}