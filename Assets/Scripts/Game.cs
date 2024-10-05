using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Game : MonoBehaviour
{
    public GameState GameState;
    public Board gameBoard;
    public int Witdh;
    public int Height;

    void Start()
    {
        NewGame();
    }


    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void NewGame()
    {
        gameBoard.CreateGame(Witdh, Height);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameBoard.PlayPosition(Input.mousePosition);
        }

        if (Input.GetMouseButtonDown(1))
        {
            gameBoard.FlagPosition(Input.mousePosition);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameBoard.RestartGame();
        }
    }
}