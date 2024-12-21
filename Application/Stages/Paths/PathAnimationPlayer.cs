using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kiro.Domain;
using Kiro.Presentation;

namespace Kiro.Application
{
    public sealed class PathAnimationPlayer
    {
        readonly StageViewRegistry _stageViewRegistry;

        public PathAnimationPlayer(StageViewRegistry stageViewRegistry)
        {
            _stageViewRegistry = stageViewRegistry;
        }

        /// <summary>
        ///     Pathの通りにアニメーションを再生する
        /// </summary>
        public async UniTask ExecuteAsync(Path path, CancellationToken token)
        {
            var player = _stageViewRegistry.Value.ItemGroup.Player;

            // 最初の座標はプレイヤーの位置そのままなのでスキップ
            var movements = path.Points.Zip(path.Points.Skip(1), (to, from) => from - to);
            foreach (var step in movements)
            {
                await player.Animation.PlayMoveWithAnimationAsync(step, token);
            }

            player.Animation.Play(PlayerAnimationType.Idle);
        }

        public async UniTask ExecuteClearAnimationAsync(CancellationToken token)
        {
            var player = _stageViewRegistry.Value.ItemGroup.Player;
            await player.Animation.PlayStageClearAnimationAsync(token);
        }
    }
}