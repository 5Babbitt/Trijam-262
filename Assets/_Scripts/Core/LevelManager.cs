using FiveBabbittGames;
using UnityEngine;

/// <summary>
/// LevelManager
/// </summary>
public class LevelManager : Singleton<LevelManager>
{
    [field: SerializeField] public int enemiesToKill {  get; private set; }
    [SerializeField] private int enemiesKilled;

    protected override void Awake()
    {
        base.Awake();

    }

    private void Start()
    {
        
    }

    public void OnEnemyKilled(Component component, object data)
    {

    }
}
