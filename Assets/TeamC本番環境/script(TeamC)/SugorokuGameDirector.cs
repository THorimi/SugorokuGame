using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static UnityEngine.ParticleSystem;
using static UnityEngine.Rendering.DebugUI;

//4/30 長崎作成
namespace TeamC
{

    //ターン進行を制御するスクリプト
    public class SugorokuGameDirector : MonoBehaviour
    {
        // eventSystemを取得するための変数宣言
        [SerializeField] EventSystem eventSystem;

        //各種UIと接続するための変数宣言
        public GameObject MenuPanel; //コマンド選択のパネル
        public GameObject SelectPanel; //はい、いいえの選択パネル
        public GameObject RollDiceButton; //ダイスロールの実行ボタン
        public GameObject ItemListPanel; //プレイヤーの所持アイテムリストUI
        public GameObject GuidePanel; //ダイスロール操作ガイドのUI
        public GameObject MessagePanel; //メッセージパネルUI
        public GameObject MessageBox; //メッセージパネルUIのテキスト部分

        public GameObject moveCountText; //移動中の、残り移動数表示UI

        public List<GameObject> playerInfoPanelList; //プレイヤーのUI表示
        public List<GameObject> coinTextList; //所持コインのUI表示

        public List<string> tutorialTexts; //チュートリアルの文章を格納

        //■■■■■■↓堀見追記 5/22 ここから■■■■■■
        //コア関連の変数
        // このターンにコアを購入しているかどうか
        private bool coreGetFlag;
        // 現在緑マスになっているマスのインデックス
        private int greenPanelIndex;
        // 緑マスの元の色
        private PanelController.PanelState greenPanelPrevState;
        // 緑マスになるマス
        private List<GameObject> greenPanelList;
        // 緑マスになるパネル
        [SerializeField] private GameObject panelGreen_0;
        [SerializeField] private GameObject panelGreen_1;
        [SerializeField] private GameObject panelGreen_2;
        // 緑マスの手前のマス
        private List<GameObject> greenPanelBeforeList;
        [SerializeField] private GameObject panelGreenBefore_0;
        [SerializeField] private GameObject panelGreenBefore_1;
        [SerializeField] private GameObject panelGreenBefore_2;
        // ワープ時に使用するフラグ
        private bool warpFlag = false;


        //■■■■■■↓長崎追記 5/23 ここから■■■■■■
        //アニメーション用のコアのプレファブ
        public GameObject coreObject;

        //ゲーム開始時のサウンド
        public AudioSource turnStartSound;

        //コア獲得時のサウンド
        public AudioSource getCoreSound;

        //サイコロが回転している間にループ再生させる音源
        public AudioSource diceRollingSound;

        //サイコロが停止した際に再生させる音源
        public AudioSource diceStopSound;

        public List<GameObject> coreTextList; //所持コア数のUI表示

        //プレイヤー間のランキングを決めるためのクラス
        public class Player
        {
            //順位の数値
            public int Rank;

            //プレイヤー名
            public string playerName;

            //所持コアの数
            public int coreCount;

            //所持コインの数
            public int coinCount;

            //残りターン0で集計する際、playerクラスのインスタンス生成する処理を想定したコンストラクタ
            public Player(string playerName, int coreCount, int coinCount)
            {
                //順位以外のデータを受け取って、インスタンス生成
                this.playerName = playerName;
                this.coreCount = coreCount;
                this.coinCount = coinCount;
            }
        }

        //上記のクラスを要素とするリスト
        public List<Player> rankingList;

        //各位の位置に表示させるUI
        public GameObject firstRankText,firstPlayerNamePanelText,firstCoreCountText, firstCoinCountText;
        public GameObject secondRankText, secondPlayerNamePanelText, secondCoreCountText, secondCoinCountText;
        public GameObject thirdRankText, thirdPlayerNamePanelText, thirdCoreCountText, thirdCoinCountText;
        public GameObject fourthRankText, fourthPlayerNamePanelText, fourthCoreCountText, fourthCoinCountText;

        //上記の各UIを格納するリスト
        public List<GameObject> firstPanel, secondPanel, thirdPanel, fourthPanel;

        //上記のリストを格納するリザルト用リスト
        public List<List<GameObject>> ResultPanelsList;

        //リザルト表示用UI
        public GameObject ResultPanel;
        //■■■■■■↑長崎追記 5/21 ここまで■■■■■■
        //ターンカウントUI
        public GameObject turnCountText;

        //ゲームスタートUI
        public GameObject playerNumSelect;
        [SerializeField] private GameObject gameStartButton;


        //■■■■■■↓堀見追記 5/15■■■■■■
        // 後処理フェーズのテキストを格納するリスト
        private List<string> endMessageList;
        //■■■■■■↑堀見追記ここまで■■■■■■

        private bool gameStart = false;

        //プレイヤー生成
        public GameObject playerGenerator;

        // プレイヤーを格納するリスト
        public List<GameObject> playerList;

        //プレイヤー人数
        public int playerNum;

        // 現在プレイ中のプレイヤーのID
        public int currentId = -1;

        // スタートポジション
        Vector3 startPos = new Vector3(-0.620000005f, 1, -4.03999996f);

        //ターンが回ってきたプレイヤーを格納するプレイヤー変数と、座標変数
        public GameObject currentPlayer;
        Vector3 currentPlayerPos;

        //ダイスオブジェクトのプレファブ
        public GameObject dicePrefab;
        public GameObject redDicePrefab;
        public GameObject blueDicePrefab;
        public GameObject purpleDicePrefab;

        //ダイスオブジェクト変数
        GameObject dice01;
        GameObject dice02;

        //ダイスオブジェクトの座標
        Vector3 dice01Pos;
        Vector3 dice02Pos;

        //■■■■■■↓長崎追記 5/23 ここから■■■■■■
        //仮置きターン数
        public int turnCount = 30;
        //■■■■■■↑長崎追記 5/23 ここから■■■■■■
        // ダイスステータス
        private string diceRollingFlag;
        //ダイスの出目を保存する変数
        private int numOfDice01;

