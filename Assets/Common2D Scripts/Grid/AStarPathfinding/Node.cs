using UnityEngine;

namespace Common2D{
    namespace AStartPathfinding{
        public class Node {
            public Vector2Int position;
            public bool walkable;
            public int gCost;
            public int hCost;
            public int fCost => gCost + hCost;
            public Node parent;
            public Node(Vector2Int pos, bool isWalkable) {
                position = pos;
                walkable = isWalkable;
            }
            
            public override string ToString() {
                return  walkable ? "|" : "x";
            }
        }
    }
}

