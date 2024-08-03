using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    public Tilemap tilemap;
    public Camera MainCamera;
    public Tile tileUnknown;
    public Tile tileEmpty;
    public Tile tileMine;
    public Tile tileExploded;
    public Tile tileFlag;
    public Tile tileNum1;
    public Tile tileNum2;
    public Tile tileNum3;
    public Tile tileNum4;
    public Tile tileNum5;
    public Tile tileNum6;
    public Tile tileNum7;

    public Tile tileNum8;

    public Cell[,] Gameboard;

    public Cell this[int x, int y] => Gameboard[x, y];
    public const int Witdh = 10;
    public const int Height = 20;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera.transform.position = new Vector3(Witdh / 2, Height / 2, -10);
        Gameboard = new Cell[Witdh, Height];
        for (int i = 0; i < Witdh; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                tilemap.SetTile(new Vector3Int(i, j, 0), tileUnknown);


                Gameboard[i, j] = new Cell
                    { position = new Vector3Int(i, j, 0), type = Cell.Type.Empty, revealed = false };
            }
        }

        for (int number = 0; number < 30; number++)
        {
            int xRange = Random.Range(0, Witdh);
            int yRange = Random.Range(0, Height);
            // tilemap.SetTile(new Vector3Int(xRange, yRange, 0), tileExploded);
// TODO CHANGER CAR PAS LE BON NOMBRE 
            Gameboard[xRange, yRange] = new Cell
                { position = new Vector3Int(xRange, yRange, 0), type = Cell.Type.Mine };
        }


        for (int x = 0; x < Witdh; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Gameboard[x, y].type == Cell.Type.Empty)
                {
                    int number = GetNumberFromBombNearby(x, y);
                    if (number > 0)
                    {
                        Gameboard[x, y].type = Cell.Type.Number;
                        Gameboard[x, y].number = number;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            changeTiles();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 worldPosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

            tilemap.SetTile(cellPosition,tileFlag);
        }
    }

    public void changeTiles()
    {
        Vector3 worldPosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

        if (Gameboard[cellPosition.x, cellPosition.y].type == Cell.Type.Empty)
        {
            flood(Gameboard[cellPosition.x, cellPosition.y]);
        }
        else
        {
            RevealTiles(cellPosition);
        }


        // tilemap.SetTile(cellPosition, tileExploded);
    }

    public Tile GetTileByNumber(int number)
    {
        switch (number)
        {
            case 1:
                return tileNum1;
            case 2:
                return tileNum2;
            case 3:
                return tileNum3;
            case 4:
                return tileNum4;
            case 5:
                return tileNum5;
            case 6:
                return tileNum6;
            case 7:
                return tileNum7;
            case 8:
                return tileNum8;
            default:
                return tileEmpty;
        }
    }


    public int GetNumberFromBombNearby(int x, int y)
    {
        int count = 0;
        for (int xCheck = x - 1; xCheck <= x + 1; xCheck++)
        {
            for (int ycheck = y - 1; ycheck <= y + 1; ycheck++)
            {
                if (xCheck < 0 || xCheck > Witdh - 1 || ycheck < 0 || ycheck > Height - 1)
                {
                    continue;
                }

                if (Gameboard[xCheck, ycheck].type == Cell.Type.Mine)
                {
                    count++;
                }
            }
        }

        return count;
    }

    public void RevealTiles(Vector3Int cellPosition)
    {
        Gameboard[cellPosition.x, cellPosition.y].revealed = true;
        if (Gameboard[cellPosition.x, cellPosition.y].type == Cell.Type.Mine)
        {
            tilemap.SetTile(cellPosition, tileExploded);
        }
        else
        {
            tilemap.SetTile(cellPosition, GetTileByNumber(Gameboard[cellPosition.x, cellPosition.y].number));
        }
    }


    public void flood(Cell cell)
    {
        if (cell.type == Cell.Type.Mine || cell.type == Cell.Type.Invalid || cell.revealed)
        {
            return;
        }


        RevealTiles(cell.position);

        if (cell.type == Cell.Type.Empty)
        {
            flood(getCell(cell.position.x - 1, cell.position.y));
            flood(getCell(cell.position.x + 1, cell.position.y));
            flood(getCell(cell.position.x, cell.position.y - 1));
            flood(getCell(cell.position.x, cell.position.y + 1));
        }
    }


    public Cell getCell(int x, int y)
    {
        if (x < 0 || x > Witdh - 1 || y < 0 || y > Height - 1)
        {
            return new Cell { type = Cell.Type.Invalid };
        }

        return Gameboard[x, y];
    }
}