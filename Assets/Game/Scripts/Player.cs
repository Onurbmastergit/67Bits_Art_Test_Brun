using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private bool useRootMotion;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Slider _lifeBar;
    [SerializeField] private StatusPlayer[] statusPlayers;
    [SerializeField] private GameObject[] tools;
    [SerializeField] private Image statusBg;
    [SerializeField] private Sprite[] handleImage;
    [SerializeField] private Image handleJoystick;
    [SerializeField] private Image lifeBarStatus;
    [SerializeField] private AudioSource audioSource;
    private float currentLife;
    private float baseLife = 100;
    private Vector3 initialPosition;
    public Animator Anim { get; private set; }
    private Rigidbody rigg;
    private VfxColor vfxColor;
    private bool wasMoving = false;
    private void Awake()
    {
        currentLife = baseLife;
        Anim = GetComponent<Animator>();
        Anim.applyRootMotion = useRootMotion;
        //_lifeBar.value = _lifeBar.maxValue = currentLife = 100;
        lifeBarStatus.fillAmount = baseLife;
        statusBg = GameObject.FindWithTag("PlayerActions").GetComponent<Image>();
        rigg = GetComponent<Rigidbody>();
        vfxColor = GameObject.FindWithTag("VFX").GetComponent<VfxColor>();
    }
    private void Update()
    {
        Anim.SetBool("Gathering", GroundButton.isGathering);
        if(currentLife > 0 && GroundButton.isAction == false)statusBg.sprite = statusPlayers[0].statusBg;
        var mobilejoystick = MobileJoystick.GetJoystickAxis();
        var joystick = mobilejoystick.magnitude > 0? mobilejoystick : JoystickAxis();
        var direction = new Vector3(joystick.x, 0, joystick.y);
        if(!useRootMotion) rigg.velocity = direction  * speed * Time.deltaTime;
        Anim.SetFloat("Movement", joystick.magnitude, .25f, Time.deltaTime);
        if(direction.magnitude != 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            ChangeHandleJoystick(direction.magnitude);
        }else handleJoystick.sprite = handleImage[0];
         bool isMoving = direction.magnitude != 0;

        if(isMoving && !wasMoving)
        {
            audioSource.Play();
        }
        else if(!isMoving && wasMoving)
        {
            audioSource.Pause();
        }
        wasMoving = isMoving;

        //_lifeBar.transform.position = new Vector3(transform.position.x, _lifeBar.transform.position.y, transform.position.z);
       // _lifeBar.value = Mathf.Lerp(_lifeBar.value, currentLife, Time.deltaTime * 2.5f);
       lifeBarStatus.fillAmount = Mathf.Lerp(lifeBarStatus.fillAmount, currentLife / baseLife, Time.deltaTime * 2.5f);
    }
    public void TakeDamage(int damage)
    {
        vfxColor.ChangeColorBarLife(baseLife,currentLife,lifeBarStatus);
        currentLife -= damage;
        if (currentLife <= 0) 
        {
            Anim.SetTrigger("Death");
           if(GroundButton.isAction == false) statusBg.sprite = statusPlayers[1].statusBg;
        }
        else Anim.SetTrigger("Hit");
    }
    private void ChangeHandleJoystick(float direc)
    {
         
        if(direc > 0 && direc <= 0.5)handleJoystick.sprite = handleImage[1];
        else if (direc == 1)handleJoystick.sprite = handleImage[2];     
    }
    private Vector2 JoystickAxis()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        return new Vector2(x, y);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out GroundButton Event))
        {
            Event.Cancell();
            Event.StartCoroutine(Event.Fill(transform));
        }
        if (other.TryGetComponent(out EnemyDamage Enemy))
        {
            Enemy.gameObject.SetActive(false);
            TakeDamage(30);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out GroundButton Event))
        {
            Event.Cancell();
            Event.buttonFill.fillAmount = 1;
            statusBg.sprite = statusPlayers[0].statusBg;
            OffGathering();
        } 
    }
    public void OnGathering()
    {
        GroundButton.isGathering = true;
        Vector3 newScale = new Vector3(2f, 2f, 2f);
        tools[1].transform.localScale = newScale;
    }
   
    public void OffGathering()
    {
         GroundButton.isGathering = false;
         Vector3 newScale = new Vector3(0.01f, 0.01f, 0.01f);
         tools[1].transform.localScale = newScale;
         //Anim.SetTrigger("Collect");
    }
}


[System.Serializable]
public class StatusPlayer
{
    public string Status;
    public Sprite statusBg;
}