        //現プレイヤーの所持アイテム
        private List<ItemData> itemDataList = new List<ItemData>();


        // アイテム画面判定ステータス
        public enum ItemSelectState
        {
            Unset,
            UseItem, //使用フェーズ
            PurchaseItem, // 購入フェーズ
            PurchaseItem_phase2, // 購入フェーズ2
        }
        public ItemSelectState itemSelectState = ItemSelectState.Unset;
        // アイテムボタンプレファブ
        public GameObject itemListButtonPrefab;

        // アイテムマネージャー
        public GameObject itemManager;
        // 現在クリックされているオブジェクト（アイテム選択用）
        GameObject selectedObj;
        //選択されたアイテム
        ItemListButtonController selectedItem;
        // 使用したアイテムのID（メソッドに渡す）
        public string useItemId = "";

        // アイテムショップのアイテムリスト
        private List<ItemData> itemShopDataList = new List<ItemData>();

        // メッセージパネルに書いてあったメッセージを一時的に格納する変数
        private string prevText = "";
        //prevTextが格納されているか？
        private bool prevTextFlag;

        // ●●●●●●↓勝山追記　5/14　●●●●●●

        // コインオブジェクトのプレファブ
        public GameObject coinsPrefab;
        // コインオブジェクトのリスト変数
        //List<GameObject> coins = new List<GameObject>();
        GameObject[] coins = new GameObject[5];
        // コインオブジェクトの表示時間と次のコインの生成までの時間
        float displayTime = 1f;
        float waitTime = 0.4f;
        // コインの増減
        public int coinNum;
        public GameObject coinUpDownTextObject;
        // カメラ
        [SerializeField] private GameObject cameraCheck;
        private Camera currentCamera;
        [SerializeField] private Camera playerFarCamera;
        [SerializeField] private Camera playerNearCamera;

        TextMeshPro coinUpDownText;

        // ●●●●●●↑勝山追記　5/14　●●●●●●
        //残りターン数
        private int leftTurnCount;

        //■■■■■■↓長崎追記 5/17 ここから■■■■■■
        //はい、いいえボタンなど、特定ボタンをクリックすることで変化するステータス
        public enum SelectState
        {
            Yes,
            No,
            ReadyForSelect //選択UIを表示していない、または選択待ちの状態
        }

        //SelectStateの変数宣言と初期化
        public SelectState selectState = SelectState.ReadyForSelect;
        //■■■■■■↑長崎追記 5/17 ここまで■■■■■■

        //プレイヤー数を決めてゲームスタート
        public void GameStart()
        {
            playerNum = playerNumSelect.GetComponent<TeamC.PlayerNumSelect>().playerNum + 1;
            Debug.Log("プレイヤー数" + (playerNum));

            //プレイヤーを生成してリストに格納
            playerList = playerGenerator.GetComponent<TeamC.PlayerGenerator>().PlayerGenerate(playerNum);

            // 5/10 所持コイン機能追加:長崎
            for(int i = 0; i < playerList.Count;i++) 
            {
                //所持コイン数をUIへ反映させる
                coinTextList[i].GetComponent<TextMeshProUGUI>().text
                    = "coin:" + playerList[i].GetComponent<TeamC.PlayerController>().coin.ToString();
            }

            //非参加していないプレイヤー枠の表示を非アクティブにする
            //(例：2人参加なら、i=2で、player3と4の表示を消す)
            for(int i = playerList.Count; i < 4;i++) 
            {
                //非表示にする処理
                playerInfoPanelList[i].SetActive(false);
            }

            // プレイヤー生成UIを消す
            playerNumSelect.SetActive(false);
            gameStartButton.SetActive(false);

            currentId = -1;

            Tutorial();

        }
        public void Tutorial()
        {
            MessageBox.GetComponent<TextMeshProUGUI>().text = "すごろくゲームへようこそ！";
            tutorialTexts = new List<string>();
            tutorialTexts.Add("すごろくをあそんで" + Environment.NewLine + "１いをめざしましょう。");
            tutorialTexts.Add("あおいマスでコインをゲット、" + Environment.NewLine + "あかいマスでコインをうしないます。");
            tutorialTexts.Add("みどりのマスにとまると、" + Environment.NewLine +  "Coin50まいでCoreをかうことができます。");
            tutorialTexts.Add("すべてのターンがおわったときに" + Environment.NewLine + "Coreをいちばんもっていたひとのかちです。" + Environment.NewLine + "せっきょくてきにゲットしていきましょう！");
            //tutorialTexts.Add("");
        }

