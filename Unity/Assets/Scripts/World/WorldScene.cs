using UnityEngine;
using System.Collections;

public class WorldScene : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		if (!Main.SubsystemInitialized) Main.InitializeSubSystem();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
