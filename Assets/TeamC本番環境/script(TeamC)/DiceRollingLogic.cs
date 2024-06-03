using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//4/26 ����쐬
//�_�C�X�����A�j���[�V�����ƁA�o�ڂ𗐐���������X�N���v�g
//����ł̓_�C�X�ɃA�^�b�`���ē����d�l

namespace TeamC
{
    public class DiceRollingLogic : MonoBehaviour
    {
        // �f�o�b�O�p
        private KeyCode[] _key = new KeyCode[]
        {
            KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2,
            KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
            KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8,
            KeyCode.Alpha9
        };

        //��]���x�Ɋ֌W����ϐ�
        private int rotateX;
        private int rotateY;
        private int rotateZ;

        //�T�C�R���̏o��(Director�X�N���v�g�ƘA�g������)
        public int numOfDice;

        //�T�C�R���̃X�e�[�^�X��\��������ϐ�
        public string rollingFlag = "ReadyForStart";

        //�㑱�̃R�[�h�ŃR���[�`�����g�p���邽�߂ɁAstart���\�b�h���R���[�`���ɑg�ݍ���
        private IEnumerator Start()
        {
            numOfDice = Random.Range(1, 7);


            yield return null; // Start�̎��s����������܂�1�t���[���҂��Č㑱�̏�����ҋ@
        }
        // �A�C�e���g�p�����܂߂��o�ڂ�Ԃ����\�b�h
        public int GetNumOfDice(string itemId)
        {
            numOfDice = Random.Range(1, 7);
            
            if (itemId == "dice_0") //�����_�C�X
            {
                numOfDice = Random.Range(1, 4) * 2;
                Debug.Log("�����_�C�X");
            }
            else if (itemId == "dice_1") //��_�C�X
            {
                numOfDice = Random.Range(1, 4) * 2 - 1;
                Debug.Log("��_�C�X");
            }
            else if (itemId == "dice_2") //*2���_�C�X
            {
                numOfDice = Random.Range(1, 7) + Random.Range(1, 7);
                Debug.Log("�~�Q�_�C�X");
            }

            return numOfDice;
        }

        private void Update()
        {
            //�T�C�R����U��O�̏�ԂŁA����Enter�L�[���������ꍇ�ɁA�_�C�X�̃X�e�[�^�X��"Rolling"�ɂ��A
            //1�b��ɁA�_�C�X���~�߂���悤�ɂ��鏈�������s
            if (rollingFlag == "ReadyForStart" && 
                (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)))
            {
                rollingFlag = "Rolling";

                //�R���[�`���J�n
                StartCoroutine(RollDice());
            }

            //1�b�o���Ă���΁AEnter�L�[�������ꂽ�ۂɁA�_�C�X�̃X�e�[�^�X��"Stop"�ɂ���(�܂�A�j���[�V�������~�߂�)
            if (rollingFlag == "ReadyForStop" && (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)))
            {
                rollingFlag = "Stop";
            }

            if (Input.anyKeyDown)
            {
                Debug.Log("keydown");
                // �f�o�b�O�p�@�e���L�[�ŏo�ڑ���
                for (int i = 0; i < _key.Length; i++)
                {
                    if (Input.GetKeyDown(_key[i]))
                    {
                        Debug.Log(_key[i]);
                        numOfDice = i;
                        rollingFlag = "Stop";
                    }
                }
            }

            //�_�C�X���񂷃A�j���[�V�����̏���
            if (rollingFlag == "Rolling" || rollingFlag == "ReadyForStop")
            {
                rotateX = Random.Range(0, 360);
                rotateY = Random.Range(0, 360);
                rotateZ = Random.Range(0, 360);
                transform.Rotate(rotateX, rotateY, rotateZ);
            }

            //�_�C�X�̃X�e�[�^�X��Stop�ɂȂ����ꍇ�A�T�C�R�����Ə������ƂŁA���̃X�N���v�g�̏������I��������
            // �x���ǋL�@Director���Ŗڂ��擾���Ă���T�C�R�����폜
            if (rollingFlag == "Destroy")
            {
                //�T�C�R���I�u�W�F�N�g�������邱�Ƃ�null�ɂȂ�ADirector���ŏo�ڂ̎󂯓n������������
                Destroy(this.gameObject);
            }

            //���ؗp�f�o�b�O���O
            //Debug.Log("rollingFlag:" + rollingFlag);
        }

        //0.5�b�ҋ@���Ă���rollingFlag��"ReadyForStop"�ɕύX����R���[�`�����\�b�h
        //�_�C�X�̃A�j���[�V�����J�n�ƒ�~�������ɓ����Ȃ��悤�ɂ��邽�߂̏���
        private IEnumerator RollDice()
        {
            yield return new WaitForSeconds(0.5f); // 0.5�b�ҋ@
            rollingFlag = "ReadyForStop";
        }
    }
}