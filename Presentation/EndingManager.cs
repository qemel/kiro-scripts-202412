using AnnulusGames.LucidTools.Audio;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Kiro.Presentation
{
    public sealed class EndingManager : MonoBehaviour
    {
        [SerializeField] Button _startButton;
        [SerializeField] TitleLogoAnimatorView _titleLogoAnimatorView;
        [SerializeField] PlayerAnimationView _playerAnimationView;
        [SerializeField] AudioClip _seWalking;
        [SerializeField] AudioClip _seStart;
        [SerializeField] AudioClip _seClear;
        [SerializeField] AudioClip _bgm;

        async UniTaskVoid Start()
        {
            _titleLogoAnimatorView.SetActive(false);

            var token = this.GetCancellationTokenOnDestroy();
            // await _startButton.GetComponent<ButtonUIView>().FadeOutAsync(0f, token);

            LucidAudio.PlaySE(_seWalking).SetTimeSamples();

            await UniTask.Delay(1000, cancellationToken: token);

            for (var i = 0; i < 7; i++)
            {
                // await _playerAnimationView.PlayMoveWithAnimationAsync(0.6f, StageSettings.GridSize * Vector2.left, token);
            }

            _playerAnimationView.Play(PlayerAnimationType.Idle);

            await UniTask.Delay(1000, cancellationToken: token);

            LucidAudio.PlaySE(_seClear).SetTimeSamples();
            await _playerAnimationView.PlayStageClearAnimationAsync(token);

            await UniTask.Delay(1000, cancellationToken: token);

            LucidAudio.PlayBGM(_bgm).SetVolume(0.3f).SetLoop();

            // titleと同じ
            {
                await _titleLogoAnimatorView.PlayAnimationAsync(token);

                // await _startButton.GetComponent<ButtonUIView>().FadeInAsync(1f, token);

                _startButton.interactable = true;

                await _startButton.OnClickAsync(token);

                _titleLogoAnimatorView.PlayFadeOutAnimationAsync(token).Forget();
                LucidAudio.PlaySE(_seStart).SetTimeSamples();

                // GameSceneLoader.LoadInGameAsync(GameSceneLoader.SceneName.InGame);
            }
        }
    }
}