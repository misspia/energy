using UnityEngine;


public class Visualization : MonoBehaviour
{
    public GameObject audioNodePrefab;
    private const int NUM_NODES = AudioPeer.SAMPLE_SIZE;
    GameObject[] audioNodes = new GameObject[NUM_NODES];

    // Start is called before the first frame update
    void Start()
    {
        InstantiateAudioNodes();
    }

    // Update is called once per frame
    void Update()
    {   
        
    }
    void InstantiateAudioNodes()
    {
        for (var i = 0; i < NUM_NODES; i++)
        {
            GameObject node = Instantiate(
                audioNodePrefab,
                new Vector3(-250 + i, 0, 0),
                Quaternion.identity,
                this.transform
            );

            node.GetComponent<Node>().setId(i);
            node.transform.name = "node" + i;
            audioNodes[i] = node;
        }
    }
}
