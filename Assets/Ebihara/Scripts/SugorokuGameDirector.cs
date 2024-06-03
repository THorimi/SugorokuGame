using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//4/30 長崎作成
namespace ebihara { 

//ターン進行を制御するスクリプト
public class SugorokuGameDirector : MonoBehaviour
{
    //各種UIと接続するための変数宣言
    public GameObject TurnCountText;  //仮のターンカウントUI    
    public GameObject MenuPanel; //コマンド選択のパネル
    public GameObject SelectPanel; //はい、いいえの選択パネル
    public GameObject RollDiceButton; //ダイスロールの実行ボタン
    public GameObject GuidePanel; //ダイスロール操作ガイドのUI
    public GameObject MessagePanel; //メッセージパネルのUI

    public GameObject moveCountText; //移動中の、残り移動数表示UI
                                  
    //キャラオブジェクト変数
    public GameObject player01;
    Vector3 player01Pos;

    //ダイスオブジェクトのプレファブ(type mismatchの問題のため、一旦保留)
    public GameObject dicePrefab;

    //ダイスオブジェクト変数
    GameObject dice01;

    //ダイスオブジェクトの座標
    Vector3 dice01Pos;

    //仮置きターン数:5
    private int testTurnCount = 5;

    //ダイスの出目を保存する変数
    private int numOfDice01;


    void Start()
    {
    
     //UIの準備
    TurnCountText.GetComponent<TextMeshProUGUI>().text = "testTurnCount:" + testTurnCount.ToString();

    //UIを非表示にする
    SelectPanel.SetActive(false);
    GuidePanel.SetActive(false);
    }

    //サイコロを回すボタンがクリックされたら実行
    public void RollDiceButtonClicked()
    {
        //ダイスのガイドを表示
        GuidePanel.SetActive(true);

        //プレイヤーの座標を取得
        float x = player01.transform.position.x;
        float y = player01.transform.position.y;
        float z = player01.transform.position.z;
        player01Pos = new Vector3(x, y, z);

        //生成する予定のダイスの座標を設定しておく(プレイヤーの頭上)
        dice01Pos = new Vector3(x, y + 3, z);

        //プレファブからダイスオブジェクトを生成
        dice01 = Instantiate(dicePrefab);

        if (dice01 != null)
        {
            //ダイスオブジェクトから出目の変数を受け取るメソッドを呼び出す
            //出目が生成されるstartメソッドを考慮して、時間差で取得
            Invoke("getNumOfDice01", 0.1f);
        }

        //先ほど設定した座標へサイコロを移動
        dice01.transform.position = dice01Pos;

        //各種UIを非表示にする
        MenuPanel.SetActive(false);
        MessagePanel.SetActive(false);

    }
        //----------------------------------------------------------------------------------------
        void Update()
        {
 
            //サイコロを振って出目が出た直後の処理
            if (numOfDice01 != 0 && dice01 == null)
            {
                //移動ロジックへ出目を渡す
                player01.GetComponent<TeamC.PlayerController>().PlayerMove(numOfDice01);
                Debug.Log("移動ロジックへ渡した出目(numOfDice01):" + numOfDice01);

                //内部の出目変数を初期化
                numOfDice01 = 0;

                //ダイスのガイドを非表示
                GuidePanel.SetActive(false);
            }


            //プレイヤーの行動が終わった場合の処理(現在、プレイヤー1人の場合にしか対応していません)
            if (player01.GetComponent<TeamC.PlayerController>().turnEndFlag)
            {

                //行動終了フラグをfalseにし、次の行動終了に備える
                player01.GetComponent<TeamC.PlayerController>().turnEndFlag = false;

                //ターンカウントを一つ減らす
                testTurnCount -= 1;

                //上記をターン表示に反映
                TurnCountText.GetComponent<TextMeshProUGUI>().text = "testTurnCount:" + testTurnCount.ToString();

                //各種UIを再表示する
                MenuPanel.SetActive(true);
                MessagePanel.SetActive(true);
            }

            //残りターンが0になったら、仮のリザルト画面へ遷移
            if (testTurnCount == 0)
            {
                SceneManager.LoadScene("TestResultScene");
            }

            //残移動数UIの更新
            moveCountText.GetComponent<TextMeshProUGUI>().text =
                player01.GetComponent<TeamC.PlayerController>().moveCount.ToString();

            //プレイヤーの残移動数が0より大きければ、UIを表示
            if (player01.GetComponent<TeamC.PlayerController>().moveCount > 0)
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
//----------------------------------------------------------------------------------------
    //DiceRollingLogicから出目を受け取るメソッド
    void getNumOfDice01()
    {
        //出目の数を受け取る
        numOfDice01 = dice01.GetComponent<TeamC.DiceRollingLogic>().numOfDice;
        Debug.Log("directorがダイスから受け取った出目(numOfDice01):" + numOfDice01);
        Debug.Log("player01:" + player01);
    }
}
}