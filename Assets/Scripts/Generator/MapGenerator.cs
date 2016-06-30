using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

public class MapGenerator : MonoBehaviour
{

    public Coord maxMapSize;

    public Map[] maps;
    public int mapIndex;
    List<Coord> allTitleCoords;
    Queue<Coord> shuffledTitleCoords;
    Queue<Coord> shuffledOpenTileCoords;
    System.Random rnd;
    Map currentMap;

    Transform[,] tileMap;

    void Start()
    {
        
        GenerateMap();
    }

    public void GenerateMap()
    {
        currentMap = maps[mapIndex];
        tileMap = new Transform[currentMap.mapSize.x, currentMap.mapSize.y];
        if (currentMap.seed == "")
        {
            currentMap.seed = Ultility.GetRandomString(rnd, 64);
        }

        CoordAllTitles();
        shuffledTitleCoords = new Queue<Coord>(Ultility.ShuffleArray(allTitleCoords.ToArray(), currentMap.seed));

        string holderName = "Generated Map";

        if (transform.FindChild(holderName))
        {
            DestroyImmediate(transform.FindChild(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        CreateFloor(mapHolder, new System.Random(currentMap.seed.GetHashCode()));
        CreateObstacles(mapHolder);
        
        CreateBorder(mapHolder);

    }

    void CreateFloor(Transform mapHolder,System.Random rnd)
    {
        for (int x = 0; x < currentMap.mapSize.x; x++)
        {
            for (int y = 0; y < currentMap.mapSize.y; y++)
            {
                int fid = rnd.Next(0, currentMap.TileFloor.Length);
                Transform floor = Instantiate(currentMap.TileFloor[fid], TilePosition(x, y), Quaternion.identity) as Transform;
                floor.parent = mapHolder;
                tileMap[x, y] = floor;
            }
        }

        currentMap.navmeshFloor.localScale = new Vector3(maxMapSize.x, maxMapSize.y) * currentMap.tileSize;

    }

    void CreateObstacles(Transform mapHolder)
    {
        rnd = new System.Random(currentMap.seed.GetHashCode());
        bool[,] obstacleMap = new bool[(int)currentMap.mapSize.x, (int)currentMap.mapSize.y];
        int currentObstacleCount = 0;
        List<Coord> allOpenCoords = new List<Coord>(allTitleCoords);
        int obstacleCount = (int)(currentMap.obstaclePercent * (currentMap.mapSize.x * currentMap.mapSize.y));

        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;
            currentObstacleCount++;

            if (randomCoord != currentMap.MapCenter && isMapFullyAcessible(obstacleMap, currentObstacleCount))
            {
                int x = UnityEngine.Random.Range(0, currentMap.TileObstacles.Length);
                Vector3 pos = TilePosition(randomCoord.x, randomCoord.y);
                float obstacleHeight = Mathf.Lerp(currentMap.obstacleMinHeight, currentMap.obstacleMaxHeight, (float)rnd.NextDouble());
                Transform obstacle = Instantiate(currentMap.TileObstacles[x], pos, Quaternion.identity) as Transform;
                obstacle.localScale = new Vector3(currentMap.TileObstacles[x].localScale.x, obstacleHeight, currentMap.TileObstacles[x].localScale.z);

//                Renderer obstacleRenderer = obstacle.GetComponent<Renderer>();
 //               Material obstacleMaterial = new Material(obstacleRenderer.sharedMaterial);
   //             obstacleMaterial.color = currentMap.obstacleColor;
     //           obstacleRenderer.sharedMaterial = obstacleMaterial;
                obstacle.parent = mapHolder;

                allOpenCoords.Remove(randomCoord);
            }
            else
            {
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstacleCount--;
            }
        }
        shuffledOpenTileCoords = new Queue<Coord>(Ultility.ShuffleArray(allOpenCoords.ToArray(), currentMap.seed));
    }

    void CreateBorder(Transform mapHolder)
    {
        for (int x = 0; x < currentMap.mapSize.x; x++)
        {
            if (x == 0)
            {
                for (int y = -1; y < currentMap.mapSize.y+1; y++)
                {
                    Vector3 pos = new Vector3(0, 0, 0) + TilePosition(x - 1, y);
                    Transform border = Instantiate(currentMap.TileObstacles[0], pos, Quaternion.identity) as Transform;
                    border.parent = mapHolder;
                }
            }
            if (x == currentMap.mapSize.x - 1)
            {
                for (int y = -1; y < currentMap.mapSize.y+1; y++)
                {
                    Vector3 pos = new Vector3(0, 0, 0) + TilePosition(x + 1, y);
                    Transform border = Instantiate(currentMap.TileObstacles[0], pos, Quaternion.identity) as Transform;
                    border.parent = mapHolder;
                }
            }
        }
        for (int y = 0; y < currentMap.mapSize.y; y++)
        {
            if (y == 0)
            {
                for (int x = 0; x < currentMap.mapSize.x; x++)
                {
                    Vector3 pos = new Vector3(0, 0, 0) + TilePosition(x, y - 1);
                    Transform border = Instantiate(currentMap.TileObstacles[0], pos, Quaternion.identity) as Transform;
                    border.parent = mapHolder;
                }
            }
            if (y == currentMap.mapSize.y - 1)
            {
                for (int x = 0; x < currentMap.mapSize.x; x++)
                {
                    Vector3 pos = new Vector3(0, 0, 0) + TilePosition(x, y + 1);
                    Transform border = Instantiate(currentMap.TileObstacles[0], pos, Quaternion.identity) as Transform;
                    border.parent = mapHolder;
                }
            }
        }

    }

    bool isMapFullyAcessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(currentMap.MapCenter);
        mapFlags[currentMap.MapCenter.x, currentMap.MapCenter.y] = true;

        int accessibleTileCount = 1;

        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;
                    if (x == 0 || y == 0)
                    {
                        if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1))
                        {
                            if (!mapFlags[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY])
                            {
                                mapFlags[neighbourX, neighbourY] = true;
                                queue.Enqueue(new Coord(neighbourX, neighbourY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }

        int targetAccessibleTileCount = (int)(currentMap.mapSize.x * currentMap.mapSize.y - currentObstacleCount);
        return targetAccessibleTileCount == accessibleTileCount;
    }

    public Coord GetRandomCoord()
    {
        Coord randomCoord = shuffledTitleCoords.Dequeue();
        shuffledTitleCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    public Transform GetRandomOpenTile()
    {
        Coord randomCoord = shuffledOpenTileCoords.Dequeue();
        shuffledOpenTileCoords.Enqueue(randomCoord);
        return tileMap[randomCoord.x, randomCoord.y];
    }

    //Retorna posicao no mundo do tile na posicao x,y no mapa//
    Vector3 TilePosition(int x, int y)
    {
        return new Vector3(-currentMap.mapSize.x / 2f + 0.5f + x, 0, -currentMap.mapSize.y / 2f + 0.5f + y) * currentMap.tileSize;
    }

    void CoordAllTitles()
    {
        allTitleCoords = new List<Coord>();
        for (int x = 0; x < currentMap.mapSize.x; x++)
        {
            for (int y = 0; y < currentMap.mapSize.y; y++)
            {
                allTitleCoords.Add(new Coord(x,y));
            }
        }
    }

}

//Classe do mapa com as variaveis dele//
[Serializable]
public class Map
{
    [Header("Not use 25+")]
    public Coord mapSize;
    public float tileSize;
    public float obstacleMinHeight;
    public float obstacleMaxHeight;
    public Transform navmeshFloor;
    public Transform[] TileFloor = new Transform[3];
    public Transform[] TileObstacles;

    [Range(0, 1)]
    public float obstaclePercent;
    public string seed;

    public Color floorColor;
    public Color obstacleColor;

    public Coord MapCenter
    {
        get
        {
            return new Coord(mapSize.x / 2, mapSize.y / 2);
        }
    }
}