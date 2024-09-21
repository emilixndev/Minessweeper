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
        GameState = new GameState { state = GameState.StateOfTheGame.Waiting };
        // gameBoard = GetComponentInChildren<Board>();
    }

    public void NewGame()
    {
        gameBoard.CreateGame(Witdh, Height);
        // Génére la grid avec les cells 
        // Génère les bombe 
        // Génère les numéro 
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameBoard.ChangeTiles(Input.mousePosition);
        }

        if (Input.GetMouseButtonDown(1))
        {
            gameBoard.FlagPosition(Input.mousePosition);
        }
    }
}