using UnityEngine;

[RequireComponent (typeof (AudioSource))]

public class AudioPeer: MonoBehaviour
{
    AudioSource audioSource;

    public const int SAMPLE_SIZE = 1024;
    private float refValue = 0.1f;
    private float threshold = 0.02f;

    public float rmsValue; // sound pitch - RMS 
    public float dbValue; // sound pitch - dB
    public float pitchValue;

    public static float[] samples = new float[SAMPLE_SIZE];
    public static float frequency;
    public static float[] spectrum = new float[SAMPLE_SIZE];

    public const int NUM_FREQ_BANDS = 8;
    public static float[] frequencyBands = new float[NUM_FREQ_BANDS];


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.3f;
        audioSource.loop = true;

        frequency = AudioSettings.outputSampleRate;
    }

    // Update is called once per frame
    void Update()
    {
        AnalyzeAudio();
        ComputeFrequencyBands();
    }
    private void AnalyzeAudio()
    {
        audioSource.GetOutputData(samples, 0);
        float sum = 0;
        for(int i = 0; i < SAMPLE_SIZE; i ++)
        {
            sum += samples[i] * samples[i];
        }
        rmsValue = Mathf.Sqrt(sum / SAMPLE_SIZE); // rms = sum of squared samples
        dbValue = 20 * Mathf.Log10(rmsValue / refValue);
        if(dbValue < -160)
        {
            dbValue = -160; // clamp to -160dB min ... Math.min?
        }

        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0; // max spectrum value
        int maxN = 0; // index of max spectrum value

        for(int i = 0; i < SAMPLE_SIZE; i ++)
        {
            // find max
            if(!(spectrum[i] > maxV) || !(spectrum[i] > threshold))
            {
                continue;
            }
            maxV = spectrum[i];
            maxN = i;
        }
        float freqN = maxN;
        if(maxN > 0 && maxN < SAMPLE_SIZE - 1)
        {
            // interpolate index using neighbours
            float dL = spectrum[maxN - 1] / spectrum[maxN];
            float dR = spectrum[maxN + 1] / spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        pitchValue = freqN * ( frequency / 2) / SAMPLE_SIZE; // convert index to frequency

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
    private void ComputeBeatValue()
    {
       // https://answers.unity.com/questions/733587/beat-detection-algorithm.html
       // https://github.com/allanpichardo/Unity-Beat-Detection
    }
    private void ComputeFrequencyBands()
    {
        int count = 0;
        for(int i = 0; i < NUM_FREQ_BANDS; i ++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            if(i == 7)
            {
                sampleCount += 2;
            }
            for(int j = 0; j < sampleCount; j ++)
            {
                average += samples[count] * (count + 1);
                count++;
            }
            average /= count;
            frequencyBands[i] = average * 10;
        }
    }

}

//https://answers.unity.com/questions/157940/getoutputdata-and-getspectrumdata-they-represent-t.html
