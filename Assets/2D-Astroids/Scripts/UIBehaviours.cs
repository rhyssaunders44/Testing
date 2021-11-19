using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBehaviours : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }
}
