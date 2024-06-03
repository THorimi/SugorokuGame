using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamC
{



    public class CameraContoroller : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Camera playerFarCamera;
        [SerializeField] private Camera playerNearCamera;
        [SerializeField] private Camera overViewCamera;
        [SerializeField] private GameObject sugorokuGameDirector;
        [SerializeField] private GameObject coinUpDownText;
        private Camera selectCamera;
        Transform selectCameraTransform;
        PlayerController cc;
        private Vector3 mainCameraStartPos;     // ���C���J�����̏����ʒu
        private Quaternion mainCameraStartRota; // ���C���J�����̏�����]
        private Vector3 overViewCameraStartPos; // overView�J�����̏����ʒu
        Transform coinUpDownTextPos;             // �R�C�������e�L�X�g�̏������

        void Start()
        {
            // ���C���J�����̏����ʒu�E������]�̎擾
            mainCameraStartPos = mainCamera.transform.position;
            mainCameraStartRota = mainCamera.transform.rotation;
            overViewCameraStartPos = new Vector3(-40, 30, -22);

            // ���C���J������Near�J������overView�J�����͏�����Ԃł͖�����
            mainCamera.enabled = false;
            playerNearCamera.enabled = false;
            overViewCamera.enabled = false;


            selectCamera = playerFarCamera;
            coinUpDownTextPos = coinUpDownText.transform;
        }

        void LotateUpdate()
        {
            // �}�E�X�̍��{�^���������ꂽ��
            //if (Input.GetKeyDown(KeyCode.Space))

            Debug.Log("moveCount�� " + cc.moveCount);

        }


        public void WorldMapView()
        {
            mainCamera.enabled = true;
            playerFarCamera.enabled = false;
            playerNearCamera.enabled = false;
            overViewCamera.enabled = false;

            // WorldMap�{�^�����������тɍŏ��̈ʒu�ɖ߂�
            mainCamera.transform.position = mainCameraStartPos;
            mainCamera.transform.rotation = mainCameraStartRota;

            selectCamera = mainCamera;
            selectCameraTransform = selectCamera.transform;
            coinUpDownText.transform.LookAt(selectCameraTransform);
        }

        public void ViewChange()
        {
            mainCamera.enabled = false;
            overViewCamera.enabled = false;

            if (playerFarCamera.enabled == false)
            {
                playerFarCamera.enabled = true;
                playerNearCamera.enabled = false;
                selectCamera = playerFarCamera;
                selectCameraTransform = selectCamera.transform;
                coinUpDownText.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                Debug.Log("selectCamera" + selectCamera);
            }
            else if (playerFarCamera.enabled == true)
            {
                playerFarCamera.enabled = false;
                playerNearCamera.enabled = true;
                selectCamera = playerNearCamera;
                selectCameraTransform = selectCamera.transform;
                coinUpDownText.transform.LookAt(selectCameraTransform);
                coinUpDownText.transform.Rotate(0.0f, -180.0f, 0.0f);
                Debug.Log("selectCamera" + selectCamera);
            }
        }

        public void OverViewMap()
        {
            overViewCamera.enabled = true;
            mainCamera.enabled = false;
            playerFarCamera.enabled = false;
            playerNearCamera.enabled = false;

            // WorldMap�{�^�����������тɍŏ��̈ʒu�ɖ߂�
            //overViewCamera.transform.position = overViewCameraStartPos;
            
        }
    }
}

