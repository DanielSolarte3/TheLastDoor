using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManagerScript : MonoBehaviour
{
    public AudioSource susurro;
    public AudioSource rugido;
    public AudioMixerGroup audioMixerGroup;

    private float susurroOriginalVolume;

    private void Start()
    {
        if (susurro != null)
            susurroOriginalVolume = susurro.volume;

        StartCoroutine(SusurroLoop());
    }

    private IEnumerator SusurroLoop()
    {
        while (true)
        {
            float randomDelay = Random.Range(5f, 15f);
            yield return new WaitForSeconds(randomDelay);
            StartCoroutine(PlaySusurroWithVolumeBoost());
        }
    }

    private IEnumerator PlaySusurroWithVolumeBoost()
    {
        float randomTime = Random.Range(0f, 2f);
        yield return new WaitForSeconds(randomTime);

        if (susurro != null)
        {
            susurroOriginalVolume = susurro.volume;
            susurro.volume = 1.0f; // Sube el volumen al máximo (ajusta si lo deseas)
            Debug.Log("Susurro played");
            susurro.Play();

            // Espera a que termine el clip
            yield return new WaitForSeconds(susurro.clip.length);

            susurro.volume = susurroOriginalVolume; // Restaura el volumen original
        }
    }
}
