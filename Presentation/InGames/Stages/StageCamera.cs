using UnityEngine;

namespace Kiro.Presentation
{
    public sealed class StageCamera : MonoBehaviour
    {
        Camera _camera;
        public Camera Camera => _camera ??= GetComponent<Camera>();

        public void SetInitPosition(Vector2 stageSize)
        {
            transform.position = new Vector3(stageSize.x / 2 - 0.5f, stageSize.y / 2 - 0.5f, -10);
        }
    }
}