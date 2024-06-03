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
        private Vector3 mainCameraStartPos;     // メインカメラの初期位置
        private Quaternion mainCameraStartRota; // メインカメラの初期回転
        private Vector3 overViewCameraStartPos; // overViewカメラの初期位置
        Transform coinUpDownTextPos;             // コイン増減テキストの初期情報

        void Start()
        {
            // メインカメラの初期位置・初期回転の取得
            mainCameraStartPos = mainCamera.transform.position;
            mainCameraStartRota = mainCamera.transform.rotation;
            overViewCameraStartPos = new Vector3(-40, 30, -22);

            // メインカメラとNearカメラとoverViewカメラは初期状態では無効化
            mainCamera.enabled = false;
            playerNearCamera.enabled = false;
            overViewCamera.enabled = false;


            selectCamera = playerFarCamera;
            coinUpDownTextPos = coinUpDownText.transform;
        }

        void LotateUpdate()
        {
            // マウスの左ボタンが押された時
            //if (Input.GetKeyDown(KeyCode.Space))

            Debug.Log("moveCountは " + cc.moveCount);

        }


        public void WorldMapView()
        {
            mainCamera.enabled = true;
            playerFarCamera.enabled = false;
            playerNearCamera.enabled = false;
            overViewCamera.enabled = false;

            // WorldMapボタンを押すたびに最初の位置に戻る
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

            // WorldMapボタンを押すたびに最初の位置に戻る
            //overViewCamera.transform.position = overViewCameraStartPos;
            
        }
    }
}

