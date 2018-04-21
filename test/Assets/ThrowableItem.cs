using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ThrowableItem))]
public class SomeEditor : Editor
{
    
    int _choiceIndex = 0;

    private string choiceToResourcePath(string choice)
    {
        return (choice == "beef")   ? "items/beef.png" :
               (choice == "carrot") ? "items/carrot.png":
                                      "items/bread.png";
    }

    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        _choiceIndex = EditorGUILayout.Popup(_choiceIndex, ThrowableItem._choices);
        var someClass = target as ThrowableItem;

        // Update the selected choice in the underlying object
        someClass.ResourcePath = ThrowableItem._choices[_choiceIndex];

        // Save the changes back to the object
        EditorUtility.SetDirty(target);
    }
}

public class ThrowableItem : MonoBehaviour
{
    // -----------------------------------------------------------------------------------
    // Static 
    // -----------------------------------------------------------------------------------

    public static string[] _choices = new[] { "beef", "carrot" };
    private static string choiceToResourcePath(string choice)
    {
        return (choice == "beef")   ? "items/beef.png" :
               (choice == "carrot") ? "items/carrot.png" :
                                      "items/bread.png";
    }

    // -----------------------------------------------------------------------------------
    // Instance
    // -----------------------------------------------------------------------------------

    private string resourcePath;
    public string ResourcePath
    {
        get { return resourcePath; }
        set { resourcePath = ThrowableItem.choiceToResourcePath(value); }
    }
    private Texture2D texture;

    public ThrowableItem(string choice)
    {
        ResourcePath = choice;
    }

    // Use this for initialization
    void Start()
    {
        texture = (Texture2D) Resources.Load(resourcePath);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
