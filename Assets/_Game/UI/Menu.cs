using UnityEngine;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{

    [SerializeField] private UIDocument doc;
    private TextField textField;

    private string oldVal;
    private string newVal;

    

    void Awake()
    {
        var root = doc.rootVisualElement;

        textField = root.Q<TextField>("Logger");

        textField.RegisterValueChangedCallback(e =>
        {
            oldVal = e.previousValue;
            newVal = e.newValue;
        });

        var btn = root.Q<Button>("Log");
        btn.clicked += LogThing;
    }



    private void LogThing()
    {
        Debug.Log(oldVal);
        Debug.Log(newVal);
    }
}
