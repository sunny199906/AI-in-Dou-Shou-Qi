using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class AI
{
    private bool isBlack;
    public int searchDepthMiniMax = 3;
    // searchdepthMiniMax 4 or above Laptop explode, too laggy
    private int searchDepthAlphaBeta = 5;
    // searchdepthMiniMax 6 or above Laptop explode, too laggy

    public AI(bool isBlack)
    {
        this.isBlack = isBlack;
    }
    public void PlayMiniMax(GameBoard board)
    {
        MoveAI moveAI;
        //Debug.Log("Works");
        moveAI = MiniMax(board, isBlack);
        //Debug.Log(moveAI.Gety() + " " + moveAI.Getx() + " " + moveAI.GetY() + " " + moveAI.GetX());
        float x = (float)moveAI.Gety() * 2 - 8.1f;
        float z = (float)moveAI.Getx() * 2 - 6f;
        float X = (float)moveAI.GetY() * 2 - 8.1f;
        float Z = (float)moveAI.GetX() * 2 - 6f;
        GameManager.Instance.MovePieceAI(x, z, X, Z);
        //Debug.Log("AI Playing");
    }
    public void PlayAlphaBeta(GameBoard board)
    {
        MoveAI moveAI;
        //Debug.Log("Works");
        moveAI = AlphaBeta(board, isBlack, -9999, 9999);
        //Debug.Log(moveAI.Gety() + " " + moveAI.Getx() + " " + moveAI.GetY() + " " + moveAI.GetX());
        float x = (float)moveAI.Gety() * 2 - 8.1f;
        float z = (float)moveAI.Getx() * 2 - 6f;
        float X = (float)moveAI.GetY() * 2 - 8.1f;
        float Z = (float)moveAI.GetX() * 2 - 6f;
        GameManager.Instance.MovePieceAI(x, z, X, Z);
        //Debug.Log("AI Playing");
    }
    public MoveAI AlphaBeta(GameBoard board, bool isBlack, int alpha, int beta)
    {
        //string msg = "";
        //Debug.Log(isBlack);
        MoveAI moveAI = new MoveAI();
        MoveAI loseMove = new MoveAI();
        if (isBlack)
        {
            List<Piece> aiPieces = board.GetBlackPieces();
            foreach (Piece i in aiPieces)
            //Piece i = aiPieces[0];
            {
                //Debug.Log(i);
                int y = i.Gety();
                int x = i.Getx();
                PossibleMove possibleMove = board.CanMove(y, x);
                if (possibleMove.GetUp())
                {
                    loseMove = new MoveAI(y, x, y - 1, x);
                    GameBoard tempBoard = board.GameBoardCopy();
                    tempBoard.SetBoardAI(y, x, y - 1, x);
                    int temp = AlphaBetaRecursive(tempBoard, !isBlack, 1, alpha, beta);
                    i.Sety(y);
                    i.Setx(x);
                    //msg += "UP " + temp + " ";
                    if (temp < beta)
                    {
                        moveAI = new MoveAI(y, x, y - 1, x);
                        beta = temp;
                    }
                    if (tempBoard.CheckWin())
                    {
                        //minEvaluationScore = -9999;
                        moveAI = new MoveAI(y, x, y - 1, x);
                        return moveAI;
                    }
                    if (beta <= alpha)
                    {
                        return moveAI;
                    }
                }
                if (possibleMove.GetDown())
                {
                    loseMove = new MoveAI(y, x, y + 1, x);
                    GameBoard tempBoard = board.GameBoardCopy();
                    tempBoard.SetBoardAI(y, x, y + 1, x);
                    int temp = AlphaBetaRecursive(tempBoard, !isBlack, 1, alpha, beta);
                    i.Sety(y);
                    i.Setx(x);
                    //msg += "DOWN " + temp + " ";
                    if (temp < beta)
                    {
                        moveAI = new MoveAI(y, x, y + 1, x);
                        beta = temp;
                    }
                    if (tempBoard.CheckWin())
                    {
                        //minEvaluationScore = -9999;
                        moveAI = new MoveAI(y, x, y + 1, x);
                        return moveAI;
                    }
                    if (beta <= alpha)
                    {
                        return moveAI;
                    }
                }
                if (possibleMove.GetLeft())
                {
                    loseMove = new MoveAI(y, x, y, x - 1);
                    GameBoard tempBoard = board.GameBoardCopy();
                    tempBoard.SetBoardAI(y, x, y, x - 1);
                    int temp = AlphaBetaRecursive(tempBoard, !isBlack, 1, alpha, beta);
                    i.Sety(y);
                    i.Setx(x);
                    //msg += "LEFT " + temp + " ";
                    if (temp < beta)
                    {
                        moveAI = new MoveAI(y, x, y, x - 1);
                        beta = temp;
                    }
                    if (tempBoard.CheckWin())
                    {
                        //minEvaluationScore = -9999;
                        moveAI = new MoveAI(y, x, y, x - 1);
                        return moveAI;
                    }
                    if (beta <= alpha)
                    {
                        return moveAI;
                    }
                }
                if (possibleMove.GetRight())
                {
                    loseMove = new MoveAI(y, x, y, x + 1);
                    GameBoard tempBoard = board.GameBoardCopy();
                    tempBoard.SetBoardAI(y, x, y, x + 1);
                    int temp = AlphaBetaRecursive(tempBoard, !isBlack, 1, alpha, beta);
                    i.Sety(y);
                    i.Setx(x);
                    //msg += "RIGHT " + temp + " ";
                    if (temp < beta)
                    {
                        moveAI = new MoveAI(y, x, y, x + 1);
                        beta = temp;
                    }
                    if (tempBoard.CheckWin())
                    {
                        //minEvaluationScore = -9999;
                        moveAI = new MoveAI(y, x, y, x + 1);
                        return moveAI;
                    }
                    if (alpha >= beta)
                    {
                        return moveAI;
                    }
                }
            }
        }
        else
        {
            List<Piece> aiPieces = board.GetRedPieces();
            foreach (Piece i in aiPieces)
            //Piece i = aiPieces[0];
            {
                //Debug.Log(i);
                //Debug.Log(i.Gety() + " " + i.Getx());
                int y = i.Gety();
                int x = i.Getx();
                PossibleMove possibleMove = board.CanMove(y, x);
                if (possibleMove.GetUp())
                {
                    loseMove = new MoveAI(y, x, y - 1, x);
                    GameBoard tempBoard = board.GameBoardCopy();
                    tempBoard.SetBoardAI(y, x, y - 1, x);
                    int temp = AlphaBetaRecursive(tempBoard, !isBlack, 1, alpha, beta);
                    i.Sety(y);
                    i.Setx(x);
                    //msg += "UP " + temp + " " + y + " " + x + "\n";
                    if (temp > alpha)
                    {
                        moveAI = new MoveAI(y, x, y - 1, x);
                        alpha = temp;
                    }
                    if (tempBoard.CheckWin())
                    {
                        //maxEvaluationScore = 9999;
                        moveAI = new MoveAI(y, x, y - 1, x);
                        return moveAI;
                    }
                    if (alpha >= beta)
                    {
                        return moveAI;
                    }
                }
                if (possibleMove.GetDown())
                {
                    loseMove = new MoveAI(y, x, y + 1, x);
                    GameBoard tempBoard = board.GameBoardCopy();
                    tempBoard.SetBoardAI(y, x, y + 1, x);
                    int temp = AlphaBetaRecursive(tempBoard, !isBlack, 1, alpha, beta);
                    i.Sety(y);
                    i.Setx(x);
                    //msg += "DOWN " + temp + " " + y + " " + x + "\n";
                    if (temp > alpha)
                    {
                        moveAI = new MoveAI(y, x, y + 1, x);
                        alpha = temp;
                    }
                    if (tempBoard.CheckWin())
                    {
                        //maxEvaluationScore = 9999;
                        moveAI = new MoveAI(y, x, y + 1, x);
                        return moveAI;
                    }
                    if (alpha >= beta)
                    {
                        return moveAI;
                    }
                }
                if (possibleMove.GetLeft())
                {
                    loseMove = new MoveAI(y, x, y, x - 1);
                    GameBoard tempBoard = board.GameBoardCopy();
                    tempBoard.SetBoardAI(y, x, y, x - 1);
                    int temp = AlphaBetaRecursive(tempBoard, !isBlack, 1, alpha, beta);
                    i.Sety(y);
                    i.Setx(x);
                    //msg += "LEFT " + temp + " " + y + " " + x + "\n";
                    if (temp > alpha)
                    {
                        moveAI = new MoveAI(y, x, y, x - 1);
                        alpha = temp;
                    }
                    if (tempBoard.CheckWin())
                    {
                        //maxEvaluationScore = 9999;
                        moveAI = new MoveAI(y, x, y, x - 1);
                        return moveAI;
                    }
                    if (alpha >= beta)
                    {
                        return moveAI;
                    }
                }
                if (possibleMove.GetRight())
                {
                    loseMove = new MoveAI(y, x, y, x + 1);
                    GameBoard tempBoard = board.GameBoardCopy();
                    tempBoard.SetBoardAI(y, x, y, x + 1);
                    int temp = AlphaBetaRecursive(tempBoard, !isBlack, 1, alpha, beta);
                    i.Sety(y);
                    i.Setx(x);
                    //msg += "RIGHT " + temp + " " + y + " " + x + "\n";
                    if (temp > alpha)
                    {
                        moveAI = new MoveAI(y, x, y, x + 1);
                        alpha = temp;
                    }
                    if (tempBoard.CheckWin())
                    {
                        //maxEvaluationScore = 9999;
                        moveAI = new MoveAI(y, x, y, x + 1);
                        return moveAI;
                    }
                    if (alpha >= beta)
                    {
                        return moveAI;
                    }
                }
            }
        }
        //Debug.Log(msg);
        if (moveAI.isNull())
        {
            return loseMove;
        }
        return moveAI;
    }

    public int AlphaBetaRecursive(GameBoard board, bool isBlack, int currentDepth, int alpha, int beta)
    {
        //Debug.Log(currentDepth);
        if (isBlack)
        {
            if (currentDepth >= searchDepthAlphaBeta)
            {
                return board.GetRedEvaluationScore() - board.GetBlackEvaluationScore();
            }
            else
            {
                List<Piece> aiPieces = board.GetBlackPieces();
                foreach (Piece i in aiPieces)
                {
                    int y = i.Gety();
                    int x = i.Getx();
                    PossibleMove possibleMove = board.CanMove(y, x);
                    if (possibleMove.GetUp())
                    {
                        GameBoard tempBoard = board.GameBoardCopy();
                        tempBoard.SetBoardAI(y, x, y - 1, x);
                        int temp = AlphaBetaRecursive(tempBoard, !isBlack, currentDepth + 1, alpha, beta);
                        i.Sety(y);
                        i.Setx(x);
                        if (temp < beta)
                        {
                            beta = temp;
                        }
                        if (tempBoard.CheckWin())
                        {
                            beta = -9999;
                            return beta;
                        }
                        if (beta <= alpha)
                        {
                            return beta;
                        }
                    }
                    if (possibleMove.GetDown())
                    {
                        GameBoard tempBoard = board.GameBoardCopy();
                        tempBoard.SetBoardAI(y, x, y + 1, x);
                        int temp = AlphaBetaRecursive(tempBoard, !isBlack, currentDepth + 1, alpha, beta);
                        i.Sety(y);
                        i.Setx(x);
                        if (temp < beta)
                        {
                            beta = temp;
                        }
                        if (tempBoard.CheckWin())
                        {
                            beta = -9999;
                            return beta;
                        }
                        if (beta <= alpha)
                        {
                            return beta;
                        }

                    }
                    if (possibleMove.GetLeft())
                    {
                        GameBoard tempBoard = board.GameBoardCopy();
                        tempBoard.SetBoardAI(y, x, y, x - 1);
                        int temp = AlphaBetaRecursive(tempBoard, !isBlack, currentDepth + 1, alpha, beta);
                        i.Sety(y);
                        i.Setx(x);
                        if (temp < beta)
                        {
                            beta = temp;
                        }
                        if (tempBoard.CheckWin())
                        {
                            beta = -9999;
                            return beta;
                        }
                        if (beta <= alpha)
                        {
                            return beta;
                        }
                    }
                    if (possibleMove.GetRight())
                    {
                        GameBoard tempBoard = board.GameBoardCopy();
                        tempBoard.SetBoardAI(y, x, y, x + 1);
                        int temp = AlphaBetaRecursive(tempBoard, !isBlack, currentDepth + 1, alpha, beta);
                        i.Sety(y);
                        i.Setx(x);
                        if (temp < beta)
                        {
                            beta = temp;
                        }
                        if (tempBoard.CheckWin())
                        {
                            beta = -9999;
                            return beta;
                        }
                        if (beta <= alpha)
                        {
                            return beta;
                        }
                    }
                }
                return beta;
            }
        }
        else
        {
            if (currentDepth >= searchDepthAlphaBeta)
            {
                return board.GetRedEvaluationScore() - board.GetBlackEvaluationScore();
            }
            else
            {
                List<Piece> aiPieces = board.GetRedPieces();
                foreach (Piece i in aiPieces)
                {
                    int y = i.Gety();
                    int x = i.Getx();
                    PossibleMove possibleMove = board.CanMove(y, x);
                    if (possibleMove.GetUp())
                    {
                        GameBoard tempBoard = board.GameBoardCopy();
                        tempBoard.SetBoardAI(y, x, y - 1, x);
                        int temp = AlphaBetaRecursive(tempBoard, !isBlack, currentDepth + 1, alpha, beta);
                        i.Sety(y);
                        i.Setx(x);
                        if (temp > alpha)
                        {
                            alpha = temp;
                        }
                        if (tempBoard.CheckWin())
                        {
                            alpha = 9999;
                            return alpha;
                        }
                        if (alpha >= beta)
                        {
                            return alpha;
                        }
                    }
                    if (possibleMove.GetDown())
                    {
                        GameBoard tempBoard = board.GameBoardCopy();
                        tempBoard.SetBoardAI(y, x, y + 1, x);
                        int temp = AlphaBetaRecursive(tempBoard, !isBlack, currentDepth + 1, alpha, beta);
                        i.Sety(y);
                        i.Setx(x);
                        if (temp > alpha)
                        {
                            alpha = temp;
                        }
                        if (tempBoard.CheckWin())
                        {
                            alpha = 9999;
                            return alpha;
                        }
                        if (alpha >= beta)
                        {
                            return alpha;
                        }
                    }
                    if (possibleMove.GetLeft())
                    {
                        GameBoard tempBoard = board.GameBoardCopy();
                        tempBoard.SetBoardAI(y, x, y, x - 1);
                        int temp = AlphaBetaRecursive(tempBoard, !isBlack, currentDepth + 1, alpha, beta);
                        i.Sety(y);
                        i.Setx(x);
                        if (temp > alpha)
                        {
                            alpha = temp;
                        }
                        if (tempBoard.CheckWin())
                        {
                            alpha = 9999;
                            return alpha;
                        }
                        if (alpha >= beta)
                        {
                            return alpha;
                        }
                    }
                    if (possibleMove.GetRight())
                    {
                        GameBoard tempBoard = board.GameBoardCopy();
                        tempBoard.SetBoardAI(y, x, y, x + 1);
                        int temp = AlphaBetaRecursive(tempBoard, !isBlack, currentDepth + 1, alpha, beta);
                        i.Sety(y);
                        i.Setx(x);
                        if (temp > alpha)
                        {
                            alpha = temp;
                        }
                        if (tempBoard.CheckWin())
                        {
                            alpha = 9999;
                            return alpha;
                        }
                        if (alpha >= beta)
                        {
                            return alpha;
                        }
                    }
                }
                return alpha;
            }
        }
    }

    public MoveAI MiniMax(GameBoard board, bool isBlack)
    {
        //string msg = "";
        //Debug.Log(isBlack);
        MoveAI moveAI = new MoveAI();
        MoveAI loseMove = new MoveAI();
        if (isBlack)
        {
            int minEvaluationScore = 9999;
            List<Piece> aiPieces = board.GetBlackPieces();
            foreach (Piece i in aiPieces)
            //Piece i = aiPieces[0];
            {
                //Debug.Log(i);
                int y = i.Gety();
                int x = i.Getx();
                PossibleMove possibleMove = board.CanMove(y, x);
                if (possibleMove.GetUp())
                {
                    loseMove = new MoveAI(y, x, y - 1, x);
                    GameBoard tempBoard = board.GameBoardCopy();
                    tempBoard.SetBoardAI(y, x, y - 1, x);
                    int temp = MiniMaxRecursive(tempBoard, !isBlack, 1);
                    i.Sety(y);
                    i.Setx(x);
                    //msg += "UP " + temp + " ";
                    if (temp < minEvaluationScore)
                    {
                        moveAI = new MoveAI(y, x, y - 1, x);
                        minEvaluationScore = temp;
                    }
                    if (tempBoard.CheckWin())
                    {
                        //minEvaluationScore = -9999;
                        moveAI = new MoveAI(y, x, y - 1, x);
                        return moveAI;
                    }
                }
                if (possibleMove.GetDown())
                {
                    loseMove = new MoveAI(y, x, y + 1, x);
                    GameBoard tempBoard = board.GameBoardCopy();
                    tempBoard.SetBoardAI(y, x, y + 1, x);
                    int temp = MiniMaxRecursive(tempBoard, !isBlack, 1);
                    i.Sety(y);
                    i.Setx(x);
                    //msg += "DOWN " + temp + " ";
                    if (temp < minEvaluationScore)
                    {
                        moveAI = new MoveAI(y, x, y + 1, x);
                        minEvaluationScore = temp;
                    }
                    if (tempBoard.CheckWin())
                    {
                        //minEvaluationScore = -9999;
                        moveAI = new MoveAI(y, x, y + 1, x);
                        return moveAI;
                    }
                }
                if (possibleMove.GetLeft())
                {
                    loseMove = new MoveAI(y, x, y, x - 1);
                    GameBoard tempBoard = board.GameBoardCopy();
                    tempBoard.SetBoardAI(y, x, y, x - 1);
                    int temp = MiniMaxRecursive(tempBoard, !isBlack, 1);
                    i.Sety(y);
                    i.Setx(x);
                    //msg += "LEFT " + temp + " ";
                    if (temp < minEvaluationScore)
                    {
                        moveAI = new MoveAI(y, x, y, x - 1);
                        minEvaluationScore = temp;
                    }
                    if (tempBoard.CheckWin())
                    {
                        //minEvaluationScore = -9999;
                        moveAI = new MoveAI(y, x, y, x - 1);
                        return moveAI;
                    }
                }
                if (possibleMove.GetRight())
                {
                    loseMove = new MoveAI(y, x, y, x + 1);
                    GameBoard tempBoard = board.GameBoardCopy();
                    tempBoard.SetBoardAI(y, x, y, x + 1);
                    int temp = MiniMaxRecursive(tempBoard, !isBlack, 1);
                    i.Sety(y);
                    i.Setx(x);
                    //msg += "RIGHT " + temp + " ";
                    if (temp < minEvaluationScore)
                    {
                        moveAI = new MoveAI(y, x, y, x + 1);
                        minEvaluationScore = temp;
                    }
                    if (tempBoard.CheckWin())
                    {
                        //minEvaluationScore = -9999;
                        moveAI = new MoveAI(y, x, y, x + 1);
                        return moveAI;
                    }
                }
            }
        }
        else
        {
            int maxEvaluationScore = -9999;
            List<Piece> aiPieces = board.GetRedPieces();
            foreach (Piece i in aiPieces)
            //Piece i = aiPieces[0];
            {
                //Debug.Log(i);
                //Debug.Log(i.Gety() + " " + i.Getx());
                int y = i.Gety();
                int x = i.Getx();
                PossibleMove possibleMove = board.CanMove(y, x);
                if (possibleMove.GetUp())
                {
                    loseMove = new MoveAI(y, x, y - 1, x);
                    GameBoard tempBoard = board.GameBoardCopy();
                    tempBoard.SetBoardAI(y, x, y - 1, x);
                    int temp = MiniMaxRecursive(tempBoard, !isBlack, 1);
                    i.Sety(y);
                    i.Setx(x);
                    //msg += "UP " + temp + " " + y + " " + x + "\n";
                    if (temp > maxEvaluationScore)
                    {
                        moveAI = new MoveAI(y, x, y - 1, x);
                        maxEvaluationScore = temp;
                    }
                    if (tempBoard.CheckWin())
                    {
                        //maxEvaluationScore = 9999;
                        moveAI = new MoveAI(y, x, y - 1, x);
                        return moveAI;
                    }
                }
                if (possibleMove.GetDown())
                {
                    loseMove = new MoveAI(y, x, y + 1, x);
                    GameBoard tempBoard = board.GameBoardCopy();
                    tempBoard.SetBoardAI(y, x, y + 1, x);
                    int temp = MiniMaxRecursive(tempBoard, !isBlack, 1);
                    i.Sety(y);
                    i.Setx(x);
                    //msg += "DOWN " + temp + " " + y + " " + x + "\n";
                    if (temp > maxEvaluationScore)
                    {
                        moveAI = new MoveAI(y, x, y + 1, x);
                        maxEvaluationScore = temp;
                    }
                    if (tempBoard.CheckWin())
                    {
                        //maxEvaluationScore = 9999;
                        moveAI = new MoveAI(y, x, y + 1, x);
                        return moveAI;
                    }
                }
                if (possibleMove.GetLeft())
                {
                    loseMove = new MoveAI(y, x, y, x - 1);
                    GameBoard tempBoard = board.GameBoardCopy();
                    tempBoard.SetBoardAI(y, x, y, x - 1);
                    int temp = MiniMaxRecursive(tempBoard, !isBlack, 1);
                    i.Sety(y);
                    i.Setx(x);
                    //msg += "LEFT " + temp + " " + y + " " + x + "\n";
                    if (temp > maxEvaluationScore)
                    {
                        moveAI = new MoveAI(y, x, y, x - 1);
                        maxEvaluationScore = temp;
                    }
                    if (tempBoard.CheckWin())
                    {
                        //maxEvaluationScore = 9999;
                        moveAI = new MoveAI(y, x, y, x - 1);
                        return moveAI;
                    }
                }
                if (possibleMove.GetRight())
                {
                    loseMove = new MoveAI(y, x, y, x + 1);
                    GameBoard tempBoard = board.GameBoardCopy();
                    tempBoard.SetBoardAI(y, x, y, x + 1);
                    int temp = MiniMaxRecursive(tempBoard, !isBlack, 1);
                    i.Sety(y);
                    i.Setx(x);
                    //msg += "RIGHT " + temp + " " + y + " " + x + "\n";
                    if (temp > maxEvaluationScore)
                    {
                        moveAI = new MoveAI(y, x, y, x + 1);
                        maxEvaluationScore = temp;
                    }
                    if (tempBoard.CheckWin())
                    {
                        //maxEvaluationScore = 9999;
                        moveAI = new MoveAI(y, x, y, x + 1);
                        return moveAI;
                    }
                }
            }
        }
        //Debug.Log(msg);
        if (moveAI.isNull())
        {
            return loseMove;
        }
        return moveAI;
    }

    public int MiniMaxRecursive(GameBoard board, bool isBlack, int currentDepth)
    {
        //Debug.Log(currentDepth);
        if (isBlack)
        {
            if (currentDepth >= searchDepthMiniMax)
            {
                return board.GetRedEvaluationScore() - board.GetBlackEvaluationScore();
            }
            else
            {
                int minEvaluationScore = 9999;
                List<Piece> aiPieces = board.GetBlackPieces();
                foreach (Piece i in aiPieces)
                {
                    int y = i.Gety();
                    int x = i.Getx();
                    PossibleMove possibleMove = board.CanMove(y, x);
                    if (possibleMove.GetUp())
                    {
                        GameBoard tempBoard = board.GameBoardCopy();
                        tempBoard.SetBoardAI(y, x, y - 1, x);
                        int temp = MiniMaxRecursive(tempBoard, !isBlack, currentDepth + 1);
                        i.Sety(y);
                        i.Setx(x);
                        if (temp < minEvaluationScore)
                        {
                            minEvaluationScore = temp;
                        }
                        if (tempBoard.CheckWin())
                        {
                            minEvaluationScore = -9999;
                            return minEvaluationScore;
                        }
                    }
                    if (possibleMove.GetDown())
                    {
                        GameBoard tempBoard = board.GameBoardCopy();
                        tempBoard.SetBoardAI(y, x, y + 1, x);
                        int temp = MiniMaxRecursive(tempBoard, !isBlack, currentDepth + 1);
                        i.Sety(y);
                        i.Setx(x);
                        if (temp < minEvaluationScore)
                        {
                            minEvaluationScore = temp;
                        }
                        if (tempBoard.CheckWin())
                        {
                            minEvaluationScore = -9999;
                            return minEvaluationScore;
                        }
                    }
                    if (possibleMove.GetLeft())
                    {
                        GameBoard tempBoard = board.GameBoardCopy();
                        tempBoard.SetBoardAI(y, x, y, x - 1);
                        int temp = MiniMaxRecursive(tempBoard, !isBlack, currentDepth + 1);
                        i.Sety(y);
                        i.Setx(x);
                        if (temp < minEvaluationScore)
                        {
                            minEvaluationScore = temp;
                        }
                        if (tempBoard.CheckWin())
                        {
                            minEvaluationScore = -9999;
                            return minEvaluationScore;
                        }
                    }
                    if (possibleMove.GetRight())
                    {
                        GameBoard tempBoard = board.GameBoardCopy();
                        tempBoard.SetBoardAI(y, x, y, x + 1);
                        int temp = MiniMaxRecursive(tempBoard, !isBlack, currentDepth + 1);
                        i.Sety(y);
                        i.Setx(x);
                        if (temp < minEvaluationScore)
                        {
                            minEvaluationScore = temp;
                        }
                        if (tempBoard.CheckWin())
                        {
                            minEvaluationScore = -9999;
                            return minEvaluationScore;
                        }
                    }
                }
            }
        }
        else
        {
            if (currentDepth >= searchDepthMiniMax)
            {
                return board.GetRedEvaluationScore() - board.GetBlackEvaluationScore();
            }
            else
            {
                int maxEvaluationScore = -9999;
                List<Piece> aiPieces = board.GetRedPieces();
                foreach (Piece i in aiPieces)
                {
                    int y = i.Gety();
                    int x = i.Getx();
                    PossibleMove possibleMove = board.CanMove(y, x);
                    if (possibleMove.GetUp())
                    {
                        GameBoard tempBoard = board.GameBoardCopy();
                        tempBoard.SetBoardAI(y, x, y - 1, x);
                        int temp = MiniMaxRecursive(tempBoard, !isBlack, currentDepth + 1);
                        i.Sety(y);
                        i.Setx(x);
                        if (temp > maxEvaluationScore)
                        {
                            maxEvaluationScore = temp;
                        }
                        if (tempBoard.CheckWin())
                        {
                            maxEvaluationScore = 9999;
                            return maxEvaluationScore;
                        }
                    }
                    if (possibleMove.GetDown())
                    {
                        GameBoard tempBoard = board.GameBoardCopy();
                        tempBoard.SetBoardAI(y, x, y + 1, x);
                        int temp = MiniMaxRecursive(tempBoard, !isBlack, currentDepth + 1);
                        i.Sety(y);
                        i.Setx(x);
                        if (temp > maxEvaluationScore)
                        {
                            maxEvaluationScore = temp;
                        }
                        if (tempBoard.CheckWin())
                        {
                            maxEvaluationScore = 9999;
                            return maxEvaluationScore;
                        }
                    }
                    if (possibleMove.GetLeft())
                    {
                        GameBoard tempBoard = board.GameBoardCopy();
                        tempBoard.SetBoardAI(y, x, y, x - 1);
                        int temp = MiniMaxRecursive(tempBoard, !isBlack, currentDepth + 1);
                        i.Sety(y);
                        i.Setx(x);
                        if (temp > maxEvaluationScore)
                        {
                            maxEvaluationScore = temp;
                        }
                        if (tempBoard.CheckWin())
                        {
                            maxEvaluationScore = 9999;
                            return maxEvaluationScore;
                        }
                    }
                    if (possibleMove.GetRight())
                    {
                        GameBoard tempBoard = board.GameBoardCopy();
                        tempBoard.SetBoardAI(y, x, y, x + 1);
                        int temp = MiniMaxRecursive(tempBoard, !isBlack, currentDepth + 1);
                        i.Sety(y);
                        i.Setx(x);
                        if (temp > maxEvaluationScore)
                        {
                            maxEvaluationScore = temp;
                        }
                        if (tempBoard.CheckWin())
                        {
                            maxEvaluationScore = 9999;
                            return maxEvaluationScore;
                        }
                    }
                }
            }
        }
        return board.GetRedEvaluationScore() - board.GetBlackEvaluationScore();
    }
}
