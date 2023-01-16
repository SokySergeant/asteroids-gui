using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SOs/GameData")]
public class GameData : ScriptableObject
{
    public float minSpawnTime;
    public float maxSpawnTime;
    public int minAmount;
    public int maxAmount;
    
    public List<AsteroidData> asteroids;
}
