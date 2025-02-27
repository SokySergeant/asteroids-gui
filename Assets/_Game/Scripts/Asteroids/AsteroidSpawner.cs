﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids
{
    public class AsteroidSpawner : MonoBehaviour
    {
        public GameData gameData;

        private float _timer;
        private float _nextSpawnTime;
        private Camera _camera;
        
        public GameObject asteroidPrefab;

        [HideInInspector] public bool bogus;


        private enum SpawnLocation
        {
            Top,
            Bottom,
            Left,
            Right
        }
        
        

        private void Start()
        {
            _camera = Camera.main;
            Spawn();
            UpdateNextSpawnTime();
        }
        
        

        private void Update()
        {
            UpdateTimer();

            if (!ShouldSpawn())
                return;

            Spawn();
            UpdateNextSpawnTime();
            _timer = 0f;
        }

        
        
        private void UpdateNextSpawnTime()
        {
            _nextSpawnTime = Random.Range(gameData.minSpawnTime, gameData.maxSpawnTime);
        }

        
        
        private void UpdateTimer()
        {
            _timer += Time.deltaTime;
        }

        
        
        private bool ShouldSpawn()
        {
            return _timer >= _nextSpawnTime;
        }

        
        
        private void Spawn()
        {
            var amount = Random.Range(gameData.minAmount, gameData.maxAmount + 1);
            
            for (var i = 0; i < amount; i++)
            {
                var location = GetSpawnLocation();
                var position = GetStartPosition(location);
                var tempAsteroid = Instantiate(asteroidPrefab, position, Quaternion.identity).GetComponent<Asteroid>();

                int randInd = Random.Range(0, gameData.asteroids.Count);
                tempAsteroid.minForce = gameData.asteroids[randInd].minForce;
                tempAsteroid.maxForce = gameData.asteroids[randInd].maxForce;
                tempAsteroid.minSize = gameData.asteroids[randInd].minSize;
                tempAsteroid.maxSize = gameData.asteroids[randInd].maxSize;
                tempAsteroid.minTorque = gameData.asteroids[randInd].minTorque;
                tempAsteroid.maxTorque = gameData.asteroids[randInd].maxTorque;
                tempAsteroid.minChildAmount = gameData.asteroids[randInd].minChildAmount;
                tempAsteroid.maxChildAmount = gameData.asteroids[randInd].maxChildAmount;
                tempAsteroid.gameObject.GetComponentInChildren<SpriteRenderer>().color = gameData.asteroids[randInd].color;
            }
        }

        
        
        private static SpawnLocation GetSpawnLocation()
        {
            var roll = Random.Range(0, 4);

            return roll switch
            {
                1 => SpawnLocation.Bottom,
                2 => SpawnLocation.Left,
                3 => SpawnLocation.Right,
                _ => SpawnLocation.Top
            };
        }

        private Vector3 GetStartPosition(SpawnLocation spawnLocation)
        {
            var pos = new Vector3 { z = Mathf.Abs(_camera.transform.position.z) };
            
            const float padding = 5f;
            switch (spawnLocation)
            {
                case SpawnLocation.Top:
                    pos.x = Random.Range(0f, Screen.width);
                    pos.y = Screen.height + padding;
                    break;
                case SpawnLocation.Bottom:
                    pos.x = Random.Range(0f, Screen.width);
                    pos.y = 0f - padding;
                    break;
                case SpawnLocation.Left:
                    pos.x = 0f - padding;
                    pos.y = Random.Range(0f, Screen.height);
                    break;
                case SpawnLocation.Right:
                    pos.x = Screen.width - padding;
                    pos.y = Random.Range(0f, Screen.height);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(spawnLocation), spawnLocation, null);
            }
            
            return _camera.ScreenToWorldPoint(pos);
        }
        
        
    }
}