        public void TurnManage(int id)
        {
            if (currentPlayer)
            {
                // 前のプレイヤーを待機状態に変更
                currentPlayer.GetComponent<PlayerController>().playerState = PlayerController.PlayerState.StandbyPhase;
            }

            currentPlayer = playerList[id];

            //メッセージパネルに、現在誰のターンなのかを表示 5/13長崎追加
            //
            MessageBox.GetComponent<TextMeshProUGUI>().text = 
                currentPlayer.GetComponent<TeamC.PlayerController>().playerName +  "さんのターンです";

            //■■■■■■↓長崎追記 5/22 ここから■■■■■■
            //ターン開始時のサウンド再生
            turnStartSound.Play();
            //■■■■■■↑長崎追記 5/22 ここまで■■■■■■

            Debug.Log(id + "番目のプレイヤーのターンです。");

            // プレイヤーを開始状態に変更
            currentPlayer.GetComponent<PlayerController>().playerState = PlayerController.PlayerState.StartPhase;

            // メニューを表示
            MenuPanel.SetActive(true);

            // アイテムリストをCurrentPlayerのものに変更
            StartCoroutine("setPlayerItemList");

        }
        public void MessageClicked()
        {
            //開始フェーズ、ワープの時
            if (warpFlag)
            {
                GameObject warpPanel = greenPanelBeforeList[greenPanelIndex];
                currentPlayer.GetComponent<PlayerController>().playerWarp(warpPanel);

                MessageBox.GetComponent<TextMeshProUGUI>().text = "ワープしました。";
                MenuPanel.SetActive(true);
                warpFlag = false;
            }
            //後処理フェーズの時、すべてのメッセージを読み終わったら次のプレイヤーのターンに移る
            if (currentPlayer && currentPlayer.GetComponent<PlayerController>().playerState == PlayerController.PlayerState.EndPhase && itemSelectState != ItemSelectState.PurchaseItem)
            {
                if (endMessageList.Count > 0)
                {
                    MessageBox.GetComponent<TextMeshProUGUI>().text = endMessageList[0];
                    endMessageList.RemoveAt(0);
                } 
                else
                {
                    //次のターンへ
                    NextTurn();
                }
            }

            if(!gameStart)
            {
                if (tutorialTexts.Count > 0)
                {
                    MessageBox.GetComponent<TextMeshProUGUI>().text = tutorialTexts[0];
                    tutorialTexts.RemoveAt(0);
                }
                else
                {
                    gameStart = true;
                    // 1ターン目スタート
                    NextTurn();
                }


            }
        }
        public void NextTurn()
        {
            //行動選択UIを表示
            MenuPanel.SetActive(true);

            //ターンカウントを一つ減らして表示
            if (currentId + 1 >= playerNum) 
            {
                leftTurnCount--;
            }

            turnCountText.GetComponent<TextMeshProUGUI>().text = "turnCount:" + leftTurnCount.ToString();


            //残りターンが0になったら、仮のリザルト画面へ遷移
            if (leftTurnCount == 0)
            {
                //■■■■■■↓長崎追記 5/21 ここから■■■■■■
                //リザルトUIをアクティブにする
                ResultPanel.SetActive(true);

                //リザルトが表示されたら、他のUIをリザルトの背面にして、操作できないようにする処理

                //playerList(GameObjectのリスト)から、RankingListを作成
                rankingList = new List<Player>();

                //プレイヤーの名前、所持コア数、所持コイン数から、Playerクラスのインスタンスを生成
                for (int i = 0; i < playerList.Count; i++)
                {
                    Player player = new Player(playerList[i].GetComponent<PlayerController>().playerName,
                        playerList[i].GetComponent<PlayerController>().core,
                        playerList[i].GetComponent<PlayerController>().coin);

                    //順位は初期値0のままで、プレイヤーをランキングリストに追加する
                    rankingList.Add(player);
                }

                // コア数の降順でプレイヤーをソート
                rankingList.Sort((a, b) => b.coreCount.CompareTo(a.coreCount));

                // コア数が同じプレイヤー間について、コイン数の降順でソートする
                rankingList.Sort((a, b) =>
                {
                    if (a.coreCount == b.coreCount)
                    {
                        // コア数が同じ場合は、コイン数の多さで降順ソート
                        return b.coinCount.CompareTo(a.coinCount);
                    }
                    else
                    {
                        // コア数が異なる場合はソートしない
                        return 0;
                    }
                });

                //rankingList内のプレイヤーの持つRank変数について、並び順で1～4を割り当てる
                //コアとコインが一致しているプレイヤー同士は同率とし、同じ順位にする
                for (int i = 0; i < rankingList.Count; i++)
                {
                    if (i > 0 &&
                        rankingList[i].coreCount == rankingList[i - 1].coreCount &&
                        rankingList[i].coinCount == rankingList[i - 1].coinCount)
                    {
                        rankingList[i].Rank = rankingList[i - 1].Rank;
                    }
                    else
                    { 
                        rankingList[i].Rank = (i + 1);
                    }
                }

                //UIと連携しているリストへ、RankingListの各表示項目を代入する
                for(int i = 0;i <  ResultPanelsList.Count; i++)
                {
                    //参加人数分だけ、リザルトを表示
                    if ((rankingList.Count - 1) >= i)
                    {
                        ResultPanelsList[i][0].GetComponent<TextMeshProUGUI>().text = rankingList[i].Rank.ToString();
                        ResultPanelsList[i][1].GetComponent<TextMeshProUGUI>().text = rankingList[i].playerName.ToString();
                        ResultPanelsList[i][2].GetComponent<TextMeshProUGUI>().text = "core:\n" + rankingList[i].coreCount.ToString();
                        ResultPanelsList[i][3].GetComponent<TextMeshProUGUI>().text = "coin:\n" + rankingList[i].coinCount.ToString();
                    }
                    //プレイ人数が3人以下で、リザルトに空欄が発生する場合は、表示をハイフンにする
                    else
                    {
                        ResultPanelsList[i][0].GetComponent<TextMeshProUGUI>().text = "--";
                        ResultPanelsList[i][1].GetComponent<TextMeshProUGUI>().text = "--";
                        ResultPanelsList[i][2].GetComponent<TextMeshProUGUI>().text = "--";
                        ResultPanelsList[i][3].GetComponent<TextMeshProUGUI>().text = "--";
                    }
                }
                //■■■■■■↑長崎追記 5/21 ここまで■■■■■■
            }

            currentId = currentId < playerNum - 1 ? currentId + 1 : 0;

            //プレイヤーにターンを回す処理
            TurnManage(currentId);
        }

        void Start()
        {
            //UIの準備
            turnCountText.GetComponent<TextMeshProUGUI>().text = "turnCount:" + turnCount.ToString();
            endMessageList = new List<string>();
            // 勝山追記 5/21
            coinUpDownText = coinUpDownTextObject.GetComponent<TextMeshPro>();

            //UIを非表示にする
            SelectPanel.SetActive(false);
            GuidePanel.SetActive(false);
            MenuPanel.SetActive(false);
            ItemListPanel.SetActive(false);
            coinUpDownTextObject.SetActive(false);

            //残りのターン数
            leftTurnCount = turnCount;

            //■■■■■■↓長崎追記 5/20 ここから■■■■■■
            //各順位のパネルをそれぞれリストへ格納する
            firstPanel = new() { firstRankText, firstPlayerNamePanelText, firstCoreCountText, firstCoinCountText };
            secondPanel = new() { secondRankText, secondPlayerNamePanelText, secondCoreCountText, secondCoinCountText };
            thirdPanel = new() { thirdRankText, thirdPlayerNamePanelText, thirdCoreCountText, thirdCoinCountText };
            fourthPanel = new() { fourthRankText, fourthPlayerNamePanelText, fourthCoreCountText, fourthCoinCountText };

            //上記のリストをさらにリストへ格納する
            ResultPanelsList = new() { firstPanel, secondPanel, thirdPanel, fourthPanel };
            //■■■■■■↑長崎追記 5/20 ここから■■■■■■

            //flagの初期化
            prevTextFlag = true;
            coreGetFlag = false;

            // 緑マスになる予定のパネル一覧
            greenPanelList = new List<GameObject>();
            greenPanelList.Add(panelGreen_0);
            greenPanelList.Add(panelGreen_1);
            greenPanelList.Add(panelGreen_2);

            // 緑マス手前のパネル一覧
            greenPanelBeforeList = new List<GameObject>();
            greenPanelBeforeList.Add(panelGreenBefore_0);
            greenPanelBeforeList.Add(panelGreenBefore_1);
            greenPanelBeforeList.Add(panelGreenBefore_2);

            greenPanelIndex = 0;

            greenPanelPrevState = PanelController.PanelState.Blue;

            // アイテムショップにアイテムを入れる
            CountItem("dice_0", 1);
            CountItem("dice_1", 1);
            CountItem("dice_2", 1);
            CountItem("warp_0", 1);
        }




        //サイコロを回すボタンがクリックされたら実行
        public void RollDiceButtonClicked()
        {
            //プレイヤーがスタート位置についていたら押せる
            if (currentPlayer.GetComponent<PlayerController>().finishMoveToStartPos)
            {
                //ダイスの操作ガイドを表示
                GuidePanel.SetActive(true);
            
                //プレイヤーの座標を取得
                float x = currentPlayer.transform.position.x;
                float y = currentPlayer.transform.position.y;
                float z = currentPlayer.transform.position.z;
                currentPlayerPos = new Vector3(x, y, z);


                //プレファブからダイスオブジェクトを生成
                if (useItemId == "dice_0")
                {
                    dice01 = Instantiate(redDicePrefab);
                }
                else if (useItemId == "dice_1")
                {
                    dice01 = Instantiate(blueDicePrefab);
                }
                else if (useItemId == "dice_2")
                {
                    dice01 = Instantiate(purpleDicePrefab);
                    dice02 = Instantiate(purpleDicePrefab);
                }
                else
                {
                    dice01 = Instantiate(dicePrefab);

                }

                //生成する予定のダイスの座標を設定しておく(プレイヤーの頭上)
                dice01Pos = new Vector3(x, y + 3, z);
                if (dice02)
                {
                    dice01Pos = new Vector3(x - 0.8f, y + 3, z);
                    dice02Pos = new Vector3(x + 0.8f, y + 3, z);
                }

                // ダイスオブジェクトから出目の変数を受け取るメソッドはupdateメソッドで呼び出すように変更　堀見

                //先ほど設定した座標へサイコロを移動
                dice01.transform.position = dice01Pos;
                if (dice02)
                {
                    dice02.transform.position = dice02Pos;
                }

                //各種UIを非表示にする
                MenuPanel.SetActive(false);
                MessagePanel.SetActive(false);
            }
        }

        // アイテムを使うボタンがクリックされたら実行
        public void UseItemButtonClicked()
        {
            //各種UIの表示・非表示
            MenuPanel.SetActive(false);
            ItemListPanel.SetActive(true);

            // アイテム使用フェーズに移行
            itemSelectState = ItemSelectState.UseItem;
        }
        //ターン開始時にプレイヤーの所持アイテムリストを生成するメソッド
        public IEnumerator setPlayerItemList()
        {
            //1フレーム停止
            yield return null;

            // ボタンを動的生成するgameObject
            Transform list = ItemListPanel.transform.Find("Scrollview/Viewport/Content");
            //Contentの中身を全削除
            foreach (Transform n in list.transform)
            {
                GameObject.Destroy(n.gameObject);
            }

            // プレイヤーの所持アイテムリスト
            itemDataList = currentPlayer.GetComponent<PlayerController>().itemDataList;
            foreach (ItemData itemData in itemDataList)
            {
                if (itemData.count <= 0)
                {
                    continue;
                }
                //プレハブからボタンを生成
                GameObject listButton = Instantiate(itemListButtonPrefab) as GameObject;
                //Vertical Layout Group の子にする
                listButton.transform.SetParent(list, false);

                ItemSourceData item = itemManager.GetComponent<ItemManager>().GetItemSourceData(itemData.id.ToString());

                ItemType itemType = item.itemType;
                string itemId = item.id;
                string itemName = item.itemName;
                string itemText = item.itemText;
                int itemPrice = item.buyingPrice;

                //ボタンのラベルを変える
                //※ Textコンポーネントを扱うには using UnityEngine.UI; が必要
                listButton.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = itemName;
                listButton.transform.Find("ItemNum").GetComponent<TextMeshProUGUI>().text = "×" + itemData.count;
                //ボタンにアイテムの情報を持たせる
                listButton.GetComponent<ItemListButtonController>().itemType = itemType;
                listButton.GetComponent<ItemListButtonController>().itemId = itemId;
                listButton.GetComponent<ItemListButtonController>().itemName = itemName;
                listButton.GetComponent<ItemListButtonController>().itemText = itemText;
            }
        }
        //アイテムショップのアイテムリストを生成
        public IEnumerator setItemShopList()
        {
            //1フレーム停止
            yield return null;

            // ボタンを動的生成するgameObject
            Transform list = ItemListPanel.transform.Find("Scrollview/Viewport/Content");
            //Contentの中身を全削除
            foreach (Transform n in list.transform)
            {
                GameObject.Destroy(n.gameObject);
            }

            // アイテムリスト
            foreach (ItemData itemData in itemShopDataList)
            {
                if (itemData.count <= 0)
                {
                    continue;
                }
                //プレハブからボタンを生成
                GameObject listButton = Instantiate(itemListButtonPrefab) as GameObject;
                //Vertical Layout Group の子にする
                listButton.transform.SetParent(list, false);

                ItemSourceData item = itemManager.GetComponent<ItemManager>().GetItemSourceData(itemData.id.ToString());

                ItemType itemType = item.itemType;
                string itemId = item.id;
                string itemName = item.itemName;
                string itemText = item.itemText;
                int itemPrice = item.buyingPrice;

                //ボタンのラベルを変える
                //※ Textコンポーネントを扱うには using UnityEngine.UI; が必要
                listButton.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = itemName;
                listButton.transform.Find("ItemNum").GetComponent<TextMeshProUGUI>().text = "Price: " + itemPrice;
                //ボタンにアイテムの情報を持たせる
                listButton.GetComponent<ItemListButtonController>().itemType = itemType;
                listButton.GetComponent<ItemListButtonController>().itemId = itemId;
                listButton.GetComponent<ItemListButtonController>().itemName = itemName;
                listButton.GetComponent<ItemListButtonController>().itemText = itemText;
                listButton.GetComponent<ItemListButtonController>().itemPrice = itemPrice;
            }
        }
        //アイテムパネルからメニューに戻る
        public void CloseItemPanel()
        {
            if (itemSelectState != ItemSelectState.PurchaseItem_phase2)
            {
                //各種UIの表示・非表示
                MenuPanel.SetActive(true);
                ItemListPanel.SetActive(false);
                itemSelectState = ItemSelectState.Unset;
            }
            else
            {
                ItemListPanel.SetActive(false);
                //アイテム購入フェーズ終了
                itemSelectState = ItemSelectState.Unset;
                //イベント処理後、ターン終了フラグをtrueにする
                currentPlayer.GetComponent<PlayerController>().turnEndFlag = true;
            }
        }
        // アイテムを選択する
        public void SelectItem()
        {
            string selectText = "";
            if(itemSelectState == ItemSelectState.UseItem)
            {
                selectText = "このアイテムをつかいますか?";
            } 
            else if (itemSelectState == ItemSelectState.PurchaseItem_phase2)
            {
                selectText = "このアイテムをかいますか？";
            }
                if (Input.GetMouseButtonDown(0))
            {
                // TryCatch文でNull回避
                try
                {
                    // 子供のコンポーネントにアクセスしたいのでいったん変数に格納
                    selectedObj = eventSystem.currentSelectedGameObject.gameObject;
                    // 選択したオブジェクトがボタンだった時、ボタンの情報を取得
                    if(selectedObj.tag == "Button")
                    {
                        selectedItem = selectedObj.GetComponent<ItemListButtonController>();
                        SelectPanel.SetActive(true);

                        if (MessageBox.GetComponent<TextMeshProUGUI>().text != selectedItem.itemText + Environment.NewLine + selectText)
                        {
                            if (prevTextFlag)
                            {
                                prevTextFlag = false;
                                prevText = MessageBox.GetComponent<TextMeshProUGUI>().text;
                            }
                            MessageBox.GetComponent<TextMeshProUGUI>().text = selectedItem.itemText + Environment.NewLine + selectText;
                        }
                    }
                    else
                    {
                        if (prevText.Length > 0)
                        {
                            MessageBox.GetComponent<TextMeshProUGUI>().text = prevText;
                            prevText = "";
                            prevTextFlag = true;
                            if (selectedObj.name != "YesButton")
                            {
                                SelectPanel.SetActive(false);
                            }
                        }
                    }
                }
                // 例外処理的なやつ
                catch (NullReferenceException ex) 
                {
                }
                    
            }
        }
        //アイテムを使用・購入する
        public void ExecuteItem()
        {


            if (selectState == SelectState.Yes && itemSelectState != ItemSelectState.Unset)
            {
                // 使用フェーズの場合
                if (itemSelectState == ItemSelectState.UseItem)
                {
                    Debug.Log(selectedItem.itemName + "を使用しました。");
                    if (selectedItem.itemType == ItemType.DICE)
                    {
                        useItemId = selectedItem.itemId;
                        CloseItemPanel();
                        RollDiceButtonClicked();
                    }
                    //　ワープを使用
                    else if (selectedItem.itemType == ItemType.WARP)
                    {
                        useItemId = selectedItem.itemId;
                        CloseItemPanel();
                        MenuPanel.SetActive(false);

                        warpFlag = true;
                        MessageBox.GetComponent<TextMeshProUGUI>().text = "コアのちょくぜんまでワープします。";
                    }

                    currentPlayer.GetComponent<PlayerController>().UseItem(useItemId, 1);
                    StartCoroutine("setPlayerItemList");
                }
                // 購入フェーズの場合
                else if (itemSelectState == ItemSelectState.PurchaseItem_phase2)
                {
                    ItemListPanel.SetActive(false);
                    string purchaseId = selectedItem.itemId;
                    string purchaseName = selectedItem.itemName;
                    int purchasePrice = selectedItem.itemPrice;
                    Debug.Log(purchaseId + "購入");

                    ItemGet(purchaseId, purchaseName, purchasePrice);


                    
                    //アイテム購入フェーズ終了
                    itemSelectState = ItemSelectState.Unset;
                    //イベント処理後、ターン終了フラグをtrueにする
                    currentPlayer.GetComponent<PlayerController>().turnEndFlag = true;
                }


                // ステータスをリセット
                selectState = SelectState.ReadyForSelect;
                itemSelectState = ItemSelectState.Unset;

                // セットされたアイテムをリセット
                selectedItem = null;

                SelectPanel.SetActive(false);
            }
        }
        public void ItemGetEventStart()
        {
            //アイテム購入フェーズに移行
            itemSelectState = ItemSelectState.PurchaseItem;

            MessagePanel.SetActive(true);
            MessageBox.GetComponent<TextMeshProUGUI>().text = "アイテムショップにようこそ！" + Environment.NewLine + "アイテムをかいますか？";
            //はい、いいえを選択するパネルを表示
            SelectPanel.SetActive(true);
        }
        // 購入フェーズにはいったら下のメソッドをUpdateの中で実行
        public void ItemGetEvent()
        {
            if(selectState != SelectState.ReadyForSelect)
            {
                SelectPanel.SetActive(false);

                //「はい」が押された場合、購入処理を実行
                if (selectState == SelectState.Yes)
                {
                    ItemListPanel.SetActive(true);
                    StartCoroutine("setItemShopList");
                    itemSelectState = ItemSelectState.PurchaseItem_phase2;
                }
                //「いいえ」が押された場合、購入しない
                else if (selectState == SelectState.No)
                {
                    //アイテム購入フェーズ終了
                    itemSelectState = ItemSelectState.Unset;
                    //イベント処理後、ターン終了フラグをtrueにする
                    currentPlayer.GetComponent<PlayerController>().turnEndFlag = true;
                }
            }
             
            //選択が終わったら、ステータスを選択前のものに戻しておく
            selectState = SelectState.ReadyForSelect;
        }
        //アイテムを取得
        public void CountItem(string itemId, int count)
        {
            //List内を検索
            for (int i = 0; i < itemShopDataList.Count; i++)
            {
                //IDが一致していたらカウント
                if (itemShopDataList[i].id == itemId)
                {
                    //アイテムをカウント
                    itemShopDataList[i].CountUp(count);
                    return;
                }
            }

            //IDが一致しなければアイテムを追加
            ItemData itemData = new ItemData(itemId, count);
            itemShopDataList.Add(itemData);
        }
        public void ItemGet(string itemId, string itemName, int itemPrice)
        {
            currentPlayer.GetComponent<PlayerController>().coin -= itemPrice;
            coinTextList[currentId].GetComponent<TextMeshProUGUI>().text
                       = "coin:" + currentPlayer.GetComponent<TeamC.PlayerController>().coin.ToString();
            currentPlayer.GetComponent<PlayerController>().CountItem(itemId, 1);
            endMessageList.Add("コインを" + itemPrice + "まいしはらって" + Environment.NewLine + "アイテム「" + itemName + "」をゲットしました。");
            StartCoroutine("setPlayerItemList");

        }
        void Update()
        {
            //最初のターンのみスタート位置まで歩く
            for (int i = 0; i < playerList.Count; i++)
            {
                GameObject startPlayer = playerList[i];
                bool startFlag = startPlayer.GetComponent<PlayerController>().startFlag;
                if (i == currentId && leftTurnCount == turnCount && startFlag)
                {
                    currentPlayer.GetComponent<TeamC.PlayerController>().MoveToStartPos(currentPlayer, startPos);
                }

            }

            // ダイスの目を取得
            if (dice01 != null)
            {
                diceRollingFlag = dice01.GetComponent<DiceRollingLogic>().rollingFlag;
                if(diceRollingFlag == "Stop")
                {
                    if(useItemId.Length > 0)
                    {
                        getNumOfDiceUsingItem(useItemId);
                        useItemId = "";
                    }
                    else
                    {
                        getNumOfDice01();
                    }
                    dice01.GetComponent<DiceRollingLogic>().rollingFlag = "Destroy";
                    if (dice02)
                    {
                        dice02.GetComponent<DiceRollingLogic>().rollingFlag = "Destroy";
                    }
                }
            }

            if (numOfDice01 != 0 && dice01 == null)
            {
                currentPlayer.GetComponent<TeamC.PlayerController>().PlayerMove(numOfDice01);
                Debug.Log("移動ロジックへ渡した出目(numOfDice01):" + numOfDice01);

                numOfDice01 = 0;

                //ダイスの操作ガイドを非表示
                GuidePanel.SetActive(false);

                // プレイヤーを移動状態に変更
                currentPlayer.GetComponent<PlayerController>().playerState = PlayerController.PlayerState.MovePhase;

            }

            // 移動を終えたらプレイヤーをMovePhaseからEndPhaseに切り替え
            if(currentPlayer &&
               currentPlayer.GetComponent<PlayerController>().playerState == PlayerController.PlayerState.MovePhase &&
               currentPlayer.GetComponent<PlayerController>().moveCount == 0 && currentPlayer.GetComponent<PlayerController>().stopFlag)
            {
                // プレイヤーを後処理状態に変更
                currentPlayer.GetComponent<PlayerController>().playerState = PlayerController.PlayerState.EndPhase;

            }
            // アイテムショップ
            if(itemSelectState == ItemSelectState.PurchaseItem)
            {
                ItemGetEvent();
            }

            // 選択したアイテムを実行
            ExecuteItem();
            // アイテムを選択したか判定
            SelectItem();

            //プレイヤーの行動が終わった場合の処理
            if (currentPlayer != null &&
                currentPlayer.GetComponent<TeamC.PlayerController>().turnEndFlag == true)
            {

                //行動終了フラグをfalseにし、次の行動終了に備える
                currentPlayer.GetComponent<TeamC.PlayerController>().turnEndFlag = false;

                //プレイヤーの所持コイン数が増減した場合、UIへ反映させる処理
                // 5/13長崎追加
                if (currentPlayer.GetComponent<TeamC.PlayerController>().updateOfCoins != 0)
                {
                    coinNum = currentPlayer.GetComponent<TeamC.PlayerController>().updateOfCoins;
                    Debug.Log("コイン数UI更新対象プレイヤーのcurrentID" + currentId);

                    // ●●●●●●↓勝山追記　5/14　●●●●●●

                    //■■■■■■↓長崎編集 5/21 ここから■■■■■■
                    // コインを5回生成して消える //コインの増減枚数を引数として渡すように変更 5/21 長崎
                    StartCoroutine(coinDisplay(coinNum));
                    //■■■■■■↑長崎編集 5/21 ここから■■■■■■

                    // ●●●●●●↑勝山追記　5/14　●●●●●●

                    // メッセージを追加
                    if (coinNum > 0)
                    {
                        endMessageList.Add("コインを" + coinNum + "まいゲットしました。");
                    }
                    else
                    {
                        //長崎編集 5/18
                        endMessageList.Add("コインを" + Math.Abs(coinNum) + "まいうしないました。");
                    }

                    //コイン表示UIの対応箇所へ、更新された所持コイン数を反映 長崎編集 5/18
                    coinTextList[currentId].GetComponent<TextMeshProUGUI>().text
                        = "coin:" + currentPlayer.GetComponent<TeamC.PlayerController>().coin.ToString();

                    //参照している変数updateOfCoinsを0にする
                    currentPlayer.GetComponent<TeamC.PlayerController>().updateOfCoins = 0;
                }

                // コア購入していた場合、緑マスを移動
                if (coreGetFlag)
                {
                    coreGetFlag = false;
                    GreenPanelChange();
                    endMessageList.Add("コアをゲットしたので、コアのおかれたマスがいどうしました。");
                }


                //各種UIを再表示する
                MessagePanel.SetActive(true);

                endMessageList.Add("ターンをしゅうりょうします。");

                MessageBox.GetComponent<TextMeshProUGUI>().text = endMessageList[0];
                endMessageList.RemoveAt(0);
            }
            
            //2ターン目以降、(待機フェーズのとき)マスの外に待機
            for (int i = 0; i < playerList.Count; i++)
            {
                GameObject player = playerList[i];
                bool isStandby = player.GetComponent<PlayerController>().playerState == PlayerController.PlayerState.StandbyPhase;
                bool startFlag = player.GetComponent<PlayerController>().startFlag;
                bool panelStayFinishFlag = player.GetComponent<PlayerController>().panelStayFinishFlag;
                if (isStandby && !startFlag && !panelStayFinishFlag)
                {
                    player.GetComponent<PlayerController>().StayOutside();
                }

            }

            //各種UIの更新
            if (currentPlayer != null) 
            {
                //残移動数の更新
                moveCountText.GetComponent<TextMeshProUGUI>().text =
                    currentPlayer.GetComponent<TeamC.PlayerController>().moveCount.ToString();

                //プレイヤーの残移動数が0より大きければ、UIを表示
                if (currentPlayer.GetComponent<TeamC.PlayerController>().moveCount > 0)
                {
                    //表示をアクティブにする
                    moveCountText.SetActive(true);

                }
                else
                {
                    //表示を非アクティブにする
                    moveCountText.SetActive(false);
                }

            }

            //ダイスのSE再生処理
            if(dice01 !=  null)
            {
                //ダイスが回っている場合、SEをループ再生
                if (dice01.GetComponent<DiceRollingLogic>().rollingFlag == "Rolling" 
                    || dice01.GetComponent<DiceRollingLogic>().rollingFlag == "ReadyForStop")
                {
                    diceRollingSound.Play();
                }

                //ダイスが回っていない場合は、音を止める
                else
                {
                    diceRollingSound.Stop();
                }

                //ダイスが消えたら
                if (dice01.GetComponent<DiceRollingLogic>().rollingFlag == "Destory")
                {
                    //ダイスが消えたときの音を再生
                    diceStopSound.Play();
                } 
            }
        }

