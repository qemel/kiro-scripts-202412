using System;
using Kiro.Application;
using Kiro.Data.EditMode;
using Kiro.Editor.StageEditor.Data;
using Kiro.Editor.StageEditor.View.EditView;
using R3;
using UnityEngine;

namespace Kiro.Editor.StageEditor.Model
{
    public sealed class StageDataModel : IDisposable
    {
        /// <summary>
        ///     StageDataの変更をReactivePropertyで検知できなかったので、Subjectで代用
        /// </summary>
        readonly Subject<StageData> _changed = new();

        StageData _current;

        public Observable<StageData> Changed => _changed;

        public void Dispose()
        {
            _changed?.Dispose();
        }

        public void Set(StageData stageData)
        {
            _current = stageData;
            _changed.OnNext(stageData);
        }

        public void SetType(Vector2Int pos, ToolBarItem type)
        {
            var stageData = _current;

            switch (type)
            {
                case ToolBarItemNone _:
                    break;
                case ToolBarItemCellType cellType:
                    stageData.SetType(pos, cellType.CellType);
                    break;
                case ToolBarItemCellColor cellColor:
                    stageData.SetColor(pos, cellColor.CellColor);
                    break;
                case ToolBarItemToolType toolType:
                    if (toolType == new ToolBarItemToolType(ToolType.Reverse))
                        stageData.ReverseColor(pos);
                    else if (toolType == new ToolBarItemToolType(ToolType.SearchPath))
                    {
                        var res = PathSearchLogic.TryExecute(stageData);
                        Debug.Log($"見つかった解法の数(実装中なので正確ではない): {res}");
                    }
                    else
                        throw new ArgumentOutOfRangeException();

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }

            _current = stageData;
            _changed.OnNext(stageData);
        }

        public void AddPanel()
        {
            var stageData = _current;
            stageData.AddPanel();
            _current = stageData;
            _changed.OnNext(stageData);
        }

        public void ChangeSizeByOne(ChangeEditStageSizeEvent changeEditStageSizeEvent)
        {
            var stageData = _current;

            if (changeEditStageSizeEvent.IsRow)
                stageData.ChangeSizeByOneRow(changeEditStageSizeEvent.IsAtFirst, changeEditStageSizeEvent.IsAdd);
            else
                stageData.ChangeSizeByOneColumn(changeEditStageSizeEvent.IsAtFirst, changeEditStageSizeEvent.IsAdd);

            _current = stageData;
            _changed.OnNext(stageData);
        }
    }
}