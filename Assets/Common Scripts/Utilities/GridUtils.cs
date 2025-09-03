using UnityEngine;

namespace Utilities
{

    public static class GridUtils
    {
        public static Vector3 GetMiddleInCell(int x, int y, float cellSize, Vector3 startPosition)
        {
            float startX = (x * cellSize) + startPosition.x + (cellSize / 2);
            float startY = (y * cellSize) + startPosition.y + (cellSize / 2);
            return new Vector3(startX, startY, 0);
        }
        public static Vector2 GetXYAxis(Vector3 position, float cellSize, Vector3 startPosition)
        {
            int x = Mathf.FloorToInt((position.x - startPosition.x) / cellSize);
            int y = Mathf.FloorToInt((position.y - startPosition.y) / cellSize);
            return new Vector2(x, y);
        }
        public static Vector3 GetPosition(int x, int y, float cellSize, Vector3 startPosition)
        {
            float startX = (x * cellSize) + startPosition.x;
            float startY = (y * cellSize) + startPosition.y;
            return new Vector3(startX, startY, 0);
        }
    }
}
