using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private List<GameObject> characters = new List<GameObject>();

    void Start()
    {
        // Add every character in unit to the list
        if(this.transform.childCount > 0) {
            foreach (Transform child in this.transform)
            {
                if (child.CompareTag("character"))
                {
                    characters.Add(child.gameObject);
                }
            }
        }
    }


    void Update()
    {
        foreach (GameObject dude in characters)
        {
            // Here we can reference different scripts that we will want to run

            // The player needs to be selected and move
            // When the player is selected, they and only they can move
            // A highlighted portion of the map should be displayed in order to show
            //      where the player can move
            // The player will also have other actions
            // Such as attack
        }
    }
}
