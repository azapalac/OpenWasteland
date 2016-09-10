using UnityEngine;
using System.Collections;

public class ImgPrefab : MonoBehaviour {
    float t;
	// Use this for initialization
	void Start () {
        t = 0;
	}
	
	// Update is called once per frame
	void Update () {
      
            Destroy(this.gameObject);
        
    }
}
