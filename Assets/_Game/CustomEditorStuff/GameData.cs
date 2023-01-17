using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SOs/GameData")]
public class GameData : ScriptableObject
{
    [Min(0.1f)] public float minSpawnTime;
    [Min(0.1f)] public float maxSpawnTime;
    public int minAmount;
    public int maxAmount;
    
    public List<AsteroidData> asteroids;
}
