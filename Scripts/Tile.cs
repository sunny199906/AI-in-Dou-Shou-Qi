using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private bool isEmpty = true;
    private Piece piece;
    // {River, Trap, Den}
    private bool[] environment = {false, false, false};
    
    public void SetTile(Piece piece, int y, int x)
    {
        this.piece = piece;
        isEmpty = false;
        this.piece.Sety(y);
        this.piece.Setx(x);
    }
    public void SetTile(Piece piece, bool isBlack, int rank, int y, int x)
    {
        piece.SetPiece(isBlack, rank, y, x);
        this.piece = piece;
        isEmpty = false;
    }

    public void SetEnvironement(bool[] environment)
    {
        this.environment[0] = environment[0];
        this.environment[1] = environment[1];
        this.environment[2] = environment[2];
    }

    public bool GetIsEmpty()
    {
        return isEmpty;
    }

    public Piece GetTilePiece()
    {
        return piece;
    }

    public void EmptyTile()
    {
        isEmpty = true;
        piece = null;
    }

    public bool[] GetTileEnvironment()
    {
        return environment;
    }
}
