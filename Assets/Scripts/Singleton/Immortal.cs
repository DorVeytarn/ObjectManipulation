using UnityEngine;

public class Immortal : MonoBehaviour 
{
    void Start () 
    {
        GameObject.DontDestroyOnLoad(gameObject);
	}
}
