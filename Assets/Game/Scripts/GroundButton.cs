using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[HelpURL("https://doc.clickup.com/9017157017/p/h/8cqdtct-30337/d473739efa49988")]
public class GroundButton : MonoBehaviour
{
    [Tooltip("The events will be played in order")]
    public EventWithDelay[] Events;

    [Header("Settings")]
    [SerializeField] private List<OptionObject> OptionsActions;
    [SerializeField] public Image buttonFill;
    [SerializeField] private float timeAction = 1.5f;
    [SerializeField] public static bool isAction;
    [SerializeField] public static bool isGathering;
    [SerializeField] private CollectableStatus[] collectableStatus;
    
    private void Awake()
    {
        buttonFill = GameObject.FindWithTag("PlayerActions").GetComponent<Image>(); 
    }
    public void Cancell()
    {
         for(int i = 0; i < OptionsActions.Count; i++)
            {
                if(OptionsActions[i].isObject == true)
                {
                 if(OptionsActions[i].Name == "CollectableObject")
                 {
                    collectableStatus[0].statusCollectable.SetActive(true);
                    collectableStatus[1].statusCollectable.SetActive(false);
                    collectableStatus[2].statusCollectable.SetActive(false); 
                 }
                }
            }
        
        buttonFill.fillAmount = 1;
        StopAllCoroutines();
    }

    public IEnumerator Fill(Transform player)
    {
        isAction = true;
        if(OptionsActions.Count > 0)
        {
             for(int i = 0; i < OptionsActions.Count; i++)
        {
            if(OptionsActions[i].isObject == true)
            {
                if(OptionsActions[i].actionHud != null)buttonFill.sprite = OptionsActions[i].actionHud;
                break;
            }
        }
        }
        if(player != null)buttonFill.fillAmount = 0;

        while (buttonFill.fillAmount < 1)
        {
            
            for(int i = 0; i < OptionsActions.Count; i++)
            {
                if(OptionsActions[i].isObject == true)
                {
                 if(OptionsActions[i].Name == "CollectableObject")
                 {
                    player.GetComponent<Player>().OnGathering();
                    for(int j = 0; j < collectableStatus.Length; j ++)
                    {
                        int statusIndex = (buttonFill.fillAmount > 0.6f && buttonFill.fillAmount < 0.8) ? 2 :
                              (buttonFill.fillAmount > 0.4f) ? 1 : 0;
                        collectableStatus[j].statusCollectable.SetActive(j == statusIndex);
                        if(buttonFill.fillAmount >= 0.8f)collectableStatus[j].statusCollectable.SetActive(false);  
                    } 
                 }
                }
            }
            buttonFill.fillAmount += Time.deltaTime/timeAction;
            yield return new WaitForEndOfFrame();
        }
        //player.transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        //player.transform.forward = transform.forward;
        buttonFill.fillAmount = 1;
        isAction = false;
        StartCoroutine(PlayEvents());
    }

    public IEnumerator PlayEvents()
    {
        foreach(EventWithDelay Event in Events)
        {
            yield return new WaitForSeconds(Event.Delay);
            Event.Events.Invoke();
        }
    }
}
[Serializable]
public class EventWithDelay
{
    public string Name =  "New Event";
    [Tooltip("Event delay in seconds")]
    public float Delay;
    public UnityEvent Events;
}
[System.Serializable]
    public class OptionObject
{
    public string Name;
    public Sprite actionHud;
    public bool isObject;
    
}

[System.Serializable]
public class CollectableStatus
{
    public string Name;
    public GameObject statusCollectable;
}
