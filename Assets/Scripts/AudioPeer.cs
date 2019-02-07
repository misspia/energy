using UnityEngine;

[RequireComponent (typeof (AudioSource))]

public class AudioPeer: MonoBehaviour
{
    AudioSource audioSource;

    public const int SAMPLE_SIZE = 1024;
    private float refValue = 0.01f;
    private float threshold = 0.02f;

    public float rmsValue; // sound pitch - RMS 
    public float dbValue; // sound pitch - dB
    public float pitchValue;

    public static float[] samples = new float[SAMPLE_SIZE];
    public static float frequency;
    public static float[] spectrum = new float[SAMPLE_SIZE];

    public const int NUM_FREQ_BANDS = 8;
    private static float[] frequencyBands = new float[NUM_FREQ_BANDS];
    private static float[] bandBuffer = new float[NUM_FREQ_BANDS];
    private float[] bufferDecrease = new float[NUM_FREQ_BANDS];
    private float[] frequencyBandHighest = new float[NUM_FREQ_BANDS];

    public static float[] audioBand = new float[NUM_FREQ_BANDS];
    public static float[] audioBandBuffer = new float[NUM_FREQ_BANDS];

    public static float averageBandBuffer;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 1.0f;
        audioSource.loop = true;

        frequency = AudioSettings.outputSampleRate;
    }

    // Update is called once per frame
    void Update()
    {
        AnalyzeAudio();
        ComputeFrequencyBands();
        BandBuffer();
        ComputeAudioBands();
        ComputeAverageBandBuffer();
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
        dbValue = Mathf.Max(-160, 20 * Mathf.Log10(rmsValue / refValue));

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
                average += spectrum[count] * (count + 1);
                count++;
            }
            average /= count;
            frequencyBands[i] = average * 10;
        }
    }
    private void BandBuffer()
    {
        for(int g = 0; g < NUM_FREQ_BANDS; g++)
        {
            if(frequencyBands[g] > bandBuffer[g])
            {
                bandBuffer[g] = frequencyBands[g];
                bufferDecrease[g] = 0.005f;
            }
            if (frequencyBands[g] < bandBuffer[g])
            {
                bandBuffer[g] -= bufferDecrease[g];
                bufferDecrease[g] *= 1.2f;
            }

        }
    }
    private void ComputeAudioBands()
    {
        for(int i = 0; i < NUM_FREQ_BANDS; i++)
        {
            if(frequencyBands[i] > frequencyBandHighest[i])
            {
                frequencyBandHighest[i] = frequencyBands[i]; 
            }
            audioBand[i] = frequencyBands[i] / frequencyBandHighest[i];
            audioBandBuffer[i] = bandBuffer[i] / frequencyBandHighest[i];
        }
    }
    private void ComputeAverageBandBuffer()
    {
        float sum = 0;
        for(int i = 0; i < audioBandBuffer.Length; i++)
        {
            sum += audioBand[i];
        }
        averageBandBuffer = sum / audioBandBuffer.Length;
    }
}

//https://answers.unity.com/questions/157940/getoutputdata-and-getspectrumdata-they-represent-t.html
// https://youtu.be/EAjNZJ8G0Hs?t=124