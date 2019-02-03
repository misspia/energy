using UnityEngine;


public class Visualization : MonoBehaviour
{
    public GameObject audioNodePrefab;
    GameObject[] audioNodes = new GameObject[512];
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
        for(var i = 0; i < 512; i++)
        {
            GameObject node = Instantiate(
                audioNodePrefab,
                new Vector3(i, 0, 0),
                Quaternion.identity
            );
            node.transform.name = "node" + i;
            node.GetComponent<Renderer>().material.color = new Color(0.50f, 0.5f, 0.9f, 1.0f);
        }
    }
}
