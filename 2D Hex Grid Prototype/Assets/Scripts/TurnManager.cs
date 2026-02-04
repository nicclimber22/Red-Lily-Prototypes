/*
    This class serves as a universal source of truth
    regarding the state of the game.
    Refer to here and here alone for the current turn.
*/

using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager main {get; private set;}

    public enum turn {PLAYERSTURN, ENEMYSTURN};
    public turn currentTurn;

    [SerializeField] private GameObject endTurnButton;

    // Ensure that only one version of this file is active at a time.
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
        currentTurn = turn.PLAYERSTURN;
    }

    void Update()
    {
        if (currentTurn == turn.PLAYERSTURN) endTurnButton.SetActive(true);
        else endTurnButton.SetActive(false);
    }

    public void OnEndTurnPressed()
    {
        currentTurn = turn.ENEMYSTURN;
    }

    public void ChangeTurn()
    {
        if (currentTurn == turn.PLAYERSTURN) currentTurn = turn.ENEMYSTURN;
        else currentTurn = turn.PLAYERSTURN;
    }
}
