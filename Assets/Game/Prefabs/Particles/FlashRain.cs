using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashRain : MonoBehaviour
{
public Light myLight;  // Sua luz direcional

    private float lastTime = 0;
    public AudioSource thunder;
    public float minTime = 5.0f;  // Tempo mínimo entre os raios
    public float maxTime = 11.0f; // Tempo máximo entre os raios
    public Color lightningColor = new Color(0.8f, 0.9f, 1.0f);  // Tom azul claro, típico de raios

    public int minFlashCount = 1;  // Número mínimo de flashes por sequência
    public int maxFlashCount = 5;  // Número máximo de flashes por sequência
    public float flashDuration = 0.1f;  // Duração de cada flash
    public float minFlashDelay = 0.1f;  // Atraso mínimo entre flashes em uma sequência
    public float maxFlashDelay = 0.3f;  // Atraso máximo entre flashes em uma sequência
    public float minIntensity = 0.3f;  // Intensidade mínima do flash
    public float maxIntensity = 0.6f;  // Intensidade máxima do flash

    private bool isFlashing = false;

    void Start()
    {
        // Ajusta a cor da luz para se parecer com um raio
        myLight.color = lightningColor;
    }

    void Update()
    {
        if (!isFlashing && (Time.time - lastTime) > Random.Range(minTime, maxTime))
        {
            StartCoroutine(StormLightning());
        }
    }

    IEnumerator StormLightning()
    {
        isFlashing = true;

        // Número de flashes aleatório para esta sequência
        int flashCount = Random.Range(minFlashCount, maxFlashCount);

        for (int i = 0; i < flashCount; i++)
        {
            // Intensidade do flash aleatória
            float flashIntensity = Random.Range(minIntensity, maxIntensity);

            // Define a intensidade da luz
            myLight.intensity = flashIntensity;
            myLight.enabled = true;

            // Duração do flash
            yield return new WaitForSeconds(flashDuration);

            // Desativa a luz para o intervalo entre flashes
            myLight.enabled = false;

            // Atraso aleatório entre flashes dentro da sequência
            yield return new WaitForSeconds(Random.Range(minFlashDelay, maxFlashDelay));
        }

        // Define um novo tempo aleatório para o próximo evento de raios
        lastTime = Time.time;

        isFlashing = false;
    }

}
