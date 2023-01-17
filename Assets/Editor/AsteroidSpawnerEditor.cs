using System.Collections.Generic;
using UnityEditor;
using Asteroids;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(AsteroidSpawner))]
public class AsteroidSpawnerEditor : Editor
{
    private VisualTreeAsset _uxml;
    private GameData _gameData;

    private MinMaxSlider _spawnTimeSlider;
    private FloatField _minSpawnTime;
    private FloatField _maxSpawnTime;

    private MinMaxSlider _asteroidAmountSlider;
    private IntegerField _minAsteroidAmount;
    private IntegerField _maxAsteroidAmount;

    private GroupBox _asteroidsBox;
    private VisualTreeAsset _asteroidContainer;
    private List<VisualElement> _tempAsteroids = new List<VisualElement>();



    public override VisualElement CreateInspectorGUI()
    {
        _gameData = ((AsteroidSpawner)target).gameData;
        
        var root = new VisualElement();

        _uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_Game/CustomEditorStuff/AsteroidSpawnerEditorUxml.uxml");
        _uxml.CloneTree(root);
        
        CreateAsteroidContainer(root);
        
        GetElements(root);
        
        SetDefaultValues();

        AssignEvents();
        
        return root;
    }



    private VisualElement CreateTempAsteroid(int i, VisualElement root)
    {
        var tempAsteroid = new VisualElement();
        _asteroidContainer.CloneTree(tempAsteroid);
            
        var asteroidIndexEl = tempAsteroid.Q<Foldout>("asteroidIndex");
        var minForceEl = tempAsteroid.Q<FloatField>("minForce");
        var maxForceEl = tempAsteroid.Q<FloatField>("maxForce");
        var minSizeEl = tempAsteroid.Q<FloatField>("minSize");
        var maxSizeEl = tempAsteroid.Q<FloatField>("maxSize");
        var minTorqueEl = tempAsteroid.Q<FloatField>("minTorque");
        var maxTorqueEl = tempAsteroid.Q<FloatField>("maxTorque");
        var deleteBtnEl = tempAsteroid.Q<Button>("deleteBtn");

        asteroidIndexEl.text = "Asteroid #" + (i + 1);
        minForceEl.value = _gameData.asteroids[i].minForce;
        maxForceEl.value = _gameData.asteroids[i].maxForce;
        minSizeEl.value = _gameData.asteroids[i].minSize;
        maxSizeEl.value = _gameData.asteroids[i].maxSize;
        minTorqueEl.value = _gameData.asteroids[i].minTorque;
        maxTorqueEl.value = _gameData.asteroids[i].maxTorque;

        minForceEl.RegisterValueChangedCallback(e =>
        {
            UpdateMinForce(i, e.newValue);
        });
        maxForceEl.RegisterValueChangedCallback(e =>
        {
            UpdateMaxForce(i, e.newValue);
        });
        minSizeEl.RegisterValueChangedCallback(e =>
        {
            UpdateMinSize(i, e.newValue);
        });
        maxSizeEl.RegisterValueChangedCallback(e =>
        {
            UpdateMaxSize(i, e.newValue);
        });
        minTorqueEl.RegisterValueChangedCallback(e =>
        {
            UpdateMinTorque(i, e.newValue);
        });
        maxTorqueEl.RegisterValueChangedCallback(e =>
        {
            UpdateMaxTorque(i, e.newValue);
        });
        deleteBtnEl.clicked += () =>
        {
            _gameData.asteroids.RemoveAt(i);
            CreateAsteroidContainer(root);
        };

        return tempAsteroid;
    }
    


    private void CreateAsteroidContainer(VisualElement root)
    {
        _asteroidContainer = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_Game/CustomEditorStuff/AsteroidContainerUxml.uxml");
        _asteroidsBox = root.Q<GroupBox>("asteroidsBox");
        _asteroidsBox.Clear();
        _tempAsteroids.Clear();
        for (int i = 0; i < _gameData.asteroids.Count; i++)
        {
            var tempAsteroid = CreateTempAsteroid(i, root);
            _tempAsteroids.Add(tempAsteroid);
            _asteroidsBox.Add(tempAsteroid);
        }
    }
    


    private void GetElements(VisualElement root)
    {
        _spawnTimeSlider = root.Q<MinMaxSlider>("spawnTime");
        _minSpawnTime = root.Q<FloatField>("minSpawnTime");
        _maxSpawnTime = root.Q<FloatField>("maxSpawnTime");
        _asteroidAmountSlider = root.Q<MinMaxSlider>("asteroidAmount");
        _minAsteroidAmount = root.Q<IntegerField>("minAsteroidAmount");
        _maxAsteroidAmount = root.Q<IntegerField>("maxAsteroidAmount");
    }



    private void SetDefaultValues()
    {
        _spawnTimeSlider.minValue = _gameData.minSpawnTime;
        _spawnTimeSlider.maxValue = _gameData.maxSpawnTime;
        _minSpawnTime.value = _gameData.minSpawnTime;
        _maxSpawnTime.value = _gameData.maxSpawnTime;

        _asteroidAmountSlider.minValue = _gameData.minAmount;
        _asteroidAmountSlider.maxValue = _gameData.maxAmount;
        _minAsteroidAmount.value = _gameData.minAmount;
        _maxAsteroidAmount.value = _gameData.maxAmount;
    }



