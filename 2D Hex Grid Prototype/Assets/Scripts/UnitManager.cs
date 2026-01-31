using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager main {get; private set;}
    private List<GameObject> characters = new List<GameObject>();

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
}
