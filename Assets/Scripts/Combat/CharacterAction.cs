using System;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    protected CharacterStats myStats;

    private void Awake()
    {
        myStats = GetComponent<CharacterStats>();
    }

    public virtual void OnTurnAction() { }
}
