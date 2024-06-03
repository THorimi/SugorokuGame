using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TeamC;
using TMPro;

namespace TeamC
{

    public class ParentPlayerNearCameraController : MonoBehaviour
    {
        [SerializeField] private GameObject player;     // �v���C���[���i�[�p
        [SerializeField] private SugorokuGameDirector sugorokuGameDirector;
        private Vector3 playerPos;                      // �v���C���[�̈ʒu
        
       
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

            // �v���C���̌���������āA�w��ɃJ�����̃|�W�V���������
            var playerForward = player.transform.forward;
            this.transform.forward = playerForward;
            this.transform.position = player.transform.position + (-player.transform.forward * 4);
            this.transform.position = new Vector3(transform.position.x, 3.1f, transform.position.z);
            
        }
    }
}