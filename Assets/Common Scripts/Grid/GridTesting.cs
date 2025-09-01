using System.Collections;
using System.Collections.Generic;
using Common2D.AStartPathfinding;
using Common2D.CreateGameObject2D;
using Common2D.EventMouse2D;
using UnityEngine;

public class GridTesting : MonoBehaviour
{
    [SerializeField] private GameObject prefadCamera;
    [SerializeField] private GameObject objMove;
    [SerializeField] float speed = 1f;
    [SerializeField] private bool isDebug = true;
    private AStarPathfinding AStarPathfinding;
    private GridCustom<Node> gridCustom;
    [SerializeField] bool isDiagonalDistance = false;
    List<Node> path = new List<Node>();

    void Awake()
    {
        gridCustom = new GridCustom<Node>(30, 30, 1f, new Vector3(-9,-9), (x, y) => new Node(new Vector2Int(x, y), true), isDebug);
  
        AStarPathfinding = new AStarPathfinding();
    }

    void FixedUpdate()
    {
        
        if(path != null)
            if(path.Count > 0)
            {
                Node item = path[0];
                Vector3 targetPosition = gridCustom.GetMiddlePositionItemGrid(item.position.x, item.position.y);
                objMove.transform.position = Vector3.MoveTowards(objMove.transform.position, targetPosition, speed * Time.deltaTime);
                if (Vector3.Distance(objMove.transform.position, targetPosition) < 0.1f)
                {
                    path.RemoveAt(0);
                    if (path.Count == 0)
                    {
                        Debug.Log("Path completed!");
                    }
                }
            }

        if(prefadCamera != null)
        {
            Vector3 prefadCameraPosition = prefadCamera.transform.position;
            prefadCameraPosition.x = objMove.transform.position.x;
            prefadCameraPosition.y = objMove.transform.position.y;
            prefadCamera.transform.position = prefadCameraPosition;
        }
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            Node item = gridCustom.GetItemWithPosition(EventMouse2D.GetPositionOnMouse());
            if (item != null)
            {
                path = AStarPathfinding.FindPath(
                        objMove.transform.position, 
                        gridCustom.GetPositionInWorld(item.position.x, item.position.y), isDiagonalDistance, gridCustom);
            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            Node item = gridCustom.GetItemWithPosition(EventMouse2D.GetPositionOnMouse());
            if (item != null)
            {
                item.walkable = false;
                gridCustom.SetItemWithPosition(EventMouse2D.GetPositionOnMouse(), item);
                // gridCustom.ChangeTextMeshPro(item.position.x, item.position.y, item.ToString());
                // gridCustom.ChangeColorSprite(item.position.x, item.position.y, Color.black);
                CreateGameObject.CreateSpriteRenderer(
                    gridCustom.GetMiddlePositionItemGrid(item.position.x, item.position.y), 
                    Color.blue, 3, new Vector3(1f, 1f, 1f),null);
            }
        }
    }
}
