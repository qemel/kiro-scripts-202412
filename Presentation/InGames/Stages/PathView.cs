using System.Linq;
using Kiro.Domain;
using UnityEngine;

namespace Kiro.Presentation
{
    public sealed class PathView : MonoBehaviour
    {
        [SerializeField] SpriteRenderer _goalCircleSprite;
        [SerializeField] LineRenderer _lineRenderer;

        void Awake()
        {
            _goalCircleSprite.transform.position = new Vector3(1000, 1000, 0);
        }

        /// <summary>
        ///     現在のパスを描画する
        /// </summary>
        /// <param name="hasPath">パス</param>
        /// <param name="originPosition">パスの原点</param>
        public void ShowCurrentPath(HasPath hasPath, Vector2 originPosition)
        {
            gameObject.SetActive(true);
            DrawPath(hasPath.Path, originPosition);
        }

        public void HidePath()
        {
            gameObject.SetActive(false);
        }

        void DrawPath(Path path, Vector2 originPosition)
        {
            _lineRenderer.positionCount = path.Length;

            var points = path.Points.ToArray();
            for (var i = 0; i < path.Length; i++)
            {
                _lineRenderer.SetPosition(i, points[i] + originPosition);
            }

            _goalCircleSprite.transform.position = points[^1] + originPosition;
        }
    }
}