using App.Game.WorldBuild;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public static class PathChecker
{
    public static bool IsPathValid(WorldGrid worldGrid)
    {
        var start = worldGrid.DestinationCells.First();

        var finish = worldGrid.DestinationCells.Last();

        start.SetDistance(finish.X, finish.Y);

        var activeCells = new List<Cell>();
        activeCells.Add(start);
        var visitedCells = new List<Cell>();

        while (activeCells.Any())
        {
            var checkTile = activeCells.OrderBy(x => x.CostDistance).First();

            if (checkTile.X == finish.X && checkTile.Y == finish.Y)
            {
                worldGrid.ClearParents();
                return true;
            }

            visitedCells.Add(checkTile);
            activeCells.Remove(checkTile);

            var walkableTiles = GetWalkableTiles(worldGrid, checkTile, finish);

            foreach (var walkableTile in walkableTiles)
            {
                //We have already visited this tile so we don't need to do so again!
                if (visitedCells.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                    continue;

                //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
                if (activeCells.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                {
                    var existingTile = activeCells.First(x => x.X == walkableTile.X && x.Y == walkableTile.Y);
                    if (existingTile.CostDistance > checkTile.CostDistance)
                    {
                        activeCells.Remove(existingTile);
                        activeCells.Add(walkableTile);
                    }
                }
                else
                {
                    //We've never seen this tile before so add it to the list. 
                    activeCells.Add(walkableTile);
                }
            }
        }

        worldGrid.ClearParents();
        return false;
        Console.WriteLine("No Path Found!");
    }

    public static Cell CalculateClosestCell(Cell start, IEnumerable<Cell> cells)
    {
        int lowest = int.MaxValue;
        Cell outCell = null;
        foreach (var cell in cells)
        {
            var distance = GetDistance(start, cell);
            if (distance < lowest)
            {
                distance = lowest;
                outCell = cell;
            }
        }

        return outCell;
    }

    public static List<Cell> CalculatePathToCell(WorldGrid worldGrid, Cell start, Cell finish)
    {
        var outCells = new List<Cell>();
        start.SetDistance(finish.X, finish.Y);

        var activeCells = new List<Cell>();
        activeCells.Add(start);
        var visitedCells = new List<Cell>();

        while (activeCells.Any())
        {
            var checkCell = activeCells.OrderBy(x => x.CostDistance).First();

            if (checkCell.X == finish.X && checkCell.Y == finish.Y)
            {
                //We found the destination and we can be sure (Because the the OrderBy above)
                //That it's the most low cost option. 
                var cell = checkCell;
                int ca = 100;
                while (ca > 0)
                {
                    Console.WriteLine("");

                    //var c = worldGrid.GetCell(new Vector2Int(cell.X, cell.Y));
                    //if (!c.IsStatic)
                    //{
                    //    outCells.Add(c);
                    //}
                    cell = cell.Parent;
                    if (cell == null)
                    {
                        worldGrid.ClearParents();
                        return outCells;
                    }
                    ca--;
                }


            }

            visitedCells.Add(checkCell);
            activeCells.Remove(checkCell);

            var walkableTiles = GetWalkableTilesAI(worldGrid, checkCell, finish);

            foreach (var walkableTile in walkableTiles)
            {
                //We have already visited this tile so we don't need to do so again!
                if (visitedCells.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                    continue;

                //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
                if (activeCells.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                {
                    var existingTile = activeCells.First(x => x.X == walkableTile.X && x.Y == walkableTile.Y);
                    if (existingTile.CostDistance > checkCell.CostDistance)
                    {
                        activeCells.Remove(existingTile);
                        activeCells.Add(walkableTile);
                    }
                }
                else
                {
                    //We've never seen this tile before so add it to the list. 
                    activeCells.Add(walkableTile);
                }
            }
        }
        worldGrid.ClearParents();
        return outCells;

    }

    static int GetDistance(Cell start, Cell finish)
    {
        return Math.Abs(start.X - finish.X) + Math.Abs(start.Y - finish.Y);
    }

    private static List<Cell> GetWalkableTiles(WorldGrid worldGrid, Cell currentTile, Cell targetTile)
    {
        var possibleCells = new List<Cell>();

        possibleCells.Add(worldGrid.GetCell(new Vector2Int(currentTile.X, currentTile.Y - 1)));
        possibleCells.Add(worldGrid.GetCell(new Vector2Int(currentTile.X, currentTile.Y + 1)));
        possibleCells.Add(worldGrid.GetCell(new Vector2Int(currentTile.X - 1, currentTile.Y)));
        possibleCells.Add(worldGrid.GetCell(new Vector2Int(currentTile.X + 1, currentTile.Y)));

        foreach (var cell in possibleCells)
        {
            if (cell == null) continue;

            cell.Parent = currentTile;
            cell.Cost = currentTile.Cost + 1;
            cell.SetDistance(targetTile.X, targetTile.Y);
        }


        var maxX = worldGrid.GridSize.x;
        var maxY = worldGrid.GridSize.y;

        return possibleCells
                .Where(cell => cell != null)
                .Where(cell => cell.X >= 0 && cell.X <= maxX)
                .Where(cell => cell.Y >= 0 && cell.Y <= maxY)
                .Where(cell => cell.CanBeUsedToPath)
                .ToList();
    }

    private static List<Cell> GetWalkableTilesAI(WorldGrid worldGrid, Cell currentTile, Cell targetTile)
    {
        var possibleCells = new List<Cell>();

        possibleCells.Add(worldGrid.GetCell(new Vector2Int(currentTile.X, currentTile.Y - 1)));
        possibleCells.Add(worldGrid.GetCell(new Vector2Int(currentTile.X, currentTile.Y + 1)));
        possibleCells.Add(worldGrid.GetCell(new Vector2Int(currentTile.X - 1, currentTile.Y)));
        possibleCells.Add(worldGrid.GetCell(new Vector2Int(currentTile.X + 1, currentTile.Y)));

        foreach (var cell in possibleCells)
        {
            if (cell == null) continue;

            cell.Parent = currentTile;
            cell.Cost = currentTile.Cost + 1;
            cell.SetDistance(targetTile.X, targetTile.Y);
        }


        var maxX = worldGrid.GridSize.x;
        var maxY = worldGrid.GridSize.y;

        return possibleCells
                .Where(cell => cell != null)
                .Where(cell => cell.X >= 0 && cell.X <= maxX)
                .Where(cell => cell.Y >= 0 && cell.Y <= maxY)
                .Where(cell => !cell.IsStatic)                             
                .ToList();
    }
}
