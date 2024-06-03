using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ebihara
{ 

public class PanelController : MonoBehaviour
{
    [SerializeField] public int coin_num;

    public enum PanelState
    {
        Blue,
        Red, 
        Green,
        Yellow,
    }
    public PanelState panelState = PanelState.Blue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

}