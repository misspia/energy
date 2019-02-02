using UnityEngine;

[RequireComponent (typeof (AudioSource))]

public class AudioPeer: MonoBehaviour
{
    AudioSource audioSource;

    public float[] nodes = new float[512];

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource> ();    
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
    }
    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(nodes, 0, FFTWindow.Blackman);
    }
}
