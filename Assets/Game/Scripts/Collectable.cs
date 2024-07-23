using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[HelpURL("https://doc.clickup.com/9017157017/p/h/8cqdtct-30337/d473739efa49988")]
public class Collectable : MonoBehaviour
{
    [SerializeField] private AnimationCurve movementCurve;
    [SerializeField] private float movementSpeed = 2.5f;

    [Header("Events")]
    [SerializeField] private UnityEvent _onSpawn;
    [SerializeField] private UnityEvent _onStartMove;
    [SerializeField] private UnityEvent _onCollect;
    [SerializeField]private Player player;

    Vector3 startPoint;
    private void Awake()
    {
        startPoint = transform.position;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _onSpawn.Invoke();
    }

    
    public void Collect()
    {
        StopAllCoroutines();
        StartCoroutine(MoveToCollect());
    }

    private IEnumerator MoveToCollect()
    {
         
        float lerpValue = 0;

        _onStartMove.Invoke();

        player.Anim.SetTrigger("Collect");
        while (lerpValue < 1)
        {
            
            lerpValue += Time.deltaTime * movementSpeed;
            transform.position = Vector3.Lerp(startPoint, player.transform.position, movementCurve.Evaluate(lerpValue));
            yield return new WaitForEndOfFrame();
        }
        
        _onCollect.Invoke();
        

        yield return new WaitForSeconds(1);
        _onSpawn.Invoke();
        transform.position = startPoint;
    }
}
