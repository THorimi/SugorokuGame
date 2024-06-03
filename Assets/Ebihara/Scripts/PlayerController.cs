using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace ebihara

{
    public class PlayerController : MonoBehaviour
    {
        private int coin;
        CinemachineDollyCart movePath;
        public int moveCount;
        bool startFlag;
        bool eventFlag;
        [SerializeField] int testMoveCount;



        // ���������R�ǉ���������
        [SerializeField] private Animator animator;
        GameObject player;
        private Vector3 playerPos;
        // player�ƃJ������Transform
        [SerializeField] private Transform _self;   // player
        [SerializeField] private Transform _target; // �J����
        // ���������R�ǉ���������


        //������������������ǋL 4/30������������
        //Director�Ƀ^�[���I���������˗����邽�߂̕ϐ�
        public bool turnEndFlag = false;

        //������������������ǋL�����܂Ł�����������

        void Start()
        {
            movePath = this.GetComponent<CinemachineDollyCart>();
            startFlag = true;
            movePath.enabled = false;
            //PlayerMove(testMoveCount);

            // ���������R�ǉ���������
            // �����ʒu��ێ�
            playerPos = transform.position;
            // ���������R�ǉ���������
        }

        // Update is called once per frame
        void Update()
        {


            // ���������R�ǉ���������
            // ���݈ʒu�擾
            var position = transform.position;
            // ���ݑ��x�v�Z
            var velocity = (position - playerPos) / Time.deltaTime;
            // �O�t���[���ʒu���X�V
            playerPos = position;

            if (velocity.x == 0 && velocity.y == 0 && velocity.z == 0)
            {
                animator.SetBool("WalkingBool", false);
                _self.LookAt(_target);
            }
            else
            {
                animator.SetBool("WalkingBool", true);
            }
            // ���������R�ǉ���������
        }

        // Player�̃X�s�[�h�𑀍�
        // �T�C�R���̖ڂ������ɂ��ďo���ڂ̐��i��
        public void PlayerMove(int moveCount)
        {
            this.moveCount = moveCount;
            movePath.enabled = true;
            movePath.m_Speed = 2;

            eventFlag = false;

        }

        bool passFlag = false;
        // Panel�ɐڐG���邽�т�moveCount������������
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Panel")
            {
                Vector3 panelPos = other.transform.position;
                Vector3 playerPos = this.transform.position;
                GameObject panel = other.gameObject;
                if (!passFlag)
                {
                    passFlag = true;
                    if (this.moveCount > 0)
                    {
                        this.moveCount -= 1;
                        Debug.Log("�c��̈ړ���" + moveCount);
                    }
                    else
                    {

                    }
                }
                if (this.moveCount == 0)
                {

                    // �p�l���̒��S���W
                    Vector3 target = new Vector3(panelPos.x, playerPos.y, panelPos.z);
                    // �v���C���[�Ƃ̋����̌v�Z
                    float distanceOfPlayer = Vector3.Distance(playerPos, target);



                    //CinemachineDollyCart���~�A�蓮�ňړ������������i�߂Ă��疳����
                    movePath.m_Speed = 0;
                    if (startFlag)
                    {
                        startFlag = false;
                    }
                    else
                    {
                        movePath.m_Position += distanceOfPlayer;
                        Debug.Log("positionAdd");
                    }
                    movePath.enabled = false;
                }

            }
        }
        public void OnTriggerStay(Collider other)
        {

            if (other.gameObject.tag == "Panel")
            {
                Vector3 panelPos = other.transform.position;
                Vector3 playerPos = this.transform.position;
                GameObject panel = other.gameObject;

                if (this.moveCount == 0)
                {

                    // �p�l���̒��S���W�܂ňړ�
                    Vector3 target = new Vector3(panelPos.x, playerPos.y, panelPos.z);

                    float step = 2.0f * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(playerPos, target, step);


                    if (!eventFlag)
                    {
                        eventFlag = true;

                        //������������������ǋL 4/30������������
                        //�~�܂����}�X�ɉ����ăC�x���g
                        //Debug.Log(panel.GetComponent<TeamC.PanelController>().panelState + "�̃}�X�ɂƂ܂�܂���");

                        //�v���C���[�̍s�����I���������߁A�t���O��true�ɂ���
                        //����̉��C��ɉ����āA���L�R�[�h�̈ʒu��ύX���Ă�������
                        turnEndFlag = true;

                        //������������������ǋL�����܂Ł�����������
                    }
                }
            }
        }
        public void OnTriggerExit(Collider other)
        {
            passFlag = false;
        }
        public void TestBtnClicked()
        {
            PlayerMove(3);
        }
    }
}