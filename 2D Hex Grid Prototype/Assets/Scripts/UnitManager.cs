/*
    Manages the currently active units.
    To make sure a unit is active:
        - Attach this script to an empty object
        - Make the player characters children objects to the empty object
        - MAKE SURE they have the character script attached to them.
*/

using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private GameObject sneakButton;
    [SerializeField] private GameObject moveButton;
    public static UnitManager main {get; private set;}
    public List<GameObject> characters = new List<GameObject>();

    private void Awake()
    {
        if (main != null)
        {
            Destroy(main.gameObject);
            return;
        }
        main = this;
        DontDestroyOnLoad(this.gameObject);
    }

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
        GameObject player = CheckIfAnySelected();
        if (player != null)
        {
            sneakButton.SetActive(true);
            moveButton.SetActive(true);
        }
        else
        {
            sneakButton.SetActive(false);
            moveButton.SetActive(false);
        }
    }

    public GameObject CheckIfAnySelected()
    {
        foreach (GameObject dude in characters)
        {
            if (dude.GetComponent<character>().CheckIfSelected())
            {
                return dude;
            }
        }
        return null;
    }

    public void OnSneakPressed()
    {
        GameObject selectedCharacter = CheckIfAnySelected();
        if (selectedCharacter != null)
        {
            selectedCharacter.GetComponent<character>().ToggleSneak();
        }
    }

    public void OnMovePressed()
    {
        GameObject selectedCharacter = CheckIfAnySelected();
        if (selectedCharacter != null)
        {
            selectedCharacter.GetComponent<character>().ToggleMovement();
        }
    }
}
