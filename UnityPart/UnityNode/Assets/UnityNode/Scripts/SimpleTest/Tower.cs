using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower 
{
	public string PlayerKey { get; set; }
    public string TowerKey { get; set; }
    public GameObject Master { get; set; }


    public string stateOf { get; set; }

    public Tower(string _playerKey, string _stateOf)
    {
        PlayerKey = _playerKey;
        stateOf = _stateOf;
    }
    public Tower()
    {
    }
}