        public void UpdateUIOfCoins(int id,int coins)
        {
            coinTextList[id].GetComponent<TextMeshProUGUI>().text
                = "coins:" + coins.ToString();
        }

        //DiceRollingLogicから出目を受け取るメソッド
        void getNumOfDice01()
        {
            //出目の数を受け取る
            numOfDice01 = dice01.GetComponent<TeamC.DiceRollingLogic>().numOfDice;

            //ダイスが止まった際の音を再生
            diceStopSound.Play();
            Debug.Log("directorがダイスから受け取った出目(numOfDice01):" + numOfDice01);
        }
        void getNumOfDiceUsingItem(string itemId)
        {
            //出目の数を受け取る
            numOfDice01 = dice01.GetComponent<TeamC.DiceRollingLogic>().GetNumOfDice(itemId);

            //ダイスが止まった際の音を再生
            diceStopSound.Play();
            Debug.Log("directorがダイスから受け取った出目(アイテム使用):" + numOfDice01);
        }

        // ●●●●●●↓勝山追記　5/14作成　●●●●●●
        // コインを5枚生成して消す処理
        private IEnumerator coinDisplay(int coinNum)
        {
            // 1秒後(パネルを踏んでいるであろう)ポジションでコインを表示させる
            yield return new WaitForSeconds(0.7f);
            Vector3 currentPlayerPos = currentPlayer.transform.position;
            coinUpDownTextObject.transform.position = new Vector3(currentPlayerPos.x, currentPlayerPos.y + 3f, currentPlayerPos.z);

            // 現在のカメラを取得
            currentCamera = cameraCheck.GetComponent<CameraCheck>()._currentCamera;
            coinUpDownTextObject.transform.LookAt(currentCamera.transform.position);

            //■■■■■■↓長崎編集 5/22 ここから■■■■■■
            if (coinNum > 0)
            {
                coinUpDownText.text = "+" + coinNum.ToString();
                coinUpDownText.color = Color.blue;
            }
            else if (coinNum < 0)
            {
                coinUpDownText.text = coinNum.ToString();
                //■■■■■■↓長崎編集 5/22 ここまで■■■■■■
                coinUpDownText.color = Color.red;
            }


            coinUpDownTextObject.SetActive(true);

            if (currentCamera == playerFarCamera)
            {
                coinUpDownTextObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else if (currentCamera == playerNearCamera)
            {
                coinUpDownTextObject.transform.Rotate(0.0f, -180.0f, 0.0f);
            }

            for (int i = 0; i < 5; i++)
            {
                coins[i] = Instantiate(coinsPrefab);
                coins[i].transform.position = new Vector3(currentPlayerPos.x, 3f, currentPlayerPos.z);
                Destroy(coins[i], displayTime);
                yield return new WaitForSeconds(waitTime);
            }
            coinUpDownTextObject.SetActive(false);
        }
        // ●●●●●●↑勝山追記　5/14作成　●●●●●●


        //■■■■■■↓長崎追記 5/21 ここから■■■■■■
        //コアを購入するイベント
        public IEnumerator BuyCoreEvent()
        {
            //コアの値段設定
            int priceOfCore = 50;

            Debug.Log("Greenマスのコア購入イベント発生");

            //操作フライング防止のため、選択ステータスを初期化しておく
            selectState = SelectState.ReadyForSelect;

            //イベント開始前のメッセージ内容を保存し退避しておく
            String tempMessage = MessageBox.GetComponent<TextMeshProUGUI>().text;

            //メッセージパネルをアクティブにする
            MessagePanel.SetActive(true);

            //コアを購入できるだけの所持コインがあれば、MessageBoxに購入選択メッセージを表示
            if (currentPlayer.GetComponent<TeamC.PlayerController>().coin >= priceOfCore)
            {
                //コアを買うかどうかのメッセージを表示
                MessageBox.GetComponent<TextMeshProUGUI>().text = "コイン" + priceOfCore.ToString() + 
                    "まいでコアをかえます。\nコアをかいますか?";

                //操作受付のフライング防止のため、待機
                yield return new WaitForSeconds(1.5f);

                //はい、いいえを選択するパネルを表示
                SelectPanel.SetActive(true);

                //はいかいいえが押されるまで待機する処理
                yield return new WaitUntil(() => selectState != SelectState.ReadyForSelect);

                //はい、いいえを選択するパネルを非表示
                SelectPanel.SetActive(false);

                //「はい」が押された場合、購入処理を実行
                if (selectState == SelectState.Yes)
                {
                    //所持金からコア代を引く
                    currentPlayer.GetComponent<TeamC.PlayerController>().coin -= priceOfCore;
                    //所持コア数を1つ増やす
                    currentPlayer.GetComponent<TeamC.PlayerController>().core += 1;

                    //プレイヤーの所持金、コア数表示を更新
                    coinTextList[currentId].GetComponent<TextMeshProUGUI>().text =
                        "coin:" + currentPlayer.GetComponent<TeamC.PlayerController>().coin.ToString();
                    coreTextList[currentId].GetComponent<TextMeshProUGUI > ().text =
                        "core:" + currentPlayer.GetComponent<TeamC.PlayerController>().core.ToString();

                    MessageBox.GetComponent<TextMeshProUGUI>().text = "コアをゲットしました!";

                    //5/23 長崎追記 コアのアニメーション関連

                    //プレイヤーの現在位置を座標で取得
                    float x = currentPlayer.transform.position.x;
                    float y = currentPlayer.transform.position.y;
                    float z = currentPlayer.transform.position.z;
                    currentPlayerPos = new Vector3(x, y, z);

                    //コアのオブジェクトを生成
                    GameObject core = Instantiate(coreObject);
                    Vector3 corePos = new Vector3(x, (y + 2), z);

                    //コアをプレイヤーの頭上に表示
                    core.transform.position = corePos;

                    //コアゲット時のSEを再生
                    getCoreSound.Play();

                    //数秒待機
                    yield return new WaitForSeconds(3.0f);

                    //コアオブジェクトを削除
                    GameObject.Destroy(core);

                    coreGetFlag = true;
                }
                //「いいえ」が押された場合、購入しない
                else if (selectState == SelectState.No)
                {
                    MessageBox.GetComponent<TextMeshProUGUI>().text = "コアをかうのをキャンセルしました。";
                }

                //選択が終わったら、ステータスを選択前のものに戻しておく
                selectState = SelectState.ReadyForSelect;
            }
            //コアを購入するための所持コインが足りなかったら、購入選択処理は実行しない
            else if (currentPlayer.GetComponent<TeamC.PlayerController>().coin < priceOfCore)
            {
                MessageBox.GetComponent<TextMeshProUGUI>().text = "コアをかうコインがたりません...";
            }

            //イベント終了直前のメッセージを読んでもらって、時間経過でメッセージを戻す
            yield return new WaitForSeconds(2.0f);

            //イベント開始前のメッセージに戻す
            MessageBox.GetComponent<TextMeshProUGUI>().text = tempMessage;

            //コア購入イベント終了
            currentPlayer.GetComponent<PlayerController>().coreFinishFlag = true;

            //残移動数が0であれば、ターン終了メッセージに変更
            if (currentPlayer.GetComponent<PlayerController>().tempMoveCount == 0)
            {
                //イベント処理後、ターン終了フラグをtrueにする
                currentPlayer.GetComponent<PlayerController>().turnEndFlag = true;
            }
            else
            {
                //再度イベントフラグをリセット
                currentPlayer.GetComponent<PlayerController>().eventFlag = false;

            }
        }

        //緑マスを移動させる
        public void GreenPanelChange()
        {
            //現在のマスをもとの色に戻す
            greenPanelList[greenPanelIndex].GetComponent<PanelController>().panelState = greenPanelPrevState;
            greenPanelList[greenPanelIndex].GetComponent<PanelController>().panelColorSet();

            //リストのインデックスを1進める
            greenPanelIndex = greenPanelIndex < greenPanelList.Count - 1 ? greenPanelIndex+1 : 0;
            // 次の緑マスの現時点での色を格納
            greenPanelPrevState = greenPanelList[greenPanelIndex].GetComponent<PanelController>().panelState;
            //次の緑マスを緑マスにする
            greenPanelList[greenPanelIndex].GetComponent<PanelController>().panelState = PanelController.PanelState.Green;
            greenPanelList[greenPanelIndex].GetComponent<PanelController>().panelColorSet();
        }

        //「はい」ボタンが押された場合に動くメソッド
        public void YesButtonClicked()
        {
            selectState = SelectState.Yes;
        }

        //「いいえ」ボタンが押された場合に動くメソッド
        public void NoButtonClicked()
        {
            selectState = SelectState.No;
        }

        //リプレイボタンが押されたときに動くメソッド
        //public void ReplayButtonClicked()
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //    ResultPanel.SetActive(false);
        //}
        //■■■■■■↑長崎追記 5/21 ここまで■■■■■■

    }
}