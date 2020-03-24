using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower 
{
	public string playerKey { get; set; }
    public string towerKey { get; set; }

    public string stateOf { get; set; }

    public Tower(string _playerKey, string _stateOf)
    {
        playerKey = _playerKey;
        stateOf = _stateOf;
    }
    public Tower()
    {
    }
}


