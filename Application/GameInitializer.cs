using System;
using System.Threading;
using Kiro.Data.EditMode;
using Kiro.Domain;
using Kiro.Presentation;

namespace Kiro.Application
{
    /// <summary>
    ///     ゲーム全体の初期化
    /// </summary>
    public sealed class GameInitializer : IDisposable
    {
        readonly StageCamera _stageCamera;
        readonly AllWorldsDataSO _allWorldsDataSO;
        readonly StageFactory _stageFactory;

        readonly CancellationTokenSource _cancellationTokenSource = new();

        public GameInitializer(StageFactory stageFactory, StageCamera stageCamera, AllWorldsDataSO allWorldsDataSO)
        {
            _stageFactory = stageFactory;
            _stageCamera = stageCamera;
            _allWorldsDataSO = allWorldsDataSO;
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        public void Execute(StageId id)
        {
            _stageFactory.Create(id);
            _stageCamera.SetInitPosition(_allWorldsDataSO.GetStageData(id).Size);
        }
    }
}