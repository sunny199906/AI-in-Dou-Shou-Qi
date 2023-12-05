using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class GameBoard
{
    private int height = 9;
    private int width = 7;
    private Tile[,] board;
    // For AI
    private int blackEvaluationScore = 45;
    private int redEvaluationScore = 45;
    private int noPiecesEach = 8;
    private List<Piece> blackPieces = new List<Piece>();
    private List<Piece> redPieces = new List<Piece>();

    public GameBoard()
    {
        board = new Tile[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j ++)
            {
                board[i, j] = new Tile();
            }
        }
    }

    public GameBoard(Tile[,] board, int blackEvaluationScore, int redEvaluationScore, 
                        List<Piece> blackPieces, List<Piece> redPieces)
    {
        this.board = board;
        this.blackEvaluationScore = blackEvaluationScore;
        this.redEvaluationScore = redEvaluationScore;
        this.blackPieces = blackPieces;
        this.redPieces = redPieces;
    }

    public void InitializeGameBoard(GameObject[] blackPieces, GameObject[] redPieces)
    {
        //Debug.Log(board[0,0]);
        board[0, 0].SetTile(redPieces[6].GetComponent<Piece>(), false, 6, 0, 0);
        board[0, 6].SetTile(redPieces[5].GetComponent<Piece>(), false, 5, 0, 6);
        board[1, 1].SetTile(redPieces[2].GetComponent<Piece>(), false, 2, 1, 1);
        board[1, 5].SetTile(redPieces[1].GetComponent<Piece>(), false, 1, 1, 5);
        board[2, 0].SetTile(redPieces[0].GetComponent<Piece>(), false, 0, 2, 0);
        board[2, 2].SetTile(redPieces[4].GetComponent<Piece>(), false, 4, 2, 2);
        board[2, 4].SetTile(redPieces[3].GetComponent<Piece>(), false, 3, 2, 4);
        board[2, 6].SetTile(redPieces[7].GetComponent<Piece>(), false, 7, 2, 6);

        board[6, 0].SetTile(blackPieces[7].GetComponent<Piece>(), true, 7, 6, 0);
        board[6, 2].SetTile(blackPieces[3].GetComponent<Piece>(), true, 3, 6, 2);
        board[6, 4].SetTile(blackPieces[4].GetComponent<Piece>(), true, 4, 6, 4);
        board[6, 6].SetTile(blackPieces[0].GetComponent<Piece>(), true, 0, 6, 6);
        board[7, 1].SetTile(blackPieces[1].GetComponent<Piece>(), true, 1, 7, 1);
        board[7, 5].SetTile(blackPieces[2].GetComponent<Piece>(), true, 2, 7, 5);
        board[8, 0].SetTile(blackPieces[5].GetComponent<Piece>(), true, 5, 8, 0);
        board[8, 6].SetTile(blackPieces[6].GetComponent<Piece>(), true, 6, 8, 6);

        for(int i = 3; i <=5; i++)
        {
            bool[] river = { true, false, false };
            board[i, 1].SetEnvironement(river);
            board[i, 2].SetEnvironement(river);
            board[i, 4].SetEnvironement(river);
            board[i, 5].SetEnvironement(river);
        }
        bool[] trap = { false, true, false };
        board[0, 2].SetEnvironement(trap);
        board[0, 4].SetEnvironement(trap);
        board[1, 3].SetEnvironement(trap);
        board[7, 3].SetEnvironement(trap);
        board[8, 2].SetEnvironement(trap);
        board[8, 4].SetEnvironement(trap);

        bool[] den = { false, false, true};
        board[0, 3].SetEnvironement(den);
        board[8, 3].SetEnvironement(den);

        for (int i = 0; i < noPiecesEach; i++)
        {
            this.blackPieces.Add(blackPieces[i].GetComponent<Piece>());
            this.redPieces.Add(redPieces[i].GetComponent<Piece>());
        }
        //checkBoard();
    }

    public void checkBoard()
    {
        string msg = "";
        for (int i = 0; i <= 8; i++)
        {
            for (int j = 0; j <= 6; j++)
            {
                bool[] tempEnvironment = board[i, j].GetTileEnvironment();
                if (tempEnvironment[0] == true)
                {
                    msg += "R ";
                }
                else if (tempEnvironment[1] == true)
                {
                    msg += "T ";
                }
                else if (tempEnvironment[2] == true)
                {
                    msg += "D ";
                }
                else if (board[i, j].GetIsEmpty() == true)
                {
                    msg += "E ";
                }
                else
                {
                    Piece tempPiece = board[i, j].GetTilePiece();
                    msg += (tempPiece.GetIsBlack()) ? "B" + tempPiece.GetRank() + " " 
                                                    : "R" + tempPiece.GetRank() + " ";
                }

            }
            msg += "\n";
        }
        Debug.Log(msg);
    }

    public PossibleMove CanMove(int y, int x)
    {
        //Debug.Log("Possible Move " + y + " " + x);
        PossibleMove possibleMove = new PossibleMove();
        if (y - 1 < 0)
        {
            possibleMove.SetUp(false);
        } else
        {
            if (board[y - 1, x].GetIsEmpty())
            {
                possibleMove.SetUp(CanMoveEnvironment(y, x, y - 1, x));
            } else
            {
                possibleMove.SetUp(CanMovePiece(y, x, y - 1, x));
            }
            
        }
        if (y + 1 >= height)
        {
            possibleMove.SetDown(false);
        } else
        {
            if (board[y + 1, x].GetIsEmpty())
            {
                possibleMove.SetDown(CanMoveEnvironment(y, x, y + 1, x));
            }
            else
            {
                possibleMove.SetDown(CanMovePiece(y, x, y + 1, x));
            }
        }
        if (x - 1 < 0)
        {
            possibleMove.SetLeft(false);
        } else
        {
            if (board[y, x - 1].GetIsEmpty())
            {
                possibleMove.SetLeft(CanMoveEnvironment(y, x, y, x - 1));
            }
            else
            {
                possibleMove.SetLeft(CanMovePiece(y, x, y, x - 1));
            }
        }
        if (x + 1 >= width)
        {
            possibleMove.SetRight(false);
        } else
        {
            if (board[y, x + 1].GetIsEmpty())
            {
                possibleMove.SetRight(CanMoveEnvironment(y, x, y, x + 1));
            }
            else
            {
                possibleMove.SetRight(CanMovePiece(y, x, y, x + 1));
            }
        }
        return possibleMove;
    }
    public bool CanMoveEnvironment(int y, int x, int Y, int X)
    {
        if (board[Y, X].GetTileEnvironment()[0] == true)
        {
            if (board[y, x].GetTilePiece().GetRank() != 0)
            {
                return false;
            }
        } else if (board[Y, X].GetTileEnvironment()[2] == true)
        {
            if (board[Y,X] == board[8, 3] && board[y, x].GetTilePiece().GetIsBlack())
            {
                return false;
            } else if (board[Y, X] == board[0, 3] && !board[y, x].GetTilePiece().GetIsBlack())
            {
                return false;
            }
        }
        return true;
    }

    public bool CanMovePiece(int y, int x, int Y, int X)
    {
        if (board[Y, X].GetTileEnvironment()[0] && board[y, x].GetTilePiece().GetRank() != 0)
        {
            return false;
        } else if (board[y, x].GetTileEnvironment()[0] && board[Y, X].GetTileEnvironment()[0])
        {
            return true;
        } else if (board[Y, X].GetTileEnvironment()[1])
        {
            return true;
        } else
        {
            if (board[Y, X].GetTilePiece().GetIsBlack() == board[y, x].GetTilePiece().GetIsBlack())
            {
                return false;
            } else if (board[Y, X].GetTilePiece().GetRank() == 0 && board[y, x].GetTilePiece().GetRank() == 7)
            {
                return false;
            } else if (board[Y, X].GetTilePiece().GetRank() == 7 && board[y, x].GetTilePiece().GetRank() == 0)
            {
                return true;
            } else if (board[Y, X].GetTilePiece().GetRank() > board[y, x].GetTilePiece().GetRank())
            {
                return false;
            }
        }
        return true;
    }

    public void SetBoard(float tempX1, float z, float tempX2, float Z)
    {
        int y = (int)(tempX1 + 8.1f) / 2;
        int x = (int)(z + 6f) / 2;
        int Y = (int)(tempX2 + 8.1f) / 2;
        int X = (int)(Z + 6f) / 2;
        //Debug.Log(Y + " " + X);
        //Debug.Log(board[y, x].GetPiece());
        if (board[Y, X].GetTilePiece() != null)
        {
            //Debug.Log(blackEvaluationScore);
            //Debug.Log(redEvaluationScore);
            if (board[Y, X].GetTilePiece().GetIsBlack())
            {
                blackEvaluationScore -= (board[Y, X].GetTilePiece().GetRank() + 1);
                blackPieces.Remove(board[Y, X].GetTilePiece());
            }
            else
            {
                redEvaluationScore -= (board[Y, X].GetTilePiece().GetRank() + 1);
                redPieces.Remove(board[Y, X].GetTilePiece());
            }
            //Debug.Log(blackEvaluationScore);
            //Debug.Log(redEvaluationScore);
            board[Y, X].GetTilePiece().DestroyPiece();
        }
        board[y, x].GetTilePiece().MovePiece(tempX2, Z);
        board[y, x].GetTilePiece().DestroyHighlightPiece();
        board[Y, X].SetTile(board[y, x].GetTilePiece(), Y, X);
        board[y, x].EmptyTile();
        //Debug.Log(y + " " + x + " " + Y + " " + X);
        //Debug.Log(board[Y, X].GetPiece());
        
        //checkBoard();
    }
    public void SetBoardAI(int y, int x, int Y, int X)
    {
        if (board[Y, X].GetTilePiece() != null)
        {
            //Debug.Log(blackEvaluationScore);
            //Debug.Log(redEvaluationScore);
            if (board[Y, X].GetTilePiece().GetIsBlack())
            {
                blackEvaluationScore -= (board[Y, X].GetTilePiece().GetRank() + 1);
                blackPieces.Remove(board[Y, X].GetTilePiece());
            }
            else
            {
                redEvaluationScore -= (board[Y, X].GetTilePiece().GetRank() + 1);
                redPieces.Remove(board[Y, X].GetTilePiece());
            }
            //Debug.Log(blackEvaluationScore);
            //Debug.Log(redEvaluationScore);
        }
        board[Y, X].SetTile(board[y, x].GetTilePiece(), Y, X);
        board[y, x].EmptyTile();
        //checkBoard();
    }

    public void SetBoardAIGame(float tempX1, float z, float tempX2, float Z)
    {
        int y = (int)(tempX1 + 8.1f) / 2;
        int x = (int)(z + 6f) / 2;
        int Y = (int)(tempX2 + 8.1f) / 2;
        int X = (int)(Z + 6f) / 2;
        //Debug.Log(y + " " + x + " " + Y + " " + X);
        if (board[Y, X].GetTilePiece() != null)
        {
            //Debug.Log(blackEvaluationScore);
            //Debug.Log(redEvaluationScore);
            if (board[Y, X].GetTilePiece().GetIsBlack())
            {
                blackEvaluationScore -= (board[Y, X].GetTilePiece().GetRank() + 1);
                blackPieces.Remove(board[Y, X].GetTilePiece());
            }
            else
            {
                redEvaluationScore -= (board[Y, X].GetTilePiece().GetRank() + 1);
                redPieces.Remove(board[Y, X].GetTilePiece());
            }
            //Debug.Log(blackEvaluationScore);
            //Debug.Log(redEvaluationScore);
            board[Y, X].GetTilePiece().DestroyPiece();
        }
        board[y, x].GetTilePiece().MovePiece(tempX2, Z);
        board[Y, X].SetTile(board[y, x].GetTilePiece(), Y, X);
        board[y, x].EmptyTile();
        //checkBoard();
    }

    public GameBoard GameBoardCopy()
    {
        Tile[,] copyBoard = new Tile[height, width];
        int copyBlackEvaluationScore = blackEvaluationScore;
        int copyRedEvaluationScore = redEvaluationScore;
        List<Piece> copyBlackPieces = new List<Piece>();
        List<Piece> copyRedPieces = new List<Piece>();
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                copyBoard[i, j] = new Tile();
                if (board[i, j].GetTileEnvironment()[0])
                {
                    bool[] tempEnvironment = { true, false, false };
                    copyBoard[i, j].SetEnvironement(tempEnvironment);
                } else if (board[i, j].GetTileEnvironment()[1])
                {
                    bool[] tempEnvironment = { false, true, false };
                    copyBoard[i, j].SetEnvironement(tempEnvironment);
                } else if (board[i, j].GetTileEnvironment()[2])
                {
                    bool[] tempEnvironment = { false, false, true };
                    copyBoard[i, j].SetEnvironement(tempEnvironment);
                }
                if (!board[i, j].GetIsEmpty())
                {
                    Tile tempTile = board[i, j];
                    Piece tempPiece = tempTile.GetTilePiece();
                    copyBoard[i, j].SetTile(tempPiece, i, j);
                    if (tempPiece.GetIsBlack())
                    {
                        copyBlackPieces.Add(tempPiece);
                    } else
                    {
                        copyRedPieces.Add(tempPiece); 
                    }
                }
            }
        }
        GameBoard copyGameBoard = new GameBoard(copyBoard, copyBlackEvaluationScore, copyRedEvaluationScore, 
                                                            copyBlackPieces, copyRedPieces);
        return copyGameBoard;
    }
    public List<Piece> GetBlackPieces()
    {
        return blackPieces;
    }
    public List<Piece> GetRedPieces()
    {
        return redPieces;
    }

    public bool CheckWin()
    {
        if (!board[0, 3].GetIsEmpty() || !board[8, 3].GetIsEmpty())
        {
            return true;
        } else if (blackEvaluationScore == 0 || redEvaluationScore == 0)
        {
            return true;
        }
        return false;
    }
    public int GetBlackEvaluationScore()
    {
        return blackEvaluationScore;
    }
    public int GetRedEvaluationScore()
    {
        return redEvaluationScore;
    }
}



public class PossibleMove
{
    private bool up, down, left, right;
    
    public PossibleMove()
    {
        up = true;
        down = true;
        left = true;
        right = true;
    }

    public bool GetUp()
    {
        return up;
    }

    public void SetUp(bool up)
    {
        this.up = up;
    }

    public bool GetDown()
    {
        return down;
    }
    public void SetDown(bool down)
    {
        this.down = down;
    }

    public bool GetLeft()
    {
        return left;
    }
    public void SetLeft(bool left)
    {
        this.left = left;
    }

    public bool GetRight()
    {
        return right;
    }
    public void SetRight(bool right)
    {
        this.right = right;
    }

    public override string ToString()
    {
        return "up: " + up + "\ndown: " + down + "\nleft: " + left + "\nright: " + right;
    }
}

public class MoveAI{
    private int y, x, Y, X;

    public MoveAI()
    {
    }
    public MoveAI(int y, int x, int Y, int X )
    {
        this.y = y;
        this.x = x;
        this.Y = Y;
        this.X = X;
    }
    public int Gety()
    {
        return y;
    }
    public int Getx()
    {
        return x;
    }
    public int GetY()
    {
        return Y;
    }
    public int GetX()
    {
        return X;
    }

    public bool isNull()
    {
        return y == 0 && x == 0 && Y == 0 && X == 0;
    }

    public override string ToString()
    {
        return y + ", " + x + ", " + Y + ", " + X + ", ";
    }

}
