using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public enum TileType {
    Empty = 0,
    Player,
    Enemy,
    Wall,
    Door,
    Key,
    Dagger,
    End
}

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] tiles;

    static int amountOfRooms = 10;

    Vector2[] roomPositions = new Vector2[amountOfRooms];
    Vector2[] roomSizes = new Vector2[amountOfRooms];

    bool[,] usedDirection = new bool[amountOfRooms, 4];

    protected void Start()
    {
        int width = 64;
        int height = 64;
        TileType[,] grid = new TileType[height, width];

        //Fill the entire map with walls
        FillBlock(grid, 0, 0, width, height, TileType.Wall);

        //First Room
        FillBlock(grid, 26, 26, 12, 12, TileType.Empty);
        FillBlock(grid, 32, 28, 1, 1, TileType.Player);
        //Add room position (middle of the room)
        roomPositions[0] = new Vector2(26,26);
        roomSizes[0] = new Vector2(12, 12);
        //FillBlock(grid, 30, 30, 1, 1, TileType.Dagger);
        //FillBlock(grid, 34, 30, 1, 1, TileType.Key);
        //FillBlock(grid, 32, 32, 1, 1, TileType.Door);
        //FillBlock(grid, 32, 36, 1, 1, TileType.Enemy);
        //FillBlock(grid, 32, 34, 1, 1, TileType.End);

        Debugger.instance.AddLabel(32, 26, "Room 0");

        //Multiple room generation
        for (int i = 1; i < amountOfRooms; i++)
        {
            GenerateRoom(grid, Random.Range(0, i), i);
        }

        //End in last room
        GenerateEnd(grid);

        //Actually instantiating the tiles starts here
        //use 2d array (i.e. for using cellular automata)
        CreateTilesFromArray(grid);
    }

    void GenerateRoom(TileType[,] grid, int roomOrigin, int roomID)
    {
        int roomWidth = Random.Range(3, 8);
        int roomHeight = Random.Range(3, 8);
        roomSizes[roomID] = new Vector2(roomWidth, roomHeight);

        int room1Options = Random.Range(0, 4);
        if (room1Options == 0 && usedDirection[roomOrigin, 0] == false)
        {
            //Go North
            int roomPosX = (int)roomPositions[roomOrigin].x + (int)roomSizes[roomOrigin].x / 2 - roomWidth / 2;
            int roomPosY = (int)roomPositions[roomOrigin].y + (int)roomSizes[roomOrigin].y + 1;
            roomPositions[roomID] = new Vector2(roomPosX, roomPosY);
            FillBlock(grid, roomPosX, roomPosY, roomWidth, roomHeight, TileType.Empty);
            //set the used direction from the origin on used
            usedDirection[roomOrigin, 0] = true;
            //Set the used direction in the direction to the origin on true
            usedDirection[roomID, 2] = true;

            //Adding an opening to the rooms
            int doorPosX = roomPosX + roomWidth / 2;
            int doorPosY = roomPosY - 1;
            FillBlock(grid, doorPosX, doorPosY, 1, 1, TileType.Empty);

            //Random object in the rooms
            GenerateRandomObject(grid, roomID);

            Debugger.instance.AddLabel(roomPosX, roomPosY, "Room" + roomID);
        }
        else if (room1Options == 1 && usedDirection[roomOrigin, 1] == false)
        {
            //Go East
            int roomPosX = (int)roomPositions[roomOrigin].x + (int)roomSizes[roomOrigin].x + 1;
            int roomPosY = (int)roomPositions[roomOrigin].y + (int)roomSizes[roomOrigin].y / 2 - roomHeight / 2;
            roomPositions[roomID] = new Vector2(roomPosX, roomPosY);
            FillBlock(grid, roomPosX, roomPosY, roomWidth, roomHeight, TileType.Empty);
            //set the used direction from the origin on used
            usedDirection[roomOrigin, 1] = true;
            //Set the used direction in the direction to the origin on true
            usedDirection[roomID, 3] = true;

            //Adding an opening to the rooms
            int doorPosX = roomPosX - 1;
            int doorPosY = roomPosY + roomHeight / 2;
            FillBlock(grid, doorPosX, doorPosY, 1, 1, TileType.Empty);

            //Random object in the rooms
            GenerateRandomObject(grid, roomID);

            Debugger.instance.AddLabel(roomPosX, roomPosY, "Room" + roomID);
        }
        else if (room1Options == 2 && usedDirection[roomOrigin, 2] == false)
        {
            //Go South
            int roomPosX = (int)roomPositions[roomOrigin].x + (int)roomSizes[roomOrigin].x / 2 - roomWidth / 2;
            int roomPosY = (int)roomPositions[roomOrigin].y - 1 - roomHeight;
            roomPositions[roomID] = new Vector2(roomPosX, roomPosY);
            FillBlock(grid, roomPosX, roomPosY, roomWidth, roomHeight, TileType.Empty);
            //set the used direction from the origin on used
            usedDirection[roomOrigin, 2] = true;
            //Set the used direction in the direction to the origin on true
            usedDirection[roomID, 0] = true;

            //Adding an opening to the rooms
            int doorPosX = roomPosX + roomWidth / 2;
            int doorPosY = roomPosY + roomHeight;
            FillBlock(grid, doorPosX, doorPosY, 1, 1, TileType.Empty);

            //Random object in the rooms
            GenerateRandomObject(grid, roomID);

            Debugger.instance.AddLabel(roomPosX, roomPosY, "Room" + roomID);
        }
        else if (room1Options == 3 && usedDirection[roomOrigin, 3] == false)
        {
            //Go West
            //Calculating positions
            int roomPosX = (int)roomPositions[roomOrigin].x - 1 - roomWidth;
            int roomPosY = (int)roomPositions[roomOrigin].y + (int)roomSizes[roomOrigin].y / 2 - roomHeight / 2;
            roomPositions[roomID] = new Vector2(roomPosX, roomPosY);
            FillBlock(grid, roomPosX, roomPosY, roomWidth, roomHeight, TileType.Empty);           
            //set the used direction from the origin on used
            usedDirection[roomOrigin, 3] = true;
            //Set the used direction in the direction to the origin on true
            usedDirection[roomID, 1] = true;

            //Adding an opening to the rooms
            int doorPosX = roomPosX + roomWidth;
            int doorPosY = roomPosY + roomHeight / 2;
            FillBlock(grid, doorPosX, doorPosY, 1, 1, TileType.Empty);

            //Random object in the rooms
            GenerateRandomObject(grid, roomID);

            Debugger.instance.AddLabel(roomPosX, roomPosY, "Room" + roomID);
        }
        else
        {
            Debug.Log("Retrying");
            GenerateRoom(grid, Random.Range(0, roomID), roomID);
        }
    }

    void GenerateEnd(TileType[,] grid)
    {
        int endRoomPosX = (int)roomPositions[amountOfRooms - 1].x;
        int endRoomPosY = (int)roomPositions[amountOfRooms - 1].y;
        int endPosX = (int)Random.Range(endRoomPosX, endRoomPosX + roomSizes[amountOfRooms - 1].x);
        int endPosY = (int)Random.Range(endRoomPosY, endRoomPosY + roomSizes[amountOfRooms - 1].y);
        FillBlock(grid, endPosX, endPosY, 1, 1, TileType.End);
    }

    void GenerateRandomObject(TileType[,] grid, int roomID)
    {
        int RoomPosX = (int)roomPositions[roomID].x;
        int RoomPosY = (int)roomPositions[roomID].y;
        int PosX = (int)Random.Range(RoomPosX, RoomPosX + roomSizes[roomID].x);
        int PosY = (int)Random.Range(RoomPosY, RoomPosY + roomSizes[roomID].y);

        int randomInt = Random.Range(0, 3);
        if (randomInt == 0)
        {
            FillBlock(grid, PosX, PosY, 1, 1, TileType.Dagger);
        }
        else if (randomInt == 1)
        {
            FillBlock(grid, PosX, PosY, 1, 1, TileType.Enemy);
        }
        else
        {
            FillBlock(grid, PosX, PosY, 1, 1, TileType.Key);
        }
    }

    //fill part of array with tiles
    private void FillBlock(TileType[,] grid, int x, int y, int width, int height, TileType fillType) {
        for (int tileY=0; tileY<height; tileY++) {
            for (int tileX=0; tileX<width; tileX++) {
                grid[tileY + y, tileX + x] = fillType;
            }
        }
    }

    //use array to create tiles
    private void CreateTilesFromArray(TileType[,] grid) {
        int height = grid.GetLength(0);
        int width = grid.GetLength(1);
        for (int y=0; y<height; y++) {
            for (int x=0; x<width; x++) {
                 TileType tile = grid[y, x];
                 if (tile != TileType.Empty) {
                     CreateTile(x, y, tile);
                 }
            }
        }
    }

    //create a single tile
    private GameObject CreateTile(int x, int y, TileType type) {
        int tileID = ((int)type) - 1;
        if (tileID >= 0 && tileID < tiles.Length)
        {
            GameObject tilePrefab = tiles[tileID];
            if (tilePrefab != null) {
                GameObject newTile = GameObject.Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                newTile.transform.SetParent(transform);
                return newTile;
            }

        } else {
            Debug.LogError("Invalid tile type selected");
        }

        return null;
    }

}
