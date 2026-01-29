using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public enum turn {PLAYERSTURN, ENEMYSTURN};
    [SerializeField] private turn currentTurn;
    public int round = 0;

    public static TurnManager inst;
    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
    }

    void Start()
    {
        currentTurn = turn.PLAYERSTURN;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEndTurnPressed()
    {
        Debug.Log("Player has ended their turn");
        currentTurn = turn.ENEMYSTURN;
    }
}
