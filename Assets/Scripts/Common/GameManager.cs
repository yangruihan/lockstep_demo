using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

	#region Singleton
	private static GameManager _instance;

	public static GameManager Instance
	{
		get
		{
			return _instance;
		}
	}
	#endregion

	private void Awake()
	{
		_instance = this;
	}
}
