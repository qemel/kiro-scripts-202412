using System.Threading;
using Cysharp.Threading.Tasks;
using Kiro.Domain;
using Kiro.Domain.Utils;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.Assertions;

namespace Kiro.Presentation
{
    public sealed class CellView : MonoBehaviour
    {
        [SerializeField] float _margin;
        [SerializeField] float _shaderNoiseIntensity;
        [SerializeField] float _flipAnimationDuration;

        CancellationTokenSource _cancellationTokenSource;
        float _sizeWithMargin;
        SpriteRenderer _spriteRenderer;

        static readonly int ColorProperty = Shader.PropertyToID("_Color");
        static readonly int IntensityProperty = Shader.PropertyToID("_NoiseIntensity");

        public SpriteRenderer SpriteRenderer => _spriteRenderer ??= GetComponent<SpriteRenderer>();
        public Vector2 InitialLocalPosition { get; private set; }
        public Vector2Int MapPosition { get; private set; }
        bool IsPlaying { get; set; }
        bool IsInitialized { get; set; }

        CancellationToken CancellationToken => _cancellationTokenSource.Token;

        /// <summary>
        ///     Factoryから呼ぶ
        /// </summary>
        public void Init(Vector2Int mapPosition, Cell cell)
        {
            if (IsInitialized) return;
            IsInitialized = true;

            Assert.AreEqual(SpriteRenderer.size.x, SpriteRenderer.size.y);

            SetColor(cell.Color);

            transform.position = (Vector2)mapPosition;

            MapPosition = mapPosition;
            _sizeWithMargin = SpriteRenderer.size.x + _margin;
            InitialLocalPosition = transform.localPosition;

            _cancellationTokenSource = new CancellationTokenSource();
        }

        void SetColor(CellColor color)
        {
            SpriteRenderer.color = color.ToDefaultStageColor();
            var rendererComponent = GetComponent<Renderer>();
            var mat = rendererComponent.material;

            // MEMO: 色によって後から分けるのは危険かもしれない
            mat.SetFloat(IntensityProperty, color == CellColor.Black ? _shaderNoiseIntensity : 0.0f);
            mat.SetColor(ColorProperty, color.ToDefaultStageColor());
        }

        public void SetParent(StageMapView parent)
        {
            transform.SetParent(parent.transform, false);
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        /// <summary>
        ///     セルをかえすアニメーションを再生する
        ///     sizeDeltaXを使っているので、CellViewのサイズ変更がある場合は修正が必要
        /// </summary>
        public async UniTask PlayFlipAnimation(CellColor color)
        {
            if (!IsInitialized) return;

            // すでに再生中の場合はキャンセルして再生
            if (IsPlaying)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();

                _cancellationTokenSource = new CancellationTokenSource();
            }

            IsPlaying = true;

            await LMotion
                  .Create(_sizeWithMargin, 0f, _flipAnimationDuration)
                  .WithEase(Ease.InExpo)
                  .BindToLocalScaleX(SpriteRenderer.transform)
                  .ToUniTask(CancellationToken);

            SetColor(color);

            await LMotion
                  .Create(0f, _sizeWithMargin, _flipAnimationDuration)
                  .WithEase(Ease.OutExpo)
                  .BindToLocalScaleX(SpriteRenderer.transform)
                  .ToUniTask(CancellationToken);

            IsPlaying = false;
        }

        void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}