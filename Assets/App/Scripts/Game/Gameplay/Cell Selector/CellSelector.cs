using App.Game.Data;
using App.Game.WorldBuild;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace App.Game.Gameplay
{
    public class CellSelector
    {
        readonly WorldGrid worldGrid;

        public Action<Cell, Cell, HashSet<Cell>> OnSelectedCellChanged;
        Cell currentSelectedCell;
        HashSet<Cell> hoverCells = new HashSet<Cell>();

        public bool IsPossibleToSelect(Cell cell)
        {
            return cell.IsEditable && !cell.IsInPreviewState && cell.Build == null && !cell.IsStatic;
        }

        public CellSelector(WorldGrid worldGrid)
        {
            this.worldGrid = worldGrid;
        }

        public Cell CurrentSelectedCell { get => currentSelectedCell; }
        public HashSet<Cell> HoverCells { get => hoverCells; }

        public void DeselectCell(Cell cell)
        {
            cell?.Deselect();
            foreach (var c in hoverCells)
            {
                if (c == null || c.IsInPreviewState) continue;
                c?.Deselect();
            }
            hoverCells.Clear();
            currentSelectedCell = null;
        }
        public void SelectCell(Vector2 pos, BuildData buildData)
        {
            Cell newCell = worldGrid.GetCellInPosition(pos);

            if (newCell != null)
            {
                var previousCell = currentSelectedCell;
                DeselectCell(previousCell);

                var selectedCells = GetHoverCells(newCell, buildData);
                var isPossibleToBuild = selectedCells.Count != 0;

                foreach (var cell in selectedCells)
                {
                    if (!IsPossibleToSelect(cell))
                    {
                        isPossibleToBuild = false;
                        break;
                    }
                }

                Debug.Log($"Cell {newCell.CellPos} isPossibleToBuild {isPossibleToBuild}");

                if (isPossibleToBuild)
                {
                    hoverCells = selectedCells;

                    currentSelectedCell?.Deselect();
                    currentSelectedCell = newCell;

                    currentSelectedCell.Select(true);
                    foreach (var cell in hoverCells)
                    {
                        cell?.Select(true);
                    }

                    OnSelectedCellChanged?.Invoke(previousCell, currentSelectedCell, hoverCells);
                }
                else
                {
                    if (!newCell.IsInPreviewState)
                        newCell?.Deselect();
                }
            }
            else
            {
                DeselectCell(currentSelectedCell);
            }

        }

        HashSet<Cell> GetHoverCells(Cell cell, BuildData buildData)
        {
            HashSet<Cell> outCells = new HashSet<Cell>();
            outCells.Add(cell);
            var directionsLenght = 1;
            for (int i = 1; i <= buildData.AreaRadius; i++)
            {
                Vector2Int[] directions = (Vector2Int[])BuildPattern.GetBuildPattern(buildData.BuildPattern);
                directionsLenght += directions.Length;
                var pos = cell.CellPos;

                foreach (var dir in directions)
                {
                    var hoverCell = worldGrid.GetCellInDirection(pos, dir * i);
                    if (hoverCell != null && hoverCell.IsEditable && hoverCell.Build == null) outCells.Add(hoverCell);
                }
            }

            if (outCells.Count * buildData.AreaRadius != directionsLenght)
                outCells.Clear();

            return outCells;

        }
    }
}