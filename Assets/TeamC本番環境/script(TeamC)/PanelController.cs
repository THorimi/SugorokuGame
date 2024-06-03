using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//堀見さん作成、5/2長崎編集
namespace TeamC { 

    public class PanelController : MonoBehaviour
    {
        //■■■■■■↓長崎追記 5/14■■■■■■
        //アイテム：コアの個数に関する変数
        [SerializeField] public int core_num;
        //■■■■■■↑長崎追記 5/14■■■■■■

        [SerializeField] public int coin_num;
        [SerializeField] public bool isJunction = false;
        GameObject PosListObj;
        public List<GameObject> PosList = new List<GameObject>();
        // 何人のプレイヤーが滞在しているか
        public int stayCount;
        // 現在のパネルのPathPosition（現状PathAのとき手作業で設定した場合のみ）
        public float pathPos;


        public enum PanelState
        {
            Blue, 
            Red, 
            Green,
            Yellow,
            // アイテムゲットイベント
            Yellow_2,
            //■■■■■■↓長崎追記 5/21ここから■■■■■■
            Orange,
            Purple,
            White,
            Black,
            Gray
        }
            //■■■■■■↑長崎追記 5/21ここまで■■■■■■

        //色を格納するためのマテリアルリスト
        //PanelPrefabのインスペクター内で、使いたい色マテリアルをアタッチしてください
        [SerializeField] public List<Material> panelColorList;

        //インスペクターのプルダウンで選択したカラーと紐づけるための変数宣言
        public PanelState panelState;

        void Start()
        {
            // 堀見追記 ==============================
            stayCount = 0;

            PosListObj = this.gameObject.transform.Find("PosList").gameObject;
            Transform PosListTransform = PosListObj.transform;
            foreach (Transform pos in PosListTransform)
            {
                PosList.Add(pos.gameObject);
            }
            // 堀見ここまで ==========================

            //■■■■■■↓長崎編集 5/21■■■■■■
            //マスの見た目の色を変更する
            panelColorSet();
            //■■■■■■↑長崎編集 5/21■■■■■■

            //各マスのインスペクターで選択した色に応じて、処理を分岐させる
            switch (panelState)
            {
                //マスがBlueなら所持コインが増える
                case PanelState.Blue:

                    //仮置き設定：コイン関連の変数に+5を代入
                    this.coin_num = 5;
                    break;

                //マスがRedなら所持コインが減る
                case PanelState.Red:

                    //仮置き設定：コイン関連の変数に(-5)を代入
                    this.coin_num = -5;
                    break;

                //マスがGreenなら、コアをコインで買うかどうかを選択
                case PanelState.Green:

                    //仮置き設定：コイン関連の変数に(-50)を代入
                    this.coin_num = -50;

                    //仮置き設定：コア関連の変数に1を代入
                    this.core_num = 1;

                    break;

                //■■■■■■↓長崎追記 5/21■■■■■■
                //マスがYellowなら
                case PanelState.Yellow:

                    //初回で獲得できる枚数の設定
                    //PlayerControllerから、枚数設定を減らしていく
                    this.coin_num = 60;
                    break;

                case PanelState.Yellow_2:

                    break;

                //ブラックマスの設定
                case PanelState.Black:

                    //実装する新規イベントに応じて追加予定
                    break;

                //オレンジマスの設定
                case PanelState.Orange:

                    //実装する新規イベントに応じて追加予定
                    break;

                //パープルマスの設定
                case PanelState.Purple:

                    //実装する新規イベントに応じて追加予定
                    break;

                //ホワイトマスの設定
                case PanelState.White:

                    //実装する新規イベントに応じて追加予定
                    break;

                //グレーマスの設定
                case PanelState.Gray:

                    //何も起きないマスとして使用中
                    break;
            }
        }

        void Update()
        {
            //パネルステートが黄色で、かつ獲得可能なコイン枚数設定が0以下になった場合
            if(panelState == PanelState.Yellow && this.coin_num <= 0)
            {
                //パネルステートをブラックに変更
                this.panelState = PanelState.Gray;

                //マスの見た目の色を変更する
                panelColorSet();
            }
        }

        //マスの見た目の色を変更するメソッド
        public void panelColorSet()
        {
            //マスに、設定されている色(panelState)と対応するマテリアルをアタッチする
            this.GetComponent<MeshRenderer>().material =
                panelColorList.Find(n => n.name.Contains(panelState.ToString()));
        }
        //■■■■■■↑長崎追記ここまで 5/21■■■■■■
    }

}