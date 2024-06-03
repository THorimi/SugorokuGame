using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamC
{
    public class PlayerGenerator : MonoBehaviour
    {
        private int playerNum; //プレイヤー数
        public List<GameObject> playerList = new List<GameObject>(); //プレイヤーを格納するリスト

        //■■■■■■↓長崎編集■■■■■■
        //プレファブ1体を変数宣言していましたが、後述の変数宣言に変更しました
        //[SerializeField] public GameObject playerPrefab;

        // 5/9 長崎追記プレイヤープレファブを格納するリスト
        //playerGeneratorのインスペクターへプレファブをアウトレット接続してください
        [SerializeField] public List<GameObject> playerPrefabList;
        //■■■■■■↑長崎編集ここまで■■■■■■

        public List<GameObject> PlayerGenerate(int playerNum)
        {
            for (int i = 0; i < playerNum; i++)
            {
                //■■■■■■↓長崎編集 5/9■■■■■■
                //GameObject player = Instantiate(playerPrefab);
                GameObject player = Instantiate(playerPrefabList[i]);

                //■■■■■■↑長崎編集ここまで■■■■■■

                player.transform.position = new Vector3(1.85f - 1.4f * i, 1f, -8.4f);

                //■■■■■■↓長崎編集 5/9■■■■■■
                //プレイヤー名を入力してもらう機能が実装できるまで仮置きの処理
                player.GetComponent<TeamC.PlayerController>().playerName = "player" + (i+1).ToString(); 
                //■■■■■■↑長崎編集ここまで■■■■■■

                player.GetComponent<TeamC.PlayerController>().coin = 100;
                playerList.Add(player);
            }
            return playerList;
        }

        void Update()
        {

        }
    }


}

