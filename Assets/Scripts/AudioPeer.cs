using UnityEngine;

[RequireComponent (typeof (AudioSource))]

public class AudioPeer: MonoBehaviour
{
    AudioSource audioSource;

    public const int SAMPLE_SIZE = 512;
    private float refValue = 0.1f;
    public float rmsValue; // sound pitch - RMS 
    public float dbValue; // sound pitch - dB

    public float[] nodesDev = new float[SAMPLE_SIZE];
    public static float[] nodes = new float[SAMPLE_SIZE];
    public static float[] samples;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource> ();
        audioSource.volume = 0.3f;
        audioSource.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        AnalyzeAudio();
    }
    private void AnalyzeAudio()
    {
       // GetOutputData();
        audioSource.GetSpectrumData(nodesDev, 0, FFTWindow.Blackman);
        audioSource.GetSpectrumData(nodes, 0, FFTWindow.Blackman);

    }
    private void GetOutputData()
    {
        audioSource.GetOutputData(samples, 0);
        float sum = 0;
        for(var i = 0; i < SAMPLE_SIZE; i ++)
        {
            sum += samples[i] * samples[i];
        }
        rmsValue = Mathf.Sqrt(sum / SAMPLE_SIZE);
        dbValue = Mathf.Min(20 * Mathf.Log10(rmsValue / refValue), -160);
    }
}

//https://answers.unity.com/questions/157940/getoutputdata-and-getspectrumdata-they-represent-t.html
