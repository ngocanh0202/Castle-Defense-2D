using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common2D{
    namespace AStartPathfinding{
        public class AStarPathfinding
        {
            int width;
            int height;
            bool isDiagonalDistance;
            Vector2Int[] directions;
            GridCustom<Node> gridCustom;
            Node[,] nodes;

            public List<Node> FindPath(Vector3 startPos, Vector3 endPos, bool isDiagonalDistance ,GridCustom<Node> gridCustom) {
                this.isDiagonalDistance = isDiagonalDistance;
                this.gridCustom = gridCustom;
                nodes = gridCustom.GetGridArray();
                width = nodes.GetLength(0);
                height = nodes.GetLength(1);
                Node start = gridCustom.GetItemWithPosition(startPos);
                Node end = gridCustom.GetItemWithPosition(endPos);
                directions = isDiagonalDistance ? new Vector2Int[]
                {
                    new Vector2Int(0, 1),   // Up
                    new Vector2Int(1, 0),   // Right
                    new Vector2Int(0, -1),  // Down
                    new Vector2Int(-1, 0),  // Left
                    new Vector2Int(1, 1),   // Up-Right (Diagonal)
                    new Vector2Int(1, -1),  // Down-Right (Diagonal)
                    new Vector2Int(-1, -1), // Down-Left (Diagonal)
                    new Vector2Int(-1, 1)   // Up-Left (Diagonal)
                } : new Vector2Int[]
                {
                    new Vector2Int(0, 1),   // Up
                    new Vector2Int(1, 0),   // Right
                    new Vector2Int(0, -1),  // Down
                    new Vector2Int(-1, 0)   // Left
                };    
                return FindPath(start.position, end.position);
            }
            private List<Node> FindPath(Vector2Int startPos, Vector2Int endPos) {


                Node startNode = nodes[startPos.x, startPos.y];
                Node endNode = nodes[endPos.x, endPos.y];

                List<Node> openSet = new List<Node>();
                HashSet<Node> closedSet = new HashSet<Node>();
                openSet.Add(startNode);

                while (openSet.Count > 0) {
                    Node currentNode = openSet[0];
                    for (int i = 1; i < openSet.Count; i++) {
                        if (openSet[i].fCost < currentNode.fCost ||
                        (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)) {
                            currentNode = openSet[i];
                        }
                    }

                    openSet.Remove(currentNode);
                    closedSet.Add(currentNode);

                    if (currentNode == endNode) {
                        return RetracePath(startNode, endNode);
                    }

                    foreach (Node neighbor in GetNeighbors(currentNode)) {
                        if (!neighbor.walkable || closedSet.Contains(neighbor))
                            continue;

                        int newCost = currentNode.gCost + GetDistance(currentNode, neighbor, isDiagonalDistance);
                        if (newCost < neighbor.gCost || !openSet.Contains(neighbor)) {
                            neighbor.gCost = newCost;
                            neighbor.hCost = GetDistance(neighbor, endNode, isDiagonalDistance);
                            neighbor.parent = currentNode;
                            gridCustom.SetItemGrid(neighbor.position.x, neighbor.position.y, neighbor);
                            if (!openSet.Contains(neighbor))
                                openSet.Add(neighbor);
                        }
                    }
                }
                return null; 
            }

            List<Node> GetNeighbors(Node node) {
                List<Node> neighbors = new List<Node>();

                foreach (var dir in directions) {
                    Vector2Int newPos = node.position + dir;
                    if (newPos.x >= 0 && newPos.x < width && newPos.y >= 0 && newPos.y < height) {
                        neighbors.Add(nodes[newPos.x, newPos.y]);
                    }
                }

                return neighbors;
            }

            int GetDistance(Node a, Node b, bool isDiagonalDistance) {
                return isDiagonalDistance ? GetDiagonalDistance(a, b) : GetManhattanDistance(a, b); 
            }

            // Manhattan Distance
            int GetManhattanDistance(Node a, Node b) {
                int dx = Mathf.Abs(a.position.x - b.position.x);
                int dy = Mathf.Abs(a.position.y - b.position.y);
                return dx + dy; 
            }

            // Diagonal Distance
            int GetDiagonalDistance(Node a, Node b) {
                int dx = Mathf.Abs(a.position.x - b.position.x);
                int dy = Mathf.Abs(a.position.y - b.position.y);
                return 10 * Mathf.Max(dx, dy) + (14 - 10) * Mathf.Min(dx, dy);
            }

            List<Node> RetracePath(Node startNode, Node endNode) {
                List<Node> path = new List<Node>();
                Node current = endNode;

                while (current != startNode) {
                    path.Add(current);
                    current = current.parent;
                }

                path.Reverse();
                return path;
            }


        }

    }
}

