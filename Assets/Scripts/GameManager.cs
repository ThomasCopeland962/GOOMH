using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    public static event Action<int> OnLiveschanged;
    public static event Action<int> OnResourcesChanged;
    private int _lives = 10;
    private int _resources = 100;
    public int Resources => _resources;

    private void Awake()
    { 
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        Enemy.OnEnemyReachedEnd += HandleEnemyReachedEnd;
        Enemy.OnEnemyDestroyed += HandleEnemyDestroyed;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyReachedEnd -= HandleEnemyReachedEnd;
        Enemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
    }

    private void Start()
    {
        OnLiveschanged?.Invoke(_lives);
        OnResourcesChanged?.Invoke(_resources);
    }

    private void HandleEnemyReachedEnd(EnemyData data)
    {
        _lives = Mathf.Max(0, _lives - data.damage);
        OnLiveschanged?.Invoke(_lives);
    }

    private void HandleEnemyDestroyed(Enemy enemy)
    {
        AddResources(Mathf.RoundToInt(enemy.Data.reward));
    }
    private void AddResources(int amount)
    {
        _resources += amount;
        OnResourcesChanged?.Invoke(_resources);
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public void SpendResources(int ammount)
    {
        if(_resources >= ammount)
        {
            _resources -= ammount;
            OnResourcesChanged?.Invoke(_resources);
        }
    }
}
