using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//4/26 ����쐬
//�_�C�X�����A�j���[�V�����ƁA�o�ڂ𗐐���������X�N���v�g
//����ł̓_�C�X�ɃA�^�b�`���ē����d�l

namespace ebihara

{

    public class DiceRollingLogic : MonoBehaviour
    {

        //��]���x�Ɋ֌W����ϐ�
        private int rotateX;
        private int rotateY;
        private int rotateZ;

        //�T�C�R���̃X�e�[�^�X��\��������ϐ�
        private string rollingFlag = "Notstart";

        //�T�C�R���̏o��
        public int numOfDice;

        void Start()
        {
            //�T�C�R���̏o�ڂ�start���\�b�h�Ō��肵�Ă���
            numOfDice = Random.Range(1, 7);
        }

        // Update is called once per frame
        void Update()
        {
            //�T�C�R����U��I���O�ŁA���AEnter�L�[�������Ă���Ԃ����A�_�C�X�̃X�e�[�^�X��"Rolling"�ɂ���
            if ((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return))
                && (rollingFlag != "Stop"))
            {

                rollingFlag = "Rolling";
            }

            //Enter�L�[�������Ă��炸�A�_�C�X������Ă���ƁA�_�C�X�̃X�e�[�^�X��"Stop"�ɂ���
            else if (rollingFlag == "Rolling")
            {
                rollingFlag = "Stop";
            }

            //�t���O��"Rolling"�̊ԁA�T�C�R���̉�]���p��������
            if (rollingFlag == "Rolling")
            {
                //��]���x�͗��������EUpDate�ōX�V�����
                //�����Ǝ��R�ȃ��[���̎d���ɂȂ�ݒ肪����΁A�����\��
                rotateX = Random.Range(0, 360);
                rotateY = Random.Range(0, 360);
                rotateZ = Random.Range(0, 360);
                transform.Rotate(rotateX, rotateY, rotateZ);
            }

            //�_�C�X�̃X�e�[�^�X��"Stop"�Ȃ�A�܂�T�C�R����U��I���������
            if (rollingFlag == "Stop")
            {
                //�T�C�R������������
                Destroy(this.gameObject);
            }
        }
    }
}