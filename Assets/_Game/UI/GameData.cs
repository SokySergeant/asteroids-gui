using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SOs/AsteroidData")]
public class GameData : ScriptableObject
{
    public float minSpawnTime;
    public float maxSpawnTime;
    public int minAmount;
    public int maxAmount;
    
    public List<SingleAsteroidData> asteroids;
}
