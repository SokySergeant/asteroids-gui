using System;
using UnityEditor;
using Asteroids;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(AsteroidSpawner))]
public class AsteroidSpawnerEditor : Editor
{
    public VisualTreeAsset uxml;
    private GameData gameData;

    private MinMaxSlider spawnTimeSlider;
    private FloatField minSpawnTime;
    private FloatField maxSpawnTime;



    public override VisualElement CreateInspectorGUI()
    {
        gameData = ((AsteroidSpawner)target).gameData;
        
        //add UIBuilder elements
        var root = new VisualElement();
        uxml.CloneTree(root);
        
        spawnTimeSlider = root.Q<MinMaxSlider>("spawnTime");
        minSpawnTime = root.Q<FloatField>("minSpawnTime");
        maxSpawnTime = root.Q<FloatField>("maxSpawnTime");
    
        //set default values
        spawnTimeSlider.minValue = gameData.minSpawnTime;
        spawnTimeSlider.maxValue = gameData.maxSpawnTime;
        minSpawnTime.value = gameData.minSpawnTime;
        maxSpawnTime.value = gameData.maxSpawnTime;
        
        //assign events
        spawnTimeSlider.RegisterValueChangedCallback(e =>
        {
            UpdateSpawnTimeMin(e.newValue.x);
            UpdateSpawnTimeMax(e.newValue.y);
        });
        
        minSpawnTime.RegisterValueChangedCallback(e =>
        {
            UpdateSpawnTimeMin(e.newValue);
        });
        
        maxSpawnTime.RegisterValueChangedCallback(e =>
        {
            UpdateSpawnTimeMax(e.newValue);
        });

        //InspectorElement.FillDefaultInspector(root, serializedObject, this); //unity default
        return root;
    }



    private void UpdateSpawnTimeMin(float min)
    {
        EditorUtility.SetDirty(gameData);
        min = Truncate(min, 2);
        min = Mathf.Clamp(min, 0.1f, gameData.maxSpawnTime);
        
        gameData.minSpawnTime = min;
        minSpawnTime.value = min;
        spawnTimeSlider.minValue = min;
    }
    
    
    
    private void UpdateSpawnTimeMax(float max)
    {
        EditorUtility.SetDirty(gameData);
        max = Truncate(max, 2);
        max = Mathf.Clamp(max, gameData.minSpawnTime, 50f);
        
        
        gameData.maxSpawnTime = max;
        maxSpawnTime.value = max;
        spawnTimeSlider.maxValue = max;
    }



    private float Truncate(float num, int decimals)
    {
        return float.Parse(num.ToString("F2"));
    }
}
