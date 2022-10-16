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
            return cell.IsEditable && cell.Build == null;
        }

        public CellSelector(WorldGrid worldGrid)
        {
            this.worldGrid = worldGrid;
        }

        public Cell CurrentSelectedCell { get => currentSelectedCell; }
        public HashSet<Cell> HoverCells { get => hoverCells; }

        public void DeselectCell()
        {

            currentSelectedCell?.Deselect();
            foreach (var cell in hoverCells)
            {
                cell?.Deselect();
            }
            currentSelectedCell = null;
        }
        public void SelectCell(Vector2 pos, BuildData buildData)
        {
            Cell newCell = worldGrid.GetCellInPosition(pos);

            if (newCell != null)
            {
                var previousCell = currentSelectedCell;
                foreach (var cell in hoverCells)
                {
                    cell?.Deselect();
                }

                hoverCells.Clear();

                if (IsPossibleToSelect(newCell))
                {
                    hoverCells = GetHoverCells(newCell, buildData);

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
                    newCell?.Deselect();
                }
            }
            else
            {
                DeselectCell();
            }

        }

        HashSet<Cell> GetHoverCells(Cell cell, BuildData buildData)
        {
            HashSet<Cell> outCells = new HashSet<Cell>();
            for (int i = 1; i <= buildData.AreaRadius; i++)
            {

                var directions = BuildPattern.GetBuildPattern(buildData.BuildPattern);
                var pos = cell.CellPos;

                foreach (var dir in directions)
                {
                    var hoverCell = worldGrid.GetCellInDirection(pos, dir * i);
                    if (hoverCell != null && hoverCell.IsEditable && hoverCell.Build == null) outCells.Add(hoverCell);
                }
            }
            return outCells;

        }
    }
}