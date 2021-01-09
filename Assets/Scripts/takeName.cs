using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardGame.Data;

public class takeName : MonoBehaviour
{
    //enter name to SaveFile
    public void nameEntry(string name)
    {
        SaveFile saveFile = new SaveFile();
        saveFile.name = name;
    }
}
