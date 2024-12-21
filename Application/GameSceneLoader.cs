using System;
using System.Threading;
using AnnulusGames.SceneSystem;
using Cysharp.Threading.Tasks;
using Kiro.Presentation;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Kiro.Application
{
    /// <summary>
    ///     ゲームシーンのロード処理
    ///     CancellationTokenSourceを持って自立して動く
    /// </summary>
    public sealed class GameSceneLoader
    {
        readonly LoadingScreen _loadingScreen;

        CancellationTokenSource _cts = new();

        public GameSceneLoader(LoadingScreen loadingScreen)
        {
            _loadingScreen = loadingScreen;
        }

        /// <summary>
        ///     シーンをロードする
        /// </summary>
        /// <param name="sceneName"></param>
        public async UniTask LoadAsync(SceneName sceneName)
        {
            var loadingScreen = await PrepareLoadingScreenAsync();

            await Scenes.LoadSceneAsync(sceneName.ToString())
                        .ToUniTask();

            await DestroyLoadingScreenAsync(loadingScreen);
        }

        /// <summary>
        ///     CutSceneを再生したのちにStageシーンをロードする
        /// </summary>
        /// <param name="timelinePlayer"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public async UniTask LoadStageWithCutSceneAsync(TimelinePlayer timelinePlayer)
        {
            if (timelinePlayer == null) throw new ArgumentNullException(nameof(timelinePlayer));

            var loadingScreen = await PrepareLoadingScreenAsync();

            await Scenes.LoadSceneAsync(SceneName.InGame.ToString())
                        .ToUniTask();

            await timelinePlayer.PlayAsync(_cts.Token);

            await DestroyLoadingScreenAsync(loadingScreen);
        }

        public async UniTask LoadSettingsAdditiveAsync()
        {
            await Scenes.LoadSceneAsync(SceneName.Settings.ToString(), LoadSceneMode.Additive)
                        .ToUniTask(_cts.Token);

            Time.timeScale = 0;
        }

        public async UniTask UnloadSettingsAsync()
        {
            await Scenes.UnloadSceneAsync(SceneName.Settings.ToString())
                        .ToUniTask(_cts.Token);

            Time.timeScale = 1;
        }

        /// <summary>
        ///     タスクのキャンセル
        /// </summary>
        public void Cancel()
        {
            _cts.Dispose();
            _cts.Cancel();
            _cts = new CancellationTokenSource();
        }

        async UniTask<LoadingScreen> PrepareLoadingScreenAsync()
        {
            var loadingScreen = Object.Instantiate(_loadingScreen);
            Object.DontDestroyOnLoad(loadingScreen);

            var loadingScreenFade = loadingScreen.GetComponent<LoadingScreenFade>();
            if (loadingScreenFade == null) throw new Exception("LoadingScreenFade is not found.");

            await loadingScreenFade.FadeInAsync(_cts.Token);

            return loadingScreen;
        }

        async UniTask DestroyLoadingScreenAsync(LoadingScreen loadingScreen)
        {
            var loadingScreenFade = loadingScreen.GetComponent<LoadingScreenFade>();
            if (loadingScreenFade == null) throw new Exception("LoadingScreenFade is not found.");

            await loadingScreenFade.FadeOutAsync(_cts.Token);

            await UniTask.DelayFrame(1, cancellationToken: _cts.Token);

            Object.Destroy(loadingScreen.gameObject);
        }
    }
}