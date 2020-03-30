using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerActions : MonoBehaviour
{
    TowerState towerState;
    
    // Start is called before the first frame update
    void Start()
    {
        towerState =GetComponent<TowerState>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (towerState.thisTower.TowerState)
        {
            case "Locked":
                this.transform.localScale = new Vector3(1,5,1);
                break;
            case "Fallow":
                this.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                break;
            case "Neutral":
                this.transform.localScale = new Vector3(2, 2, 2);

                break;
            default:
                break;

        }
    }


}
