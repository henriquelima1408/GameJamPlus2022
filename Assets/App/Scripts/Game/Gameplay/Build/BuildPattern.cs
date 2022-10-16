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
                case BuildPatternType.I1:
                    return new Vector2Int[] { down, down * 2 };
                case BuildPatternType.I2:
                    return new Vector2Int[] { right, right * 2 };
                case BuildPatternType.S:
                    return new Vector2Int[] { right, down, downLeft };
                case BuildPatternType.Z:
                    return new Vector2Int[] { right, up, upLeft };
                case BuildPatternType.L1:
                    return new Vector2Int[] { left, up, up * 2 };
                case BuildPatternType.L2:
                    return new Vector2Int[] { left, left * 2, down };
                case BuildPatternType.L3:
                    return new Vector2Int[] { right, right * 2, up };
                case BuildPatternType.L4:
                    return new Vector2Int[] { right, right * 2, down };
                case BuildPatternType.L5:
                    return new Vector2Int[] { left, down, down * 2 };
                case BuildPatternType.L6:
                    return new Vector2Int[] { right, down, down * 2 };
                case BuildPatternType.J:
                    return new Vector2Int[] { left, up, up * 2 };
                case BuildPatternType.T1:
                    return new Vector2Int[] { left, right, down };
                case BuildPatternType.T2:
                    return new Vector2Int[] { up, left, down };
                case BuildPatternType.T3:
                    return new Vector2Int[] { up, right, down };
                case BuildPatternType.T4:
                    return new Vector2Int[] { up, right, left };
                case BuildPatternType.Y1:
                    return new Vector2Int[] { up, right, downRight };
                case BuildPatternType.S1:
                    return new Vector2Int[] { right, down, downLeft };
                case BuildPatternType.S2:
                    return new Vector2Int[] { right, down, upRight };
                case BuildPatternType.S3:
                    return new Vector2Int[] { left, down, downRight };

                default:
                    break;
            }

            return null;
        }




    }
}