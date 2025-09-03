using System;
using UnityEngine;
using Utilities;

public class GridCustom<T>
{
    int width;
    int height;
    float cellSize;
    Vector3 startPosition;
    T[,] gridArray;

    public GridCustom(
        int width,
        int height,
        float cellSize,
        Vector3 startPosition,
        Func<GridCustom<T>, int, int, T> func)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.startPosition = startPosition;
        gridArray = new T[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                T cellValue = func(this, x, y);
                gridArray[x, y] = cellValue;
            }
        }
    }

    public override string ToString()
    {
        return $"{width}, {height}";
    } 

    public Vector3 GetMiddleInCell(int x, int y)
    {
        float startX = (x * cellSize) + startPosition.x + (cellSize / 2);
        float startY = (y * cellSize) + startPosition.y + (cellSize / 2);
        return new Vector3(startX, startY, 0);
    }

    public T GetItemWithPosition(Vector3 position)
    {
        Vector2 pos = GridUtils.GetXYAxis(position, cellSize, startPosition);
        int x = (int)pos.x;
        int y = (int)pos.y;
        return GetItemGrid(x, y);
    }

    public void SetItemWithPosition(Vector3 position, T item)
    {
        Vector2 pos = GridUtils.GetXYAxis(position, cellSize, startPosition);
        int x = (int)pos.x;
        int y = (int)pos.y;
        SetItemGrid(x, y, item);
    }

    public bool IsValidGridPosition(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public void SetItemGrid(int x, int y, T item)
    {
        if (!IsValidGridPosition(x, y))
            return;

        gridArray[x, y] = item;
    }

    public T GetItemGrid(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return default(T);

        return gridArray[x, y];
    }

    public T[,] GetGridArray()
    {
        return gridArray;
    }

    public void ReadGridData(Action<T,int, int> onReadComplete)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                onReadComplete?.Invoke(gridArray[x, y], x, y);
            }
        }
    }
}