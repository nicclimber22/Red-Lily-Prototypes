using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class enemy : MonoBehaviour
{
    [SerializeField] private Grid hexgrid;
    private Tilemap land;
    private List<GameObject> detectedCharacters = new List<GameObject>();
    void Start()
    {
        land = hexgrid.transform.GetChild(1).gameObject.GetComponent<Tilemap>();

        LockToCell();
    }

    void LockToCell()
    {
        Vector3Int playerPos = land.WorldToCell(transform.position);
        Vector3 closestPlayerGridPos = land.CellToLocal(playerPos);
        transform.position = closestPlayerGridPos;
    }

    /*
    void Update()
    {
        if (TurnManager.main.currentTurn != TurnManager.turn.ENEMYSTURN) return;
        foreach (GameObject character in UnitManager.main.characters)
        {

            if(Vector3.Distance(character.transform.position, this.transform.position) < 3)
            {
                float xDist = character.transform.position.x - this.transform.position.x;
                float yDist = character.transform.position.y - this.transform.position.y;
                Debug.Log(xDist + " " + yDist);
                this.transform.position = new Vector3(this.transform.position.x + (xDist < 0 ? -1 : 1) * (xDist != 0 ? xDist / xDist : 0) , this.transform.position.y + (yDist < 0 ? -1 : 1) * (yDist != 0 ? yDist / yDist : 0), this.transform.position.z);
                LockToCell();
                TurnManager.main.ChangeTurn();
            }
        }
    }
    */

    public void OnCharacterCalled()
    {
        string foundList = this.gameObject.name + " has found: ";
        foreach (GameObject character in detectedCharacters)
        {
            foundList += character.name + ", ";
        }
        Debug.Log(foundList);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("character"))
        {
            Debug.Log("Character found!");
            detectedCharacters.Add(collision.gameObject);
        }
        else Debug.Log(collision.gameObject.name);    
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("character"))
        {
            Debug.Log("Character lost!");
            detectedCharacters.Remove(collision.gameObject);
        }
    }

}
