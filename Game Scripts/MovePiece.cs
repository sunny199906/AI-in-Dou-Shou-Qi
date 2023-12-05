using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePiece : MonoBehaviour
{
    private float x, z;

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            float X = transform.position.x;
            float Z = transform.position.z;
            //Debug.Log("Move Piece " + X + " " + Z);
            GameManager.Instance.MovePiecePlayer(x, z, X, Z);
        }
    }
    public void SetYX(float x, float z)
    {
        this.x = x;
        this.z = z;
    }
}
