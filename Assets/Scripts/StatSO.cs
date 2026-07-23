using UnityEngine;

[CreateAssetMenu(fileName = "StatSO", menuName = "Scriptable Objects/StatSO")]
public class StatSO : ScriptableObject
{
    public float MaxHealth;
    public float Damage;
    public float Defend;
    public float MovementSpeed;
}
