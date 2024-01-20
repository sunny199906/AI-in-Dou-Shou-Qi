using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private bool isBlack;
    private int rank;
    [SerializeField] private GameObject highlightPiece;
    private static bool isSelected = false;
    private static GameObject instantiatedHighlightPiece;
    private int y, x;

    public void SetPiece(bool isBlack, int rank)
    {
        this.isBlack = isBlack;
        this.rank = rank;
    }
    public void SetPiece(bool isBlack, int rank, int y, int x)
    {
        this.isBlack = isBlack;
        this.rank = rank;
        this.y = y;
        this.x = x;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && isSelected == false && GameManager.Instance.GetPlayersTurn() 
                && !GameManager.Instance.GetWin() && isBlack == GameManager.Instance.GetPlayerGoFirst())
        {
            float tempx = transform.position.x;
            float tempz = transform.position.z;
            GameManager.Instance.PlaceHighlightedTile(tempx, tempz);

            instantiatedHighlightPiece = Instantiate(highlightPiece, transform.position, transform.rotation);
            isSelected = true;
        } else if (Input.GetMouseButtonDown(0) && isSelected == true) 
        {
            DestroyHighlightPiece();
        }
    }

    public void DestroyHighlightPiece()
    {
        GameManager.Instance.DestroyHighlightedTile();
        Destroy(instantiatedHighlightPiece);
        isSelected = false;
    }
    public void MovePiece(float x, float z)
    {
        transform.position = new Vector3(x, 0.2f, z);
    }

    public bool GetIsBlack()
    {
        return isBlack;
    }
    public int GetRank()
    {
        return rank;
    }
    public void DestroyPiece()
    {
        Destroy(gameObject);
    }
    public int Gety()
    {
        return y;
    }
    public int Getx()
    {
        return x;
    }
    public void Sety(int y)
    {
        this.y = y;
    }
    public void Setx(int x)
    {
        this.x = x;
    }
}
