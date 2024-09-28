using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Tilemap))]
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
    public  int Witdh = 10;
    public  int Height = 20;


    public bool IsGameOver = false;

    public void CreateGame(int witdh, int height)
    {
        Witdh = witdh;
        Height = height;
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

        generateMine();
        GenerateNumber();
    }


    public void PlayPosition(Vector3 mousePosition)
    {
        Vector3Int cellPosition = GetMousePosition(mousePosition);
        RevealTiles(cellPosition);


        // if (Gameboard[cellPosition.x, cellPosition.y].type == Cell.Type.Empty)
        // {
        //     flood(Gameboard[cellPosition.x, cellPosition.y]);
        // }
        // else
        // {
        //     RevealTiles(cellPosition);
        // }


        // tilemap.SetTile(cellPosition, tileExploded);
    }


    public void ChangeTiles(Vector3 mousePosition)
    {
        Vector3Int cellPosition = GetMousePosition(mousePosition);
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
        Tile tile = GetTilesByPosition(cellPosition);
        
        if (tile == tileExploded && IsGameOver==false)
        {
            tilemap.SetTile(cellPosition, tile);
            Gameboard[cellPosition.x, cellPosition.y].revealed = true;
            GameOver();
        }

        tilemap.SetTile(cellPosition, tile);
        if (tile == tileEmpty && IsGameOver == false)
        {
            flood(Gameboard[cellPosition.x, cellPosition.y]);
            return;
        }

        Gameboard[cellPosition.x, cellPosition.y].revealed = true;
        //
        //
        // if (Gameboard[cellPosition.x, cellPosition.y].type == Cell.Type.Mine)
        // {
        // }
        // else
        // {
        //     tilemap.SetTile(cellPosition, GetTileByNumber(Gameboard[cellPosition.x, cellPosition.y].number));
        // }
    }


    public void flood(Cell cell)
    {
        if (cell.type == Cell.Type.Mine || cell.type == Cell.Type.Invalid || cell.revealed)
        {
            return;
        }

        Gameboard[cell.position.x, cell.position.y].revealed = true;

        RevealTiles(cell.position);

        if (cell.type == Cell.Type.Empty)
        {
            flood(GetCell(cell.position.x - 1, cell.position.y));
            flood(GetCell(cell.position.x + 1, cell.position.y));
            flood(GetCell(cell.position.x, cell.position.y - 1));
            flood(GetCell(cell.position.x, cell.position.y + 1));
        }
    }


    public Cell GetCell(int x, int y)
    {
        if (x < 0 || x > Witdh - 1 || y < 0 || y > Height - 1)
        {
            return new Cell { type = Cell.Type.Invalid };
        }

        return Gameboard[x, y];
    }


    public void FlagPosition(Vector3 mousePosition)
    {
        tilemap.SetTile(GetMousePosition(mousePosition), tileFlag);
    }

    private Tile GetTilesByPosition(Vector3Int cellPosition)
    {
        Cell cell = Gameboard[cellPosition.x, cellPosition.y];

        if (cell.type == Cell.Type.Mine)
        {
            if (IsGameOver)
            {
                return tileMine;
            }

            return tileExploded;
        }

        if (cell.type == Cell.Type.Empty)
        {
            return tileEmpty;
        }


        switch (cell.number)
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
                return tileUnknown;
        }
    }

    private Vector3Int GetMousePosition(Vector3 mousePosition)
    {
        Vector3 worldPosition = MainCamera.ScreenToWorldPoint(mousePosition);
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        return cellPosition;
    }

    private void generateMine()
    {
        for (int number = 0; number < 30; number++)
        {
            int xRange = Random.Range(0, Witdh);
            int yRange = Random.Range(0, Height);
            // tilemap.SetTile(new Vector3Int(xRange, yRange, 0), tileExploded);
            // TODO CHANGER CAR PAS LE BON NOMBRE 
            Gameboard[xRange, yRange] = new Cell
                { position = new Vector3Int(xRange, yRange, 0), type = Cell.Type.Mine };
        }
    }

    public void GenerateNumber()
    {
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

    private void GameOver()
    {
        IsGameOver = true;
        ShowAllTiles();
    }

    private void ShowAllTiles()
    {
        for (int x = 0; x < Witdh; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                
                if (Gameboard[x, y].revealed == false)
                { 
                    RevealTiles(new Vector3Int(x, y));
                }
            }
        }
    }

    public void RestartGame()
    {
        if (IsGameOver)
        {
            IsGameOver = false;
            CreateGame(10,20);
        }
    }
}