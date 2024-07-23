using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VfxColor : MonoBehaviour
{
    Color32 collorRed = new Color32(249, 6, 0, 255); 
    Color32 collorGreen = new Color32(0, 249, 22, 255);   
    Color32 collorYellow = new Color32(249, 192, 0, 255);

    public  void ChangeColorBarLife(float baseLife, float currentLife,Image lifeBarStatus)
    {
        Color32 corAtual = (currentLife <= baseLife * 0.4f) ? collorRed :
                       (currentLife >= baseLife * 0.8f) ? collorGreen : collorYellow;
        lifeBarStatus.color = corAtual;
    }  
}
