using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TeamC;
using System.IO;

namespace TeamC
{


    public class ParentPlayerFarCameraController : MonoBehaviour
    {
        
        public GameObject player;
        private Vector3 offset = new Vector3(0, 10, 11.5f);  // Far�J�����̃v���C���[�Ƃ̈ʒu����
        [SerializeField] private SugorokuGameDirector sugorokuGameDirector;

        // Start is called before the first frame update
        void Start()
        {

            // �Q�[���J�n���_�̃J�����ƃ^�[�Q�b�g�̋����i�I�t�Z�b�g�j���擾

        }

        void Update()
        {
            if (sugorokuGameDirector.currentPlayer != null)
            {
                player = sugorokuGameDirector.currentPlayer;
            }
            else if (sugorokuGameDirector.currentPlayer == null)
            {

            }
        }


        /// <summary>
        /// �v���C���[���ړ�������ɃJ�������ړ�����悤�ɂ��邽�߂�LateUpdate�ɂ���B
        /// </summary>
        void LateUpdate()
        {
            // �J�����̈ʒu���v���C���[�̈ʒu�ɃI�t�Z�b�g�𑫂����ꏊ�ɂ���B
            // ���Ԋu��ۂ��v���C���̉�]�ɍ��E����Ȃ�
            this.transform.position = player.transform.position + offset;
            this.transform.rotation = Quaternion.Euler(45f, 180f, 0f);

        }
    }
}
