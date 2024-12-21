using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using Kiro.Domain;
using Kiro.Presentation;
using UnityEngine;

namespace Kiro.Data.EditMode
{
    [Serializable]
    public sealed record StageData
    {
        [SerializeField] int _levelIndex;

        [ValidateInput("ValidateCellInfo", "CellInfosの要素数とSizeの縦横の積が一致していません。")]
        [SerializeField] List<CellInfo> _cellInfos;

        [ValidateInput("ValidatePanelInfo", "PanelInfosがありません。")]
        [SerializeField] List<PanelInfo> _panelInfos;
        [SerializeField] Vector2Int _size;

        [SerializeField] StageBackgroundView _stageBackgroundView;

        public LevelInWorldId ActualId => new(_levelIndex + 1);
        public Vector2Int Size => _size;
        public StageBackgroundView StageBackgroundView => _stageBackgroundView;

        bool ValidateCellInfo(List<CellRawData> cellInfos)
        {
            if (cellInfos.Count != _size.x * _size.y) return false;

            return true;
        }

        bool ValidatePanelInfo(List<PanelInfo> panelInfos) => panelInfos.Count != 0;

        /// <summary>
        ///     すべてのセルを取得する
        /// </summary>
        public IEnumerable<Cell> GetCells()
        {
            for (var y = 0; y < _size.y; y++)
            {
                for (var x = 0; x < _size.x; x++)
                {
                    var cellInfo = _cellInfos[y * _size.x + x];
                    yield return cellInfo.Create();
                }
            }
        }

        public Dictionary<Vector2Int, Cell> GetCellDictionary()
        {
            var dictionary = new Dictionary<Vector2Int, Cell>();
            for (var y = 0; y < _size.y; y++)
            {
                for (var x = 0; x < _size.x; x++)
                {
                    var cellInfo = _cellInfos[y * _size.x + x];
                    dictionary[new Vector2Int(x, y)] = cellInfo.Create();
                }
            }

            return dictionary;
        }

        public Cell GetCell(int index) => _cellInfos[index].Create();

        public IEnumerable<PanelInfo> GetPanelInfos() => _panelInfos;

        public void SetType(Vector2Int position, CellType type)
        {
            var index = position.y * _size.x + position.x;
            _cellInfos[index] = new CellInfo(_cellInfos[index].Color, type);
        }

        public void SetType(int index, CellType type)
        {
            _cellInfos[index] = new CellInfo(_cellInfos[index].Color, type);
        }

        public void SetColor(Vector2Int position, CellColor color)
        {
            var index = position.y * _size.x + position.x;
            _cellInfos[index] = new CellInfo(color, _cellInfos[index].Type);
        }

        public void SetColor(int index, CellColor color)
        {
            _cellInfos[index] = new CellInfo(color, _cellInfos[index].Type);
        }

        public void ReverseColor(Vector2Int position)
        {
            var index = position.y * _size.x + position.x;
            var cellInfo = _cellInfos[index];
            var color = cellInfo.Color;

            // TODO: このロジック外出しした方がいいかも
            color = color switch
            {
                CellColor.Empty => CellColor.Empty,
                CellColor.White => CellColor.Black,
                CellColor.Black => CellColor.White,
                _               => color
            };

            _cellInfos[index] = new CellInfo(color, cellInfo.Type);
        }

        public void AddPanel()
        {
            _panelInfos.Add(new PanelInfo());
        }

        public void ChangeSizeByOneRow(bool isAtFirst, bool isAdd)
        {
            var size = _size;
            if (isAtFirst)
            {
                if (isAdd)
                {
                    for (var x = 0; x < size.x; x++)
                    {
                        _cellInfos.Insert(0, default);
                    }

                    size.y++;
                }
                else
                {
                    for (var x = 0; x < size.x; x++)
                    {
                        _cellInfos.RemoveAt(0);
                    }

                    size.y--;
                }
            }
            else
            {
                if (isAdd)
                {
                    for (var x = 0; x < size.x; x++)
                    {
                        _cellInfos.Add(default);
                    }

                    size.y++;
                }
                else
                {
                    for (var x = 0; x < size.x; x++)
                    {
                        _cellInfos.RemoveAt(_cellInfos.Count - 1);
                    }

                    size.y--;
                }
            }

            _size = size;
            Debug.Log($"ChangeSizeByOneRow: {_size}, cellInfos: {_cellInfos.Count}");
        }

        public void ChangeSizeByOneColumn(bool isAtFirst, bool isAdd)
        {
            var size = _size;
            if (isAtFirst)
            {
                if (isAdd)
                {
                    for (var y = size.y - 1; y >= 0; y--)
                    {
                        _cellInfos.Insert(y * size.x, default);
                    }

                    size.x++;
                }
                else
                {
                    for (var y = size.y - 1; y >= 0; y--)
                    {
                        _cellInfos.RemoveAt(y * size.x);
                    }

                    size.x--;
                }
            }
            else
            {
                if (isAdd)
                {
                    _cellInfos.Add(default);

                    for (var y = size.y - 2; y >= 0; y--)
                    {
                        _cellInfos.Insert(y * size.x + size.x, default);
                    }

                    size.x++;
                }
                else
                {
                    for (var y = size.y - 1; y >= 0; y--)
                    {
                        _cellInfos.RemoveAt(y * size.x + size.x - 1);
                    }

                    size.x--;
                }
            }

            _size = size;
            Debug.Log($"ChangeSizeByOneRow: {_size}, cellInfos: {_cellInfos.Count}");
        }

        public override string ToString() => $"StageData: {ActualId}, Size: {_size}";
    }
}