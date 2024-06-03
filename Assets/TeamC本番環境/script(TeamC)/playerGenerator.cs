using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamC
{
    public class PlayerGenerator : MonoBehaviour
    {
        private int playerNum; //�v���C���[��
        public List<GameObject> playerList = new List<GameObject>(); //�v���C���[���i�[���郊�X�g

        //������������������ҏW������������
        //�v���t�@�u1�̂�ϐ��錾���Ă��܂������A��q�̕ϐ��錾�ɕύX���܂���
        //[SerializeField] public GameObject playerPrefab;

        // 5/9 ����ǋL�v���C���[�v���t�@�u���i�[���郊�X�g
        //playerGenerator�̃C���X�y�N�^�[�փv���t�@�u���A�E�g���b�g�ڑ����Ă�������
        [SerializeField] public List<GameObject> playerPrefabList;
        //������������������ҏW�����܂Ł�����������

        public List<GameObject> PlayerGenerate(int playerNum)
        {
            for (int i = 0; i < playerNum; i++)
            {
                //������������������ҏW 5/9������������
                //GameObject player = Instantiate(playerPrefab);
                GameObject player = Instantiate(playerPrefabList[i]);

                //������������������ҏW�����܂Ł�����������

                player.transform.position = new Vector3(1.85f - 1.4f * i, 1f, -8.4f);

                //������������������ҏW 5/9������������
                //�v���C���[������͂��Ă��炤�@�\�������ł���܂ŉ��u���̏���
                player.GetComponent<TeamC.PlayerController>().playerName = "player" + (i+1).ToString(); 
                //������������������ҏW�����܂Ł�����������

                player.GetComponent<TeamC.PlayerController>().coin = 100;
                playerList.Add(player);
            }
            return playerList;
        }

        void Update()
        {

        }
    }


}

