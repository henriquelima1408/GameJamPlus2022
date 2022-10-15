using App.Game.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.Game.Gameplay
{
    public static class BuildPattern
    {
        public static IEnumerable<Vector2Int> GetBuildPattern(BuildPatternType buildPatternType)
        {
            var left = Vector2Int.left;
            var right = Vector2Int.right;
            var down = Vector2Int.down;
            var up = Vector2Int.up;
            var upLeft = new Vector2Int(-1, 1);
            var upRight = new Vector2Int(1, 1);
            var downLeft = new Vector2Int(-1, -1);
            var downRight = new Vector2Int(1, -1);

            switch (buildPatternType)
            {
                case BuildPatternType.EightDirections:
                    return new Vector2Int[] { left, right, down, up, upLeft, upRight, downLeft, downRight };
                case BuildPatternType.O:
                    return new Vector2Int[] { right, down, downRight };
                case BuildPatternType.I:
                    return new Vector2Int[] { down, down * 2, down * 3 };
                case BuildPatternType.S:
                    return new Vector2Int[] { right, down, downLeft };
                case BuildPatternType.Z:
                    return new Vector2Int[] { right, up, upLeft };
                case BuildPatternType.L:
                    return new Vector2Int[] { right, up, up * 2 };
                case BuildPatternType.J:
                    return new Vector2Int[] { left, up, up * 2 };
                case BuildPatternType.T:
                    return new Vector2Int[] { left, right, down };
                default:
                    break;
            }

            return null;
        }




    }
}