using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    /*
        IMPORTANT:
            The descriptions and implementations of the stats are based off of the design document
            AS OF 1/30/2026. Check the Red Lily Design Doc frequently to make sure stats are being
            accurately represented here.
    */

    [Tooltip("Higher strength reduces AP cost for strength based tasks and melee attacks")]
    [SerializeField] private float _strength;
    [Tooltip("Higher endurance increses AP and defense")]
    [SerializeField] private float _endurance;
    [Tooltip("Higher agility makes it easier to traverse difficult terrain")]
    [SerializeField] private float _agility;
    [Tooltip("Higher intellect increases healing ability (self & others)")]
    [SerializeField] private float _intellect;

    [Tooltip("Total AP is calculated using starting AP and endurance")]
    [SerializeField] private float _startingAP = 3.0f;
    private float _maxAP;
    private float _currentAP;
    private float _nimbleness;

    /*
        A NOTE FOR LATER: sneaking will cost more AP than standard walking
            (~50% for now)
    */

    // Here we define player characteristics (health, AP, etc.)
    // based on their stats.
    void Start() 
    {
        OnStatChange();
    }

    public void OnStatChange()
    {
        // Bad maths until we get real maths
        _maxAP = _startingAP + _endurance;
        _nimbleness = _agility;
    }

    public float GetCurrentAP()
    {
        return _currentAP;
    }
    public float GetNimbleness()
    {
        return _nimbleness;
    }

    public void IncreaseStrength(float amount)
    {
        _strength += amount;
        OnStatChange();
    }

    public float GetStrength()
    {
        return _strength;
    }

    public void IncreaseEndurance(float amount)
    {
        _endurance += amount;
        OnStatChange();
    }

    public float GetEndurance()
    {
        return _endurance;
    }

    public void IncreaseAgility(float amount)
    {
        _agility += amount;
        OnStatChange();
    }

    public float GetAgility()
    {
        return _agility;
    }

    public void IncreaseIntellect(float amount)
    {
        _intellect += amount;
        OnStatChange();
    }

    public float GetIntellect()
    {
        return _intellect;
    }
}
