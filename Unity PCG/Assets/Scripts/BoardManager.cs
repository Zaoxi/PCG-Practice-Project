using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

	
public class BoardManager : MonoBehaviour
{
	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;
		
		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}

    public int columns = 5;
    public int rows = 5;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    private Transform boardHolder;
    private Dictionary<Vector2, Vector2> gridPositions = new Dictionary<Vector2, Vector2>();

    public void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for(int x = 0; x < columns; x++)
        {
            for(int y = 0; y<rows; y++)
            {
                gridPositions.Add(new Vector2(x, y), new Vector2(x, y));

                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    public void addToBoard(int horizontal, int vertical)
    {
        if(horizontal == 1)
        {
            // 타일이 있는지 체크
            int x = (int)Player.position.x;
            int sightX = x + 2;
            for(x += 1; x <= sightX; x++)
            {
                int y = (int)Player.position.y;
                int sightY = y + 1;
                for(y -= 1; y <= sightY; y++)
                {
                    addTiles(new Vector2(x, y));
                }
            }
        }
        else if(horizontal == -1)
        {
            int x = (int)Player.position.x;
            int sightX = x - 2;
            for(x -= 1; x >= sightX; x--)
            {
                int y = (int)Player.position.y;
                int sightY = y + 1;
                for(y -= 1; y<=sightY; y++)
                {
                    addTiles(new Vector2(x, y));
                }
            }
        }
        else if(vertical == 1)
        {
            int y = (int)Player.position.y;
            int sightY = y + 2;
            for(y += 1; y <= sightY; y++)
            {
                int x = (int)Player.position.x;
                int sightX = x + 1;
                for(x -= 1; x <= sightX; x++)
                {
                    addTiles(new Vector2(x, y));
                }
            }
        }
        else if(vertical == -1)
        {
            int y = (int)Player.position.y;
            int sightY = y - 2;
            for(y -= 1; y >= sightY; y--)
            {
                int x = (int)Player.position.x;
                int sightX = x + 1;
                for(x -= 1; x <= sightX; x++)
                {
                    addTiles(new Vector2(x, y));
                }
            }
        }
    }

    private void addTiles(Vector2 tileToAdd)
    {
        if(!gridPositions.ContainsKey(tileToAdd))
        {
            gridPositions.Add(tileToAdd, tileToAdd);
            GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

            GameObject instance = Instantiate(toInstantiate, new Vector3(tileToAdd.x, tileToAdd.y, 0f), Quaternion.identity) as GameObject;

            instance.transform.SetParent(boardHolder);

            // Choose at random a wall tile to lay
            // 1/3 확률로 벽생성
            if (Random.Range(0, 3) == 1)
            {
                // 랜덤으로 벽 하나 고름
                toInstantiate = wallTiles[Random.Range(0, wallTiles.Length)];
                // 고른 벽을 기준으로 벽 생성
                instance = Instantiate(toInstantiate, new Vector3(tileToAdd.x, tileToAdd.y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }
}
