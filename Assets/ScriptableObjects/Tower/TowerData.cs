using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Scriptable Objects/TowerData")]
public class TowerData : ScriptableObject
{
    public float range;
    public float shootInterval;
    public float projectileSpeed;
    public float projectileDuration;
    public float damage;
    public float knockback;
    public float slowDown;
    public float dot;
    public int cost;
    public string description;
    public Sprite sprite;
    public GameObject prefab;
}
