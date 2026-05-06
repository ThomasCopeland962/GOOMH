using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public float lives;
    public int damage;
    public float speed;
    public float reward;
    public bool flying;
    public AudioClip deathSound;
}
