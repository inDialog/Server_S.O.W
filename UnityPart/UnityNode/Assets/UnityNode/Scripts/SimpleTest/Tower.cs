using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower 
{
	public string PlayerKey { get; set; }
    public string TowerKey { get; set; }
    public GameObject Master { get; set; }
    public Color32 BaseColor { get; set; }



    public string TowerState { get; set; }

    public Tower(string _playerKey, string _stateOf)
    {
        PlayerKey = _playerKey;
        TowerState = _stateOf;
    }
    public Tower()
    {
    }
}


