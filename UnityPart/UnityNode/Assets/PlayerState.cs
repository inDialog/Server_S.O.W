using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public string playerState = "Neutral";
    public string _playerKey;
    public bool active;
  
    public Transform respawnPos;
    public GameObject newTower;
   public  List<GameObject> TowersFowlowing = new List<GameObject>();

    private void Awake()
    {
        CreateColor();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            playerState = "Fallow";
            TowerState towerState = newTower.GetComponent<TowerState>();
            if (towerState.thisTower.TowerState != "Locked")
            {
                towerState.thisTower.Master = this.transform.gameObject;
                if(!TowersFowlowing.Contains(newTower))
                TowersFowlowing.Add(newTower);
            }
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            foreach (var towers in TowersFowlowing)
            {
                towers.GetComponent<TowerState>().thisTower.Master = null;
                towers.GetComponent<TowerState>().thisTower.TowerState = "Neutral";

            }
            TowersFowlowing.Clear();
            playerState = "Neutral";


        }
        if (Input.GetKey(KeyCode.K))
        {
            playerState = "Locked";
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            playerState = "Neutral";
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            RestartKey();
        }
        RespawnPosition();
    }
  
    void CreateColor()
    {
        if (PlayerPrefs.GetFloat("R") == 0)
        {
            int r = UnityEngine.Random.Range(10, 220);
            int b = UnityEngine.Random.Range(10, 220);
            int g = UnityEngine.Random.Range(10, 220);
            string _myColor = r.ToString() + g.ToString() + b.ToString();
            PlayerPrefs.SetFloat("R", r);
            PlayerPrefs.SetFloat("G", g);
            PlayerPrefs.SetFloat("B", b);
            print(r);
        }
    }
    void RestartKey()
    {
        PlayerPrefs.SetString("key", "");
        PlayerPrefs.SetInt("NrOfConnections", 0);
        PlayerPrefs.SetFloat("R", 0);
        PlayerPrefs.SetFloat("G", 0);
        PlayerPrefs.SetFloat("B", 0);
    }
    void RespawnPosition()
    {
        if (respawnPos != null)
        {
            float step = 5 * Time.deltaTime; // calculate distance to move
            Vector3 targget = respawnPos.position - respawnPos.up * -1;
            transform.position = Vector3.MoveTowards(transform.position, targget, step);
            GhostMode(true);
            if (transform.position == targget)
            {
                GhostMode(false);
                respawnPos = null;
            }
            else if(Input.anyKey)
            {
                GhostMode(false);
                respawnPos = null;
            }
        }
    }
    void GhostMode(bool state)
    {
        GetComponent<Rigidbody>().isKinematic = state;
        GetComponent<Collider>().isTrigger = state;
    }


}
