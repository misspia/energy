using UnityEngine;

public class Node : MonoBehaviour
{
    private Renderer rend;
    private GameObject nodePrefab;
    private Material materialRef;
    public Color minColor;
    public Color maxColor;

    public int id = 0;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        if(rend)
        {
            rend.material = materialRef;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float spectrumNode = AudioPeer.spectrum[id];
        Color rgb = Color.Lerp(minColor, maxColor, spectrumNode * 1000);
        rend.material.color = rgb;
    }
    public void setId(int newId)
    {
        id = newId;
    }
}
