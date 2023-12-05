using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] blackPiecePrefabs;
    [SerializeField] private GameObject[] redPiecePrefabs;
    private bool playerGoFirst;
    [SerializeField] private GameObject gameBoard;
    private GameBoard board;
    [SerializeField] private GameObject highlightTile;
    private List<GameObject> instantiatedHighlightTile = new List<GameObject>();
    private AI ai;
    private bool isMiniMax;
    private bool playersTurn;
    private bool win = false;
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        // Make Sure Not Exist 2 GameManager
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameBoard = Instantiate(gameBoard, new Vector3(0f, 0f, 0f), Quaternion.Euler(89.98f, 0f, 180f));
        blackPiecePrefabs[0] = Instantiate(blackPiecePrefabs[0], new Vector3(4.1f, .2f, 6f), Quaternion.Euler(-89.98f, 108f, 0f));
        blackPiecePrefabs[1] = Instantiate(blackPiecePrefabs[1], new Vector3(6.1f, .2f, -4f), Quaternion.Euler(-89.98f, 84f, 0f));
        blackPiecePrefabs[2] = Instantiate(blackPiecePrefabs[2], new Vector3(6.1f, .2f, 4f), Quaternion.Euler(-89.98f, 112f, 0f));
        blackPiecePrefabs[3] = Instantiate(blackPiecePrefabs[3], new Vector3(4.1f, .2f, -2f), Quaternion.Euler(-89.98f, 92f, 0f));
        blackPiecePrefabs[4] = Instantiate(blackPiecePrefabs[4], new Vector3(4.1f, .2f, 2f), Quaternion.Euler(-89.98f, 114f, 0f));
        blackPiecePrefabs[5] = Instantiate(blackPiecePrefabs[5], new Vector3(8.1f, .2f, -6f), Quaternion.Euler(-89.98f, 94f, 0f));
        blackPiecePrefabs[6] = Instantiate(blackPiecePrefabs[6], new Vector3(8.1f, .2f, 6f), Quaternion.Euler(-89.98f, 115f, 0f));
        blackPiecePrefabs[7] = Instantiate(blackPiecePrefabs[7], new Vector3(4.1f, .2f, -6f), Quaternion.Euler(-89.98f, 100f, 0f));

        redPiecePrefabs[0] = Instantiate(redPiecePrefabs[0], new Vector3(-4.1f, .2f, -6f), Quaternion.Euler(-89.98f, -72f, 0f));
        redPiecePrefabs[1] = Instantiate(redPiecePrefabs[1], new Vector3(-6.1f, .2f, 4f), Quaternion.Euler(-89.98f, -96f, 0f));
        redPiecePrefabs[2] = Instantiate(redPiecePrefabs[2], new Vector3(-6.1f, .2f, -4f), Quaternion.Euler(-89.98f, -68f, 0f));
        redPiecePrefabs[3] = Instantiate(redPiecePrefabs[3], new Vector3(-4.1f, .2f, 2f), Quaternion.Euler(-89.98f, -88f, 0f));
        redPiecePrefabs[4] = Instantiate(redPiecePrefabs[4], new Vector3(-4.1f, .2f, -2f), Quaternion.Euler(-89.98f, -66f, 0f));
        redPiecePrefabs[5] = Instantiate(redPiecePrefabs[5], new Vector3(-8.1f, .2f, 6f), Quaternion.Euler(-89.98f, -86f, 0f));
        redPiecePrefabs[6] = Instantiate(redPiecePrefabs[6], new Vector3(-8.1f, .2f, -6f), Quaternion.Euler(-89.98f, -65f, 0f));
        redPiecePrefabs[7] = Instantiate(redPiecePrefabs[7], new Vector3(-4.1f, .2f, 6f), Quaternion.Euler(-89.98f, -80f, 0f));

        // Setup Pieces' Colour and Rank
        for (int i = 0; i <= 7; i++)
        {
            blackPiecePrefabs[i].GetComponent<Piece>().SetPiece(true, i);
            redPiecePrefabs[i].GetComponent<Piece>().SetPiece(false, i);
        }

        // See if Player or AI Goes First i.e. 0 = AI, 1 = Player
        System.Random rnd = new System.Random();
        playerGoFirst = Convert.ToBoolean(rnd.Next(0, 2));
        //playerGoFirst = true;
        //Debug.Log(playerGoFirst);

        if (!playerGoFirst)
        {
            Camera.main.transform.position = new Vector3(-10.75f, 11f, 0f);
            Camera.main.transform.rotation = Quaternion.Euler(55f, 90f, 0f);
        }
        ai = new AI(!playerGoFirst);
        playersTurn = playerGoFirst;

        board = new GameBoard();
        board.InitializeGameBoard(blackPiecePrefabs, redPiecePrefabs);
        //Debug.Log(board.canMove(0, 0).ToString());
        //Debug.Log(board.canMove(0, 2).ToString());

        SetAI(false);
        // true = MiniMax, false = Alpha-Beta Pruning
        // Above chooses AI to use Alpha-Beta Pruning 
        if (!playersTurn)
        {
            if (isMiniMax)
            {
                ai.PlayMiniMax(board);
            } else
            {
                ai.PlayAlphaBeta(board);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MovePiecePlayer(float x, float z, float X, float Z)
    {
        //Debug.Log("Game Manager " + X + " " + Z);
        board.SetBoard(x,z,X,Z);
        if (board.CheckWin())
        {
            win = true;
            if (playersTurn)
            {
                Debug.Log("Player Wins");
            } else
            {
                Debug.Log("AI Wins");
            }
            return;
        }
        SwitchTurn();
    }

    public void MovePieceAI(float x, float z, float X, float Z)
    {
        //Debug.Log("Game Manager " + X + " " + Z);
        board.SetBoardAIGame(x, z, X, Z);
        if (board.CheckWin())
        {
            win = true;
            Debug.Log("Win");
            return;
        }
        SwitchTurn();
    }

    public PossibleMove CheckCanMove(float x, float z)
    {
        int Y = (int)(x + 8.1f) / 2;
        int X = (int)(z + 6f) / 2;
        return board.CanMove(Y, X);
    }
    public void PlaceHighlightedTile(float x, float z)
    {
        DestroyHighlightedTile();
        instantiatedHighlightTile = new List<GameObject>();
        PossibleMove possibleMove = CheckCanMove(x, z);
        //Debug.Log(possibleMove.ToString());
        if (possibleMove.GetUp())
        {
            instantiatedHighlightTile.Add(Instantiate(highlightTile, new Vector3((x - 2f), 0.3f, z), 
                                                                        Quaternion.Euler(-89.98f, 0f, 0f)));
        }
        if (possibleMove.GetDown())
        {
            instantiatedHighlightTile.Add(Instantiate(highlightTile, new Vector3((x + 2f), 0.3f, z), 
                                                                        Quaternion.Euler(-89.98f, 0f, 0f)));
        }
        if (possibleMove.GetLeft())
        {
            instantiatedHighlightTile.Add(Instantiate(highlightTile, new Vector3(x, 0.3f, (z - 2f)), 
                                                                        Quaternion.Euler(-89.98f, 0f, 0f)));
        }
        if (possibleMove.GetRight())
        {
            instantiatedHighlightTile.Add(Instantiate(highlightTile, new Vector3(x, 0.3f, (z + 2f)), 
                                                                        Quaternion.Euler(-89.98f, 0f, 0f)));
        }
        foreach(GameObject i in instantiatedHighlightTile)
        {
            //Debug.Log(i);
            i.GetComponent<MovePiece>().SetYX(x, z);
        }
    }

    public void DestroyHighlightedTile()
    {
        foreach(GameObject i in instantiatedHighlightTile) {
            Destroy(i);
        }
    }
    public bool GetPlayerGoFirst()
    {
        return playerGoFirst;
    }
    public bool GetPlayersTurn()
    {
        return playersTurn;
    }

    public void SwitchTurn()
    {
        playersTurn = !playersTurn;
        if (!playersTurn)
        {
            if (isMiniMax)
            {
                ai.PlayMiniMax(board);
            }
            else
            {
                ai.PlayAlphaBeta(board);
            }
        }
    }
    
    public bool GetWin()
    {
        return win;
    }

    public void SetAI(bool isMiniMax)
    {
        this.isMiniMax = isMiniMax;
    }
}
