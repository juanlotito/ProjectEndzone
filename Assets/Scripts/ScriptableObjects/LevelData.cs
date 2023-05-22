using UnityEngine;

[CreateAssetMenu(menuName = "Data/LevelData")]
public class LevelData : ScriptableObject
{
    public int maxZombies = 5;
    public float spawnTime = 2f;
}
