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



        // ●●●勝山追加分●●●
        [SerializeField] private Animator animator;
        GameObject player;
        private Vector3 playerPos;
        // playerとカメラのTransform
        [SerializeField] private Transform _self;   // player
        [SerializeField] private Transform _target; // カメラ
        // ●●●勝山追加分●●●


        //■■■■■■↓長崎追記 4/30■■■■■■
        //Directorにターン終了処理を依頼するための変数
        public bool turnEndFlag = false;

        //■■■■■■↑長崎追記ここまで■■■■■■

        void Start()
        {
            movePath = this.GetComponent<CinemachineDollyCart>();
            startFlag = true;
            movePath.enabled = false;
            //PlayerMove(testMoveCount);

            // ●●●勝山追加分●●●
            // 初期位置を保持
            playerPos = transform.position;
            // ●●●勝山追加分●●●
        }

        // Update is called once per frame
        void Update()
        {


            // ●●●勝山追加分●●●
            // 現在位置取得
            var position = transform.position;
            // 現在速度計算
            var velocity = (position - playerPos) / Time.deltaTime;
            // 前フレーム位置を更新
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
            // ●●●勝山追加分●●●
        }

        // Playerのスピードを操作
        // サイコロの目を引数にして出た目の数進む
        public void PlayerMove(int moveCount)
        {
            this.moveCount = moveCount;
            movePath.enabled = true;
            movePath.m_Speed = 2;

            eventFlag = false;

        }

        bool passFlag = false;
        // Panelに接触するたびにmoveCountを減少させる
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
                        Debug.Log("残りの移動数" + moveCount);
                    }
                    else
                    {

                    }
                }
                if (this.moveCount == 0)
                {

                    // パネルの中心座標
                    Vector3 target = new Vector3(panelPos.x, playerPos.y, panelPos.z);
                    // プレイヤーとの距離の計算
                    float distanceOfPlayer = Vector3.Distance(playerPos, target);



                    //CinemachineDollyCartを停止、手動で移動した距離分進めてから無効化
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

                    // パネルの中心座標まで移動
                    Vector3 target = new Vector3(panelPos.x, playerPos.y, panelPos.z);

                    float step = 2.0f * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(playerPos, target, step);


                    if (!eventFlag)
                    {
                        eventFlag = true;

                        //■■■■■■↓長崎追記 4/30■■■■■■
                        //止まったマスに応じてイベント
                        //Debug.Log(panel.GetComponent<TeamC.PanelController>().panelState + "のマスにとまりました");

                        //プレイヤーの行動が終了したため、フラグをtrueにする
                        //今後の改修具合に応じて、下記コードの位置を変更してください
                        turnEndFlag = true;

                        //■■■■■■↑長崎追記ここまで■■■■■■
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