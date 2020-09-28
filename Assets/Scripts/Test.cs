using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    //make references to UI elements
    public Toggle toggle;
    public InputField intField;
    public InputField stringField;

    //save file name
    const string fileName = "SaveFile";
    //save path, it will be constructed in Awake
    string path;

    //variable to store all your saved values
    SavedValues savedValues;

    //Construct path
    private void Awake()
    {
        path = Application.persistentDataPath + "/" + fileName;
    }

    //populate UI elements with values from save
    void RefreshUI()
    {
        toggle.isOn = savedValues.toggleValue;
        intField.text = savedValues.intValue.ToString();
        stringField.text = savedValues.stringValue;
    }

    //Create save method
    public void Save()
    {
        //collect the values from ui
        savedValues.toggleValue = toggle.isOn;
        savedValues.intValue = int.Parse(intField.text);
        savedValues.stringValue = stringField.text;
        //save the values to a file
        SaveManager.Instance.Save(savedValues, path, SaveComplete, false);
    }

    //this method will be called after save process is done
    private void SaveComplete(SaveResult result, string message)
    {
        //check for error
        if(result == SaveResult.Error)
        {
            Debug.LogError("Save error " + message);
        }

        //if no error save was successful

    }

    //Load data from file
    public void Load()
    {
        SaveManager.Instance.Load<SavedValues>(path, LoadComplete, false);
    }

    //this method will be called when load process is done
    private void LoadComplete(SavedValues data, SaveResult result, string message)
    {
        //result is success-> load your saved data into variables
        if(result == SaveResult.Success)
        {
            savedValues = data;
        }

        //if for some reason your load failed, create an empty data to work with inside your game or give a message to the user
        if(result == SaveResult.Error || result == SaveResult.EmptyData)
        {
            savedValues = new SavedValues();
        }

        //after load is done refresh the UI
        RefreshUI();
    }
}
