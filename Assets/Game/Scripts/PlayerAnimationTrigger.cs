using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    [SerializeField] private string _animatorTriggerName;

    [System.Obsolete]
    private void OnEnable()
    {
        FindObjectOfType<Player>().Anim.SetTrigger(_animatorTriggerName);
        gameObject.SetActive(false);
    }
}
