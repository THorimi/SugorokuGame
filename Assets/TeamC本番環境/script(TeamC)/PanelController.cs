using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�x������쐬�A5/2����ҏW
namespace TeamC { 

    public class PanelController : MonoBehaviour
    {
        //������������������ǋL 5/14������������
        //�A�C�e���F�R�A�̌��Ɋւ���ϐ�
        [SerializeField] public int core_num;
        //������������������ǋL 5/14������������

        [SerializeField] public int coin_num;
        [SerializeField] public bool isJunction = false;
        GameObject PosListObj;
        public List<GameObject> PosList = new List<GameObject>();
        // ���l�̃v���C���[���؍݂��Ă��邩
        public int stayCount;
        // ���݂̃p�l����PathPosition�i����PathA�̂Ƃ����ƂŐݒ肵���ꍇ�̂݁j
        public float pathPos;


        public enum PanelState
        {
            Blue, 
            Red, 
            Green,
            Yellow,
            // �A�C�e���Q�b�g�C�x���g
            Yellow_2,
            //������������������ǋL 5/21�������灡����������
            Orange,
            Purple,
            White,
            Black,
            Gray
        }
            //������������������ǋL 5/21�����܂Ł�����������

        //�F���i�[���邽�߂̃}�e���A�����X�g
        //PanelPrefab�̃C���X�y�N�^�[���ŁA�g�������F�}�e���A�����A�^�b�`���Ă�������
        [SerializeField] public List<Material> panelColorList;

        //�C���X�y�N�^�[�̃v���_�E���őI�������J���[�ƕR�Â��邽�߂̕ϐ��錾
        public PanelState panelState;

        void Start()
        {
            // �x���ǋL ==============================
            stayCount = 0;

            PosListObj = this.gameObject.transform.Find("PosList").gameObject;
            Transform PosListTransform = PosListObj.transform;
            foreach (Transform pos in PosListTransform)
            {
                PosList.Add(pos.gameObject);
            }
            // �x�������܂� ==========================

            //������������������ҏW 5/21������������
            //�}�X�̌����ڂ̐F��ύX����
            panelColorSet();
            //������������������ҏW 5/21������������

            //�e�}�X�̃C���X�y�N�^�[�őI�������F�ɉ����āA�����𕪊򂳂���
            switch (panelState)
            {
                //�}�X��Blue�Ȃ珊���R�C����������
                case PanelState.Blue:

                    //���u���ݒ�F�R�C���֘A�̕ϐ���+5����
                    this.coin_num = 5;
                    break;

                //�}�X��Red�Ȃ珊���R�C��������
                case PanelState.Red:

                    //���u���ݒ�F�R�C���֘A�̕ϐ���(-5)����
                    this.coin_num = -5;
                    break;

                //�}�X��Green�Ȃ�A�R�A���R�C���Ŕ������ǂ�����I��
                case PanelState.Green:

                    //���u���ݒ�F�R�C���֘A�̕ϐ���(-50)����
                    this.coin_num = -50;

                    //���u���ݒ�F�R�A�֘A�̕ϐ���1����
                    this.core_num = 1;

                    break;

                //������������������ǋL 5/21������������
                //�}�X��Yellow�Ȃ�
                case PanelState.Yellow:

                    //����Ŋl���ł��閇���̐ݒ�
                    //PlayerController����A�����ݒ�����炵�Ă���
                    this.coin_num = 60;
                    break;

                case PanelState.Yellow_2:

                    break;

                //�u���b�N�}�X�̐ݒ�
                case PanelState.Black:

                    //��������V�K�C�x���g�ɉ����Ēǉ��\��
                    break;

                //�I�����W�}�X�̐ݒ�
                case PanelState.Orange:

                    //��������V�K�C�x���g�ɉ����Ēǉ��\��
                    break;

                //�p�[�v���}�X�̐ݒ�
                case PanelState.Purple:

                    //��������V�K�C�x���g�ɉ����Ēǉ��\��
                    break;

                //�z���C�g�}�X�̐ݒ�
                case PanelState.White:

                    //��������V�K�C�x���g�ɉ����Ēǉ��\��
                    break;

                //�O���[�}�X�̐ݒ�
                case PanelState.Gray:

                    //�����N���Ȃ��}�X�Ƃ��Ďg�p��
                    break;
            }
        }

        void Update()
        {
            //�p�l���X�e�[�g�����F�ŁA���l���\�ȃR�C�������ݒ肪0�ȉ��ɂȂ����ꍇ
            if(panelState == PanelState.Yellow && this.coin_num <= 0)
            {
                //�p�l���X�e�[�g���u���b�N�ɕύX
                this.panelState = PanelState.Gray;

                //�}�X�̌����ڂ̐F��ύX����
                panelColorSet();
            }
        }

        //�}�X�̌����ڂ̐F��ύX���郁�\�b�h
        public void panelColorSet()
        {
            //�}�X�ɁA�ݒ肳��Ă���F(panelState)�ƑΉ�����}�e���A�����A�^�b�`����
            this.GetComponent<MeshRenderer>().material =
                panelColorList.Find(n => n.name.Contains(panelState.ToString()));
        }
        //������������������ǋL�����܂� 5/21������������
    }

}