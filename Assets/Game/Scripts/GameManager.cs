using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Configuração do clima/tempo")]
    public float dayDuration = 10f;
    public float minRainDuration = 5f;
    public float maxRainDuration = 15f;
    public float rainInterval = 20f;
    public float lightningChance = 0.5f; // Chance de 50% de ter raios durante a chuva
    public float fogChance = 0.5f; // Chance de 50% de ter névoa durante a chuva

    [Header("Objetos em cena")]
    public GameObject sun;
    public AudioSource rain;
    public GameObject musicDay;
    public GameObject musicNight;
    public Light directionalLight; // Luz da cena para sol/lua
    public ParticleSystem rainEffect; // Partícula de chuva
    public GameObject lightningObject; // GameObject para o raio
    public GameObject fogEffect; // Objeto de névoa
    public Image dayIcon;
    public Sprite moonIcon;
    public Sprite sunIcon;
    public TextMeshProUGUI dayCounterText; // Contador de dias na tela

    private bool isNight;
    private bool isRaining;
    private float nextRainTime;
    private int dayCount = 0;
    private float lastSunAngle;
    void Awake()
    {
      Instance = this;
    }
    void Start()
    {
        isNight = false;
        isRaining = false;
        lightningObject.SetActive(false); // Deixa o raio desativado no início
        fogEffect.SetActive(false); // Deixa a névoa desativada no início
        nextRainTime = Time.time + Random.Range(0, rainInterval); // Define o tempo inicial para a primeira chuva
        lastSunAngle = sun.transform.eulerAngles.x;
        UpdateDayCounter();
    }

    void Update()
    {
        RotationSun();
        CheckWeather();
        HandleRain();
        CheckDayCycle();
    }

    void CheckWeather()
    {
        float sunAngle = sun.transform.eulerAngles.x;
        musicDay.SetActive(!isNight);
        musicNight.SetActive(!musicDay.activeSelf);
        
        if (sunAngle >= 180f && sunAngle <= 360f)
        {
            isNight = true;
            dayIcon.sprite = moonIcon;

            // Ajuste de cor e intensidade para a noite
            directionalLight.color = Color.blue;
            directionalLight.intensity = 0.2f;
        }
        else
        {
            isNight = false;
            dayIcon.sprite = sunIcon;

            // Ajuste de cor e intensidade para o dia
            directionalLight.color = Color.yellow;
            directionalLight.intensity = 1f;
        }
    }
   

    void RotationSun()
    {
        sun.transform.Rotate(Vector3.right * dayDuration * Time.deltaTime);
    }

    void HandleRain()
    {
        if (!isRaining && Time.time >= nextRainTime)
        {
            StartCoroutine(RainEvent());
            nextRainTime = Time.time + rainInterval + Random.Range(minRainDuration, maxRainDuration);
        }
    }

    IEnumerator RainEvent()
    {
        isRaining = true;
        rain.Play();
        rainEffect.Play(); // Ativa a chuva

        // Define aleatoriamente se haverá névoa e raios neste evento de chuva
        fogEffect.SetActive(Random.value <= fogChance);
        lightningObject.SetActive(Random.value <= lightningChance);

        // Define a duração da chuva aleatoriamente entre os valores mínimos e máximos
        float rainDuration = Random.Range(minRainDuration, maxRainDuration);
        yield return new WaitForSeconds(rainDuration);

        // Para a chuva e desativa os efeitos adicionais
        rainEffect.Stop();
        rain.Stop();
        fogEffect.SetActive(false); // Desativa a névoa
        lightningObject.SetActive(false); // Desativa o raio
        isRaining = false;
    }

    void CheckDayCycle()
    {
        float sunAngle = sun.transform.eulerAngles.x;

        // Verifica se o sol completou uma volta de 360 graus
        if (lastSunAngle > 350f && sunAngle < 10f)
        {
            dayCount++; // Incrementa o contador de dias
            UpdateDayCounter(); // Atualiza o contador na tela
        }
        lastSunAngle = sunAngle;
    }

    void UpdateDayCounter()
    {
        dayCounterText.text = "Day : " + dayCount;
    }
}