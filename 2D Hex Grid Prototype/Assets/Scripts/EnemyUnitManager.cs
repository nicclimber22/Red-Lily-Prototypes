using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitManager : MonoBehaviour
{
    public static EnemyUnitManager main {get; private set;}
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
                if (child.CompareTag("enemy"))
                {
                    characters.Add(child.gameObject);
                }
            }
        }
    }

    void Update()
    {
        if (TurnManager.main.currentTurn != TurnManager.turn.ENEMYSTURN) return;
        foreach (GameObject enemy in characters)
        {
            enemy.GetComponent<enemy>()?.OnCharacterCalled();
        }
        TurnManager.main.ChangeTurn();
    }

}
