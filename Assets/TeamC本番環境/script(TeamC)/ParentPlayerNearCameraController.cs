using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TeamC;
using TMPro;

namespace TeamC
{

    public class ParentPlayerNearCameraController : MonoBehaviour
    {
        [SerializeField] private GameObject player;     // プレイヤー情報格納用
        [SerializeField] private SugorokuGameDirector sugorokuGameDirector;
        private Vector3 playerPos;                      // プレイヤーの位置
        
       
        void Start()
        {
            //offset = transform.position - player.transform.position;

        }

        // Update is called once per frame
        void Update()
        {
            if (sugorokuGameDirector.currentPlayer != null)
            {
                player = sugorokuGameDirector.currentPlayer;
            }
            
            playerPos = player.transform.position;

            // プレイヤの向きを取って、背後にカメラのポジションを取る
            var playerForward = player.transform.forward;
            this.transform.forward = playerForward;
            this.transform.position = player.transform.position + (-player.transform.forward * 4);
            this.transform.position = new Vector3(transform.position.x, 3.1f, transform.position.z);
            
        }
    }
}