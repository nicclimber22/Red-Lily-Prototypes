using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public static TurnManager main {get; private set;}

    public enum turn {PLAYERSTURN, ENEMYSTURN};
    public turn currentTurn;
    public int round = 0;

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
}
