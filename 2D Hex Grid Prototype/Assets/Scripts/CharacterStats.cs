using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private float _strength;
    [SerializeField] private float _endurance;
    [SerializeField] private float _agility;
    [SerializeField] private float _intellect;

    [SerializeField] private float _startingAP = 3.0f;
    private float _maxAP;
    private float _currentAP;
    private float _nimbleness;

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

    public void IncreaseStrength(float amount)
    {
        _strength += amount;
        OnStatChange();
    }

    public void IncreaseEndurance(float amount)
    {
        _endurance += amount;
        OnStatChange();
    }

    public void IncreaseAgility(float amount)
    {
        _agility += amount;
        OnStatChange();
    }

    public void IncreaseIntellect(float amount)
    {
        _intellect += amount;
        OnStatChange();
    }
}
