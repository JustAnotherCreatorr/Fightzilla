using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Data/Attack")]
public class AttackSO : ScriptableObject
{
    public string attackName;
    public float damage;
}
