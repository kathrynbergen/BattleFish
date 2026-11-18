using UnityEngine;

public class AbilityStats : MonoBehaviour
{
    public AbilityData data;

    public string abilityName;
    public int Damage { get; private set; }

    // Sets initial values based on the ability's data
    public void Initialize(AbilityData abilityData)
    {
        if (abilityData == null)
        {
            Debug.LogError("Missing AbilityData!");
            return;
        }

        data = abilityData;
        abilityName = data.abilityName;
        Damage = data.damage;
    }
}