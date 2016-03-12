using UnityEngine;
using System.Collections;
using RVO;


public class RVO_LibraryTest : MonoBehaviour {

	Simulator _simulator = new Simulator();
	public GameObject[] agents;

	// Use this for initialization
	void Start () {	

		_simulator.setAgentDefaults(3.0f, 5, 1.0f, 1.0f, 1.0f, 2.0f, new RVO.Vector2(0.0f, 0.0f));
		_simulator.addAgent(new RVO.Vector2(0.0f, 0.0f));
		_simulator.addAgent (new RVO.Vector2 (1.0f, 0.0f));
		_simulator.agents_ [0].velocity_ = new RVO.Vector2 (1.0f, 0.0f);	
		_simulator.SetNumWorkers (2);
	}
	
	// Update is called once per frame
	void Update () {
		_simulator.SetNumWorkers (1);
		_simulator.doStep ();
		agents [0].transform.position = new Vector3 (_simulator.agents_ [0].position_.x(), 0.0f, _simulator.agents_ [0].position_.y());
		agents [1].transform.position = new Vector3 (_simulator.agents_ [1].position_.x(), 0.0f, _simulator.agents_ [1].position_.y());

	}
}
