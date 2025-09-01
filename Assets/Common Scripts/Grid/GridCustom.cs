using System;
using Common2D.CreateGameObject2D;
using TMPro;
using UnityEngine;

public class GridCustom<T>
{
    int width;
    int height;
    float cellSize;
    bool isDebug;
    Vector3 startPosition;
    GameObject gridParent;
    T[,] gridArray;
    TextMeshPro[,] textMeshPro;
    SpriteRenderer[,] spriteRenderers;

    public GridCustom(
        int width,
        int height,
        float cellSize,
        Vector3 startPosition,
        Func<int, int, T> func,
        bool isDebug = false)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.startPosition = startPosition;
        this.isDebug = isDebug;
        gridParent = new GameObject("GridParent");
        gridParent.transform.position = startPosition;
        gridArray = new T[width, height];
        textMeshPro = new TextMeshPro[width, height];
        spriteRenderers = new SpriteRenderer[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 middleCell = GetPositionInWorld(x, y) + new Vector3(cellSize / 2, cellSize / 2, 0);
                T cellValue = func(x, y);
                gridArray[x, y] = cellValue;

                if (isDebug)
                {
                    spriteRenderers[x , y] = CreateGameObject.CreateSpriteRenderer(middleCell, color: Color.white, 0, new Vector3(0.1f, 0.1f, 0.1f) , null);
                    textMeshPro[x , y] = CreateGameObject.CreateTextMeshPro(cellValue.ToString(), middleCell, null,  Color.black, 1 );
                    DrawLineGrid(GetPositionInWorld(x, y), GetPositionInWorld(x + 1, y));
                    DrawLineGrid(GetPositionInWorld(x, y), GetPositionInWorld(x, y + 1));
                }
            }
        }
        if(isDebug){
            DrawLineGrid(GetPositionInWorld(width, height), GetPositionInWorld(width, 0));
            DrawLineGrid(GetPositionInWorld(width, height), GetPositionInWorld(0, height));
        }

    }

    public override string ToString()
    {
        return $"{width}, {height}";
    }

    public Vector3 GetMiddlePositionItemGrid(int x, int y)
    {
        float startX = (x * cellSize) + startPosition.x + (cellSize / 2);
        float startY = (y * cellSize) + startPosition.y + (cellSize / 2);
        return new Vector3(startX, startY, 0);
    }

    public Vector3 GetPositionInWorld(int x, int y)
    {
        float startX = (x * cellSize) + startPosition.x;
        float startY = (y * cellSize) + startPosition.y;
        return new Vector3(startX, startY, 0);
    }

    public Vector2 GetPositionInGrid(Vector3 position)
    {
        int x = Mathf.FloorToInt((position.x - startPosition.x) / cellSize);
        int y = Mathf.FloorToInt((position.y - startPosition.y) / cellSize);
        return new Vector2(x, y);
    }

    public T GetItemWithPosition(Vector3 position)
    {
        Vector2 pos = GetPositionInGrid(position);
        int x = (int)pos.x;
        int y = (int)pos.y;
        return GetItemGrid(x, y);
    }

    public void SetItemWithPosition(Vector3 position, T item)
    {
        Vector2 pos = GetPositionInGrid(position);
        int x = (int)pos.x;
        int y = (int)pos.y;
        SetItemGrid(x, y, item);
    }

    public void ChangeTextMeshPro(int x, int y, string text)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return;

        textMeshPro[x, y].text = text;
    }

    public void ChangeColorSprite(int x, int y, Color color){
        if (x < 0 || x >= width || y < 0 || y >= height)
            return;

        spriteRenderers[x, y].color = color;
    }

    public void SetItemGrid(int x, int y, T item)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
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

    private void DrawLineGrid(Vector3 x, Vector3 y)
    {
        Debug.DrawLine(x, y, Color.white, 100f);
    }


    public void ShowDebugGrid()
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.Log($"Grid[{x},{y}] = {gridArray[x, y]}");
            }
        }
    }
}