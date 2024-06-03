using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//4/30 ����쐬
namespace ebihara { 

//�^�[���i�s�𐧌䂷��X�N���v�g
public class SugorokuGameDirector : MonoBehaviour
{
    //�e��UI�Ɛڑ����邽�߂̕ϐ��錾
    public GameObject TurnCountText;  //���̃^�[���J�E���gUI    
    public GameObject MenuPanel; //�R�}���h�I���̃p�l��
    public GameObject SelectPanel; //�͂��A�������̑I���p�l��
    public GameObject RollDiceButton; //�_�C�X���[���̎��s�{�^��
    public GameObject GuidePanel; //�_�C�X���[������K�C�h��UI
    public GameObject MessagePanel; //���b�Z�[�W�p�l����UI

    public GameObject moveCountText; //�ړ����́A�c��ړ����\��UI
                                  
    //�L�����I�u�W�F�N�g�ϐ�
    public GameObject player01;
    Vector3 player01Pos;

    //�_�C�X�I�u�W�F�N�g�̃v���t�@�u(type mismatch�̖��̂��߁A��U�ۗ�)
    public GameObject dicePrefab;

    //�_�C�X�I�u�W�F�N�g�ϐ�
    GameObject dice01;

    //�_�C�X�I�u�W�F�N�g�̍��W
    Vector3 dice01Pos;

    //���u���^�[����:5
    private int testTurnCount = 5;

    //�_�C�X�̏o�ڂ�ۑ�����ϐ�
    private int numOfDice01;


    void Start()
    {
    
     //UI�̏���
    TurnCountText.GetComponent<TextMeshProUGUI>().text = "testTurnCount:" + testTurnCount.ToString();

    //UI���\���ɂ���
    SelectPanel.SetActive(false);
    GuidePanel.SetActive(false);
    }

    //�T�C�R�����񂷃{�^�����N���b�N���ꂽ����s
    public void RollDiceButtonClicked()
    {
        //�_�C�X�̃K�C�h��\��
        GuidePanel.SetActive(true);

        //�v���C���[�̍��W���擾
        float x = player01.transform.position.x;
        float y = player01.transform.position.y;
        float z = player01.transform.position.z;
        player01Pos = new Vector3(x, y, z);

        //��������\��̃_�C�X�̍��W��ݒ肵�Ă���(�v���C���[�̓���)
        dice01Pos = new Vector3(x, y + 3, z);

        //�v���t�@�u����_�C�X�I�u�W�F�N�g�𐶐�
        dice01 = Instantiate(dicePrefab);

        if (dice01 != null)
        {
            //�_�C�X�I�u�W�F�N�g����o�ڂ̕ϐ����󂯎�郁�\�b�h���Ăяo��
            //�o�ڂ����������start���\�b�h���l�����āA���ԍ��Ŏ擾
            Invoke("getNumOfDice01", 0.1f);
        }

        //��قǐݒ肵�����W�փT�C�R�����ړ�
        dice01.transform.position = dice01Pos;

        //�e��UI���\���ɂ���
        MenuPanel.SetActive(false);
        MessagePanel.SetActive(false);

    }
        //----------------------------------------------------------------------------------------
        void Update()
        {
 
            //�T�C�R����U���ďo�ڂ��o������̏���
            if (numOfDice01 != 0 && dice01 == null)
            {
                //�ړ����W�b�N�֏o�ڂ�n��
                player01.GetComponent<TeamC.PlayerController>().PlayerMove(numOfDice01);
                Debug.Log("�ړ����W�b�N�֓n�����o��(numOfDice01):" + numOfDice01);

                //�����̏o�ڕϐ���������
                numOfDice01 = 0;

                //�_�C�X�̃K�C�h���\��
                GuidePanel.SetActive(false);
            }


            //�v���C���[�̍s�����I������ꍇ�̏���(���݁A�v���C���[1�l�̏ꍇ�ɂ����Ή����Ă��܂���)
            if (player01.GetComponent<TeamC.PlayerController>().turnEndFlag)
            {

                //�s���I���t���O��false�ɂ��A���̍s���I���ɔ�����
                player01.GetComponent<TeamC.PlayerController>().turnEndFlag = false;

                //�^�[���J�E���g������炷
                testTurnCount -= 1;

                //��L���^�[���\���ɔ��f
                TurnCountText.GetComponent<TextMeshProUGUI>().text = "testTurnCount:" + testTurnCount.ToString();

                //�e��UI���ĕ\������
                MenuPanel.SetActive(true);
                MessagePanel.SetActive(true);
            }

            //�c��^�[����0�ɂȂ�����A���̃��U���g��ʂ֑J��
            if (testTurnCount == 0)
            {
                SceneManager.LoadScene("TestResultScene");
            }

            //�c�ړ���UI�̍X�V
            moveCountText.GetComponent<TextMeshProUGUI>().text =
                player01.GetComponent<TeamC.PlayerController>().moveCount.ToString();

            //�v���C���[�̎c�ړ�����0���傫����΁AUI��\��
            if (player01.GetComponent<TeamC.PlayerController>().moveCount > 0)
            {
                //�\�����A�N�e�B�u�ɂ���
                moveCountText.SetActive(true);
            }
            else
            {
                //�\�����A�N�e�B�u�ɂ���
                moveCountText.SetActive(false);
            }

        }
//----------------------------------------------------------------------------------------
    //DiceRollingLogic����o�ڂ��󂯎�郁�\�b�h
    void getNumOfDice01()
    {
        //�o�ڂ̐����󂯎��
        numOfDice01 = dice01.GetComponent<TeamC.DiceRollingLogic>().numOfDice;
        Debug.Log("director���_�C�X����󂯎�����o��(numOfDice01):" + numOfDice01);
        Debug.Log("player01:" + player01);
    }
}
}