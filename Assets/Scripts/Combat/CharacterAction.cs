using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    protected CharacterStats myStats;

    private void Awake()
    {
        myStats = GetComponent<CharacterStats>();
    }

    public virtual void OnTurnAction(List<CharacterStats> allies, List<CharacterStats> enemies)
    {
        print(myStats.data.characterName + "'s turn");
    }

    public void DealDamage(CharacterStats target, int damageAmount)
    {
        target.Health = Mathf.Max(0, target.Health - damageAmount);
    }
}
