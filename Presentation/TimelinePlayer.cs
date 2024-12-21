using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace Kiro.Presentation
{
    /// <summary>
    ///     PlayableDirectorのラッパークラス
    /// </summary>
    [RequireComponent(typeof(PlayableDirector))]
    public sealed class TimelinePlayer : MonoBehaviour
    {
        PlayableDirector _playableDirector;

        bool _isInitialized;

        void Awake()
        {
            _playableDirector = GetComponent<PlayableDirector>();
            _isInitialized = true;
        }

        /// <summary>
        ///     Timelineを再生し、再生が終了(Stop等も込み)するまで待機する
        ///     完全に最後まで待つと最初の画面にリセットされた状態が見えて不格好なので、ほぼ再生が終わったら止める
        /// </summary>
        public async UniTask PlayAsync(CancellationToken token)
        {
            if (!_isInitialized) Awake();

            _playableDirector.Play();

            await UniTask.WaitUntil(
                () => _playableDirector.duration - _playableDirector.time < 0.1f,
                cancellationToken: token
            );

            Pause();
        }

        public void Stop()
        {
            _playableDirector.Stop();
        }

        public void Pause()
        {
            _playableDirector.Pause();
        }

        public void Resume()
        {
            _playableDirector.Resume();
        }
    }
}