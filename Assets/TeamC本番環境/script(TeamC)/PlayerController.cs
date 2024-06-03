using Cinemachine;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using TMPro;

namespace TeamC
{
    public class PlayerController : MonoBehaviour
    {
        public int playerId;
        public String playerName;
        [SerializeField] public int coin;
        [SerializeField] private GameObject cameraCheck;
        private Camera currentCamera;
        CinemachineDollyCart movePath;
        public int moveCount;
        public bool startFlag;
        public bool finishMoveToStartPos;
        public bool eventFlag;
        public bool coreFinishFlag = false;

        public int moveSpeed;

        //マスの中心で止まったかどうか
        public bool stopFlag;

        //マスに滞在するときの変数
        public bool panelStayFlag;
        public bool panelStayFinishFlag;

        // 現在自分がいるパネル
        GameObject currentPanel;
        // 分岐点のロジック
        GameObject junctionLogic;
        // 分岐点フラグ
        public bool junctionFlag;


        // ●●●勝山追加分●●●
        [SerializeField] private Animator animator; // キャラクターのアニメーター 
        GameObject player;
        private Vector3 playerPos;
        //[SerializeField] private GameObject chest;                           
        Animator chestAnimator;                     // 宝箱のアニメーター
        ParticleSystem chestParticle;               // 宝箱のエフェクト
        AudioSource chestAudioSource;               // 宝箱の開閉の音
        AudioSource warpAudioSource;                // ワープ音
        // ●●●勝山追加分●●●




        //プレイヤーの状態を（ターン開始、移動、後処理、待機）の4段階で分ける
        public enum PlayerState
        {
            StartPhase,
            MovePhase,
            EndPhase,
            StandbyPhase,
            //処理中フェーズを追加
            //主にイベント関連で、他の処理を止めたいときに使用
            ProcessPhase
        }
        public PlayerState playerState;

        public CinemachinePathBase pathA;
        public CinemachinePathBase newPath;

        public String PlayerName
        {
            get => playerName;
            set => playerName = value;
        }
        
        public List<ItemData> itemDataList = new List<ItemData>();   //プレイヤーの所持アイテム

        
        //■■■■■■↓長崎追記 5/14■■■■■■
        //Directorにターン終了処理を依頼するための変数 4/30
        public bool turnEndFlag = false;

        //Directorに、所持コイン増減を伝達するための変数
        public int updateOfCoins = 0;

        //プレイヤーのコアの所持数を保持しておくための変数
        [SerializeField] public int core;

        //残り移動数を一時的に退避するための変数
        public int tempMoveCount = 0;

        //Directorと接続するための変数
        public GameObject sugorokuGameDirector;

        //■■■■■■↑長崎追記ここまで■■■■■■

        void Start()
        {
            movePath = this.GetComponent<CinemachineDollyCart>();
            startFlag = true;
            finishMoveToStartPos = false;
            movePath.enabled = false;
            moveSpeed = 6;

            // 最初は待機状態
            playerState = PlayerState.StandbyPhase;

            // ●●●勝山追加分●●●
            // 初期位置を保持
            GameObject chest = GameObject.Find("chest");
            playerPos = transform.position;
            chestAnimator = chest.GetComponent<Animator>();
            chestParticle = chest.GetComponent<ParticleSystem>();
            chestAudioSource = chest.GetComponent<AudioSource>();
            GameObject warpSound = GameObject.Find("ワープのSE");
            warpAudioSource = warpSound.GetComponent<AudioSource>();
            // ●●●勝山追加分●●●


            //CameraCheckスクリプトをアタッチ
            cameraCheck = GameObject.Find("CameraCheck");

            //Pathをアタッチ
            pathA = GameObject.Find("PathA").GetComponent<CinemachinePathBase>();
            movePath.m_Path = pathA;

            //■■■■■■↓長崎追記 5/16 ここから■■■■■■
            //Directorと接続するため、Findメソッドでオブジェクトを取得
            sugorokuGameDirector = GameObject.Find("SugorokuGameDirector");
            
            //■■■■■■↑長崎追記 5/16 ここまで■■■■■■

            // 分岐ロジックをアタッチ
            junctionLogic = GameObject.Find("JunctionLogic");
            junctionFlag = false;

            // 停止をフラグ管理
            stopFlag = false;

            panelStayFlag = false;
            panelStayFinishFlag = false;
            //■■■■■■↑堀見追記ここまで■■■■■■

            var currentCamera = Camera.current;

            CountItem("dice_0", 2);
            CountItem("dice_1", 2);
            //CountItem("dice_2", 1);
            //CountItem("warp_0", 1);
        }

        // Update is called once per frame
        void Update()
        {
            //■■■■■■↓堀見追記 5/14■■■■■■
            // 開始状態の時、待機位置からパネルの中心に戻る
            if (playerState == PlayerState.StartPhase && currentPanel)
            {
                if (panelStayFlag)
                {
                    int stayCount = currentPanel.GetComponent<PanelController>().stayCount;
                    //パネルの待機人数を減らす
                    stayCount--;
                    if (stayCount < 0)
                    {
                        stayCount = 0;
                    }
                    currentPanel.GetComponent<PanelController>().stayCount = stayCount;
                    panelStayFlag = false;
                    panelStayFinishFlag = false;
                }

                Vector3 panelPos = currentPanel.transform.position;
                Vector3 playerPos = this.transform.position;
                Vector3 target = new Vector3(panelPos.x, playerPos.y, panelPos.z);
                Stop(playerPos, target);
            }

            // 分岐の矢印をクリック
            if (Input.GetMouseButtonDown(0) && cameraCheck)
            {
                // 現在のカメラを取得
                currentCamera = cameraCheck.GetComponent<CameraCheck>()._currentCamera;

                GameObject clickedArrow = null;

                Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray, out hit))
                {
                    clickedArrow = hit.collider.gameObject;
                    if (clickedArrow.tag == "Arrow" && junctionFlag && moveCount > 0)
                    {
                        //次に移るパスと種類とパス上の位置を取得
                        String path = clickedArrow.GetComponent<ArrowController>().selectPath.ToString();
                        newPath = GameObject.Find(path).GetComponent<CinemachinePathBase>();
                        float newPosition = clickedArrow.GetComponent<ArrowController>().positionPath;
                        // 矢印を非表示
                        junctionLogic.GetComponent<JunctionLogic>().ArrowHide(currentPanel);

                        // cinemachineを再度動かす
                        stopFlag = false;
                        junctionFlag = false;
                        movePath.m_Path = newPath;
                        movePath.m_Position = newPosition;
                        movePath.enabled = true;
                        movePath.m_Speed = moveSpeed;
                    }
                }
            }

            //■■■■■■↑堀見追記ここまで■■■■■■



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
            }
            else
            {
                animator.SetBool("WalkingBool", true);
            }
            // ●●●勝山追加分●●●
        }

        // Playerをスタート位置に移動
        public void MoveToStartPos(GameObject player, Vector3 startPos)
        {
            Vector3 playerPos = player.transform.position;

            float step = 2.0f * Time.deltaTime;
            if(!finishMoveToStartPos)
            {
                transform.position = Vector3.MoveTowards(playerPos, startPos, step);
            }
            if(playerPos == startPos)
            {
                finishMoveToStartPos = true;
            }
        }

        // Playerのスピードを操作
        // サイコロの目を引数にして出た目の数進む
        public void PlayerMove(int moveCount)
        {
            this.moveCount = moveCount;

            // 現在分岐点にいる場合は進まない
            if (junctionFlag)
            {
                Debug.Log("分岐点からのスタートです");
                junctionLogic.GetComponent<JunctionLogic>().ArrowActive(currentPanel);
            }
            else
            {
                movePath.enabled = true;
                movePath.m_Speed = moveSpeed;
            }

            //出目を受け取ってからStartFlagをTrue
            {
                startFlag = false;
            }

            eventFlag = false;

            // ターン開始時足元が緑マスでなければコアイベントフラグをリセット（緑マスでリセットするとイベントが再発する）
            if(currentPanel && currentPanel.GetComponent<PanelController>().panelState != PanelController.PanelState.Green)
            {
                coreFinishFlag = false;
            }

            stopFlag = false;
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
                PanelController.PanelState thisPanelState = panel.GetComponent<TeamC.PanelController>().panelState;
                bool panel_isJunction = panel.GetComponent<PanelController>().isJunction;

                if (!passFlag)
                {
                    passFlag = true;
                    if (this.moveCount > 0)
                    {
                        this.moveCount -= 1;
                        Debug.Log("残りの移動数" + moveCount);
                    }
                }

                // 残りのマス0 or 分岐点 or 緑マス
                if (moveCount == 0 || panel_isJunction || thisPanelState == PanelController.PanelState.Green)
                {

                    // パネルの中心座標
                    Vector3 target = new Vector3(panelPos.x, playerPos.y, panelPos.z);
                    // プレイヤーとの距離の計算
                    float distanceOfPlayer = Vector3.Distance(playerPos, target);

                    //CinemachineDollyCartを停止、手動で移動した距離分進めてから無効化
                    movePath.m_Speed = 0;
                    if (!startFlag)
                    {
                        movePath.m_Position += distanceOfPlayer;
                        //Debug.Log("positionAdd");
                    }
                    movePath.enabled = false;

                    //分岐点(最後のマスではないとき)での処理
                    if (panel_isJunction)
                    {
                        junctionFlag = true;
                        if (moveCount != 0)
                        {
                            Debug.Log("分岐点です");
                            junctionLogic.GetComponent<JunctionLogic>().ArrowActive(panel);
                        }
                    }
                }
            }
        }

        public void OnTriggerStay(Collider other)
        {

            if (other.gameObject.tag == "Panel" && playerState == PlayerState.MovePhase)
            {
                Vector3 panelPos = other.transform.position;
                Vector3 playerPos = this.transform.position;
                Vector3 target = new Vector3(panelPos.x, playerPos.y, panelPos.z);

                GameObject panel = other.gameObject;
                currentPanel = panel;
                bool isJunction = panel.GetComponent<PanelController>().isJunction;
                PanelController.PanelState thisPanelState = panel.GetComponent<TeamC.PanelController>().panelState;

                if (isJunction && junctionFlag)
                {
                    // パネルの中心座標まで移動
                    Stop(playerPos, target);
                }

                //Greenマス(コア購入マス)進入時にmoveCountが1以上、つまりGreenマスで止まらずに通過しようとした場合
                if (thisPanelState == PanelController.PanelState.Green && moveCount != 0)
                {
                    // パネルの中心座標まで移動
                    Stop(playerPos, target);
                    if(playerPos == target && !coreFinishFlag)
                    {
                        StartCoroutine(BuyCoreEventCoroutine());
                    }

                    if (playerState == PlayerState.MovePhase && coreFinishFlag)
                    {

                        // cinemachineを再度動かす
                        stopFlag = false;
                        movePath.enabled = true;
                        movePath.m_Speed = moveSpeed;
                    }

                }

                if (moveCount == 0)
                {

                    // パネルの中心座標まで移動
                    Stop(playerPos, target);

                    if (!eventFlag && !startFlag)
                    {
                        eventFlag = true;
                        //止まったマスに応じてイベント
                        Debug.Log(panel.GetComponent<TeamC.PanelController>().panelState + "のマスにとまりました");

                        //■■■■■■↓長崎追記 5/21 ここから■■■■■■
                        //止まったパネルが青、赤、黄のいずれかだった場合 5/21 長崎編集
                        if (thisPanelState == PanelController.PanelState.Blue
                            || thisPanelState == PanelController.PanelState.Red
                            || thisPanelState == PanelController.PanelState.Yellow)
                        {
                            //止まったプレイヤーの所持コイン数を更新する
                            int coinsOfPanel = panel.GetComponent<TeamC.PanelController>().coin_num;
                            this.coin += coinsOfPanel;
                            Debug.Log("このプレイヤーの現在所持コイン数(this.coin):" + this.coin);

                            //Directorとの連携用変数へ、所持コインを代入
                            //DirectorでUI更新等の処理が終わったら、初期値0に戻される
                            updateOfCoins = coinsOfPanel;
                            Debug.Log("updateOfCoins:" + updateOfCoins);

                            //黄色マスの場合
                            if (thisPanelState == PanelController.PanelState.Yellow)
                            {
                                Debug.Log("黄色マスに止まった時の設定獲得コイン:" + panel.GetComponent<PanelController>().coin_num);
        
                                //黄色マスで獲得できる枚数を減らす
                                panel.GetComponent<PanelController>().coin_num -= 20;

                                // ●●●勝山追記●●●
                                chestAnimator.SetTrigger("openTrigger");
                                chestAudioSource.Play();
                                chestParticle.Play();

                                Debug.Log("減らした設定獲得コイン:" + panel.GetComponent<PanelController>().coin_num);
                            }
                        }
                        //■■■■■■↑長崎追記 5/21 ここから■■■■■■

                        // アイテムゲットマス yellow‗2
                        if (thisPanelState == PanelController.PanelState.Yellow_2)
                        {
                            sugorokuGameDirector.GetComponent<SugorokuGameDirector>().ItemGetEventStart();
                        }


                        // 緑マス、アイテムマスの場合、イベント完了でターン終了
                        if (thisPanelState != PanelController.PanelState.Green
                        && thisPanelState != PanelController.PanelState.Yellow_2)
                        {
                            turnEndFlag = true;

                        }
                    }
                    // 残り移動0の時の緑マスのイベント（マスの中心で発火）
                    if (thisPanelState == PanelController.PanelState.Green && playerPos == target && !coreFinishFlag)
                    {
                        StartCoroutine(BuyCoreEventCoroutine());
                    }
                }


            }
        }
        // パネル中心で泊まるメソッド
        public void Stop(Vector3 playerPos, Vector3 target)
        {
            if (!stopFlag || playerState == PlayerState.StartPhase)
            {
                float step = 2.0f * Time.deltaTime;
                // パネルの中心座標まで移動
                transform.position = Vector3.MoveTowards(playerPos, target, step);

                if (playerPos == target)
                {
                    stopFlag = true;
                }
            }
        }
        // 自ターン以外はマス外で待機
        public void StayOutside()
        {
            if (currentPanel)
            {
                int stayCount = currentPanel.GetComponent<PanelController>().stayCount;

                //パネルの待機位置リストを取得
                List<GameObject> posList = currentPanel.GetComponent<PanelController>().PosList;

                Vector3 playerPos = this.transform.position;
                Vector3 pos = posList[stayCount].transform.position;
                Vector3 target = new Vector3(pos.x, playerPos.y, pos.z);
                float step = 2.0f * Time.deltaTime;
                // パネルの中心座標まで移動
                transform.position = Vector3.MoveTowards(playerPos, target, step);
                if (playerPos == target)
                {
                    panelStayFinishFlag = true;
                }

                if (!panelStayFlag && panelStayFinishFlag)
                {
                    //パネルの待機人数を増やす
                    stayCount++;
                    Debug.Log("StayCount" + stayCount);
                    if (stayCount >= 3)
                    {
                        stayCount = 0;
                    }
                    currentPanel.GetComponent<PanelController>().stayCount = stayCount;
                    panelStayFlag = true;
                }
            }

        }
        //アイテムを取得
        public void CountItem(string itemId, int count)
        {
            //List内を検索
            for (int i = 0; i < itemDataList.Count; i++)
            {
                //IDが一致していたらカウント
                if (itemDataList[i].id == itemId)
                {
                    //アイテムをカウント
                    itemDataList[i].CountUp(count);
                    return;
                }
            }

            //IDが一致しなければアイテムを追加
            ItemData itemData = new ItemData(itemId, count);
            itemDataList.Add(itemData);
        }

        //アイテムを使用
        public void UseItem(string itemId, int count)
        {
            //List内を検索
            for (int i = 0; i < itemDataList.Count; i++)
            {
                //IDが一致していたらカウント
                if (itemDataList[i].id == itemId)
                {
                    //アイテムをカウントダウン
                    itemDataList[i].CountDown(count);
                    break;
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            passFlag = false;
        }

        //■■■■■■↓長崎追記 5/16 ここから■■■■■■
        //コア購入イベントを呼び出すためのコルーチンメソッド
        public IEnumerator BuyCoreEventCoroutine()
        {
            //イベント処理前に、プレイヤーの現在のPlayerStateを取得し、退避用変数に保存
            int tempPlayerState = (int)playerState;

            //PlayerStateをProcessPhaseへ変更し、他の処理が動かないよう一時停止させる
            playerState = PlayerState.ProcessPhase;

            yield return sugorokuGameDirector.GetComponent<SugorokuGameDirector>().BuyCoreEvent();

            //退避していた変数に基づいて、PlayerStateをイベント処理前の状態に戻す
            playerState = (PlayerState)tempPlayerState;

        }
        //■■■■■■↑長崎追記 5/16 ここから■■■■■■

        // プレイヤーをワープさせる
        public void playerWarp(GameObject panel)
        {
            currentPanel = panel;
            Vector3 panelPos = panel.transform.position;
            float pathPos = panel.GetComponent<PanelController>().pathPos;

            // ワープ先が分岐点だった場合
            if (panel.GetComponent<PanelController>().isJunction)
            {
                junctionFlag = true;
            }

            this.transform.position = panelPos;
            //●●●勝山追記●●●
            warpAudioSource.Play();
            //●●●勝山追記●●●

            movePath.m_Path = pathA;
            movePath.m_Position = pathPos;
        }

    }
}