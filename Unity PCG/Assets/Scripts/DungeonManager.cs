using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum TileType
{
    essential, random, empty
}

public class DungeonManager : MonoBehaviour {
    [Serializable]
    public class PathTile
    {
        public TileType type;
        public Vector2 position;
        public List<Vector2> adjacentPathTiles;

        public PathTile(string t, Vector2 p, int min, int max, Dictionary<Vector2, TileType> currentTiles)
        {
            type = t;
            position = p;
            adjacentPathTiles = GetAdjacentPath(min, max, currentTiles);
        }

        public List<Vector2> GetAdjacentPath(int minBound, int maxBound, Dictionary<Vector2, TileType> currentTiles)
        {
            List<Vector2> pathTiles = new List<Vector2>();
            if(position.y + 1 < maxBound && !currentTiles.ContainsKey(new Vector2(position.x, position.y + 1)))
            {
                pathTiles.Add(new Vector2(position.x, position.y + 1));
            }
            if(position.x + 1 < maxBound && !currentTiles.ContainsKey(new Vector2(position.x + 1, position.y)))
            {
                pathTiles.Add(new Vector2(position.x + 1, position.y));
            }
            if (position.y - 1 > minBound && !currentTiles.ContainsKey(new Vector2(position.x, position.y - 1)))
            {
                pathTiles.Add(new Vector2(position.x, position.y - 1));
            }
            if(position.x - 1 >= minBound && !currentTiles.ContainsKey(new Vector2(position.x - 1, position.y)) && type != TileType.essential)
            {
                pathTiles.Add(new Vector2(position.x - 1, position.y));
            }

            return pathTiles;

        }
    }

	
}
