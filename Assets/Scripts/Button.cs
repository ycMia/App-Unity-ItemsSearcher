using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public Manager gM;
    public InputField if1;
    public InputField if2;
    void Start()
    {
        
    }
    
    public void MyClick()
    {
        gM.Insert(gM.g_dbName, gM.g_tbName, gM.FindMaximalId(gM.g_dbName, gM.g_tbName)+1, if1.text, if2.text);
        gM.ReadTable_All(gM.g_dbName, gM.g_tbName);
    }

    void Update()
    {
        
    }
}