    private void AssignEvents()
    {
        _spawnTimeSlider.RegisterValueChangedCallback(e =>
        {
            UpdateSpawnTimeMin(e.newValue.x);
            UpdateSpawnTimeMax(e.newValue.y);
        });
        _minSpawnTime.RegisterValueChangedCallback(e =>
        {
            UpdateSpawnTimeMin(e.newValue);
        });
        _maxSpawnTime.RegisterValueChangedCallback(e =>
        {
            UpdateSpawnTimeMax(e.newValue);
        });

        _asteroidAmountSlider.RegisterValueChangedCallback(e =>
        {
            UpdateAsteroidMin((int)Mathf.Floor(e.newValue.x));
            UpdateAsteroidMax((int)Mathf.Floor(e.newValue.y));
        });
        _minAsteroidAmount.RegisterValueChangedCallback(e =>
        {
            UpdateAsteroidMin(e.newValue);
        });
        _maxAsteroidAmount.RegisterValueChangedCallback(e =>
        {
            UpdateAsteroidMax(e.newValue);
        });
    }
    

    
    private void UpdateSpawnTimeMin(float min)
    {
        EditorUtility.SetDirty(_gameData);
        min = Truncate(min);
        min = Mathf.Clamp(min, _spawnTimeSlider.lowLimit, _gameData.maxSpawnTime);
        
        _gameData.minSpawnTime = min;
        _minSpawnTime.value = min;
        _spawnTimeSlider.minValue = min;
    }
    
    private void UpdateSpawnTimeMax(float max)
    {
        EditorUtility.SetDirty(_gameData);
        max = Truncate(max);
        max = Mathf.Clamp(max, _gameData.minSpawnTime, _spawnTimeSlider.highLimit);
        
        
        _gameData.maxSpawnTime = max;
        _maxSpawnTime.value = max;
        _spawnTimeSlider.maxValue = max;
    }
    
    
    
    private void UpdateAsteroidMin(int min)
    {
        EditorUtility.SetDirty(_gameData);
        min = Mathf.Clamp(min, (int)Mathf.Floor(_asteroidAmountSlider.lowLimit), _gameData.maxAmount);
        _gameData.minAmount = min;
        _minAsteroidAmount.value = min;
        _asteroidAmountSlider.minValue = min;
    }
    
    private void UpdateAsteroidMax(int max)
    {
        EditorUtility.SetDirty(_gameData);
        max = Mathf.Clamp(max, _gameData.minAmount, (int)Mathf.Floor(_asteroidAmountSlider.highLimit));
        _gameData.maxAmount = max;
        _maxAsteroidAmount.value = max;
        _asteroidAmountSlider.maxValue = max;
    }



    private void UpdateMinForce(int index, float value)
    {
        EditorUtility.SetDirty(_gameData);
        value = Mathf.Clamp(value, 0f, _gameData.asteroids[index].maxForce);
        _tempAsteroids[index].Q<FloatField>("minForce").value = value;
        _gameData.asteroids[index].minForce = value;
    }
    
    private void UpdateMaxForce(int index, float value)
    {
        EditorUtility.SetDirty(_gameData);
        value = Mathf.Clamp(value, _gameData.asteroids[index].minForce, Mathf.Infinity);
        _tempAsteroids[index].Q<FloatField>("maxForce").value = value;
        _gameData.asteroids[index].maxForce = value;
    }
    
    
    
    private void UpdateMinSize(int index, float value)
    {
        EditorUtility.SetDirty(_gameData);
        value = Mathf.Clamp(value, 0f, _gameData.asteroids[index].maxSize);
        _tempAsteroids[index].Q<FloatField>("minSize").value = value;
        _gameData.asteroids[index].minSize = value;
    }
    
    private void UpdateMaxSize(int index, float value)
    {
        EditorUtility.SetDirty(_gameData);
        value = Mathf.Clamp(value, _gameData.asteroids[index].minSize, Mathf.Infinity);
        _tempAsteroids[index].Q<FloatField>("maxSize").value = value;
        _gameData.asteroids[index].maxSize = value;
    }
    
    
    
    private void UpdateMinTorque(int index, float value)
    {
        EditorUtility.SetDirty(_gameData);
        value = Mathf.Clamp(value, 0f, _gameData.asteroids[index].maxTorque);
        _tempAsteroids[index].Q<FloatField>("minTorque").value = value;
        _gameData.asteroids[index].minTorque = value;
    }
    
    private void UpdateMaxTorque(int index, float value)
    {
        EditorUtility.SetDirty(_gameData);
        value = Mathf.Clamp(value, _gameData.asteroids[index].minTorque, Mathf.Infinity);
        _tempAsteroids[index].Q<FloatField>("maxTorque").value = value;
        _gameData.asteroids[index].maxTorque = value;
    }
    


    private float Truncate(float num)
    {
        return float.Parse(num.ToString("F2"));
    }
}
