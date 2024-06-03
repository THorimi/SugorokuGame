using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//4/26 長崎作成
//ダイスが回るアニメーションと、出目を乱数生成するスクリプト
//現状ではダイスにアタッチして動く仕様

namespace ebihara

{

    public class DiceRollingLogic : MonoBehaviour
    {

        //回転速度に関係する変数
        private int rotateX;
        private int rotateY;
        private int rotateZ;

        //サイコロのステータスを表す文字列変数
        private string rollingFlag = "Notstart";

        //サイコロの出目
        public int numOfDice;

        void Start()
        {
            //サイコロの出目をstartメソッドで決定しておく
            numOfDice = Random.Range(1, 7);
        }

        // Update is called once per frame
        void Update()
        {
            //サイコロを振り終わる前で、かつ、Enterキーを押している間だけ、ダイスのステータスを"Rolling"にする
            if ((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return))
                && (rollingFlag != "Stop"))
            {

                rollingFlag = "Rolling";
            }

            //Enterキーを押しておらず、ダイスが回っていると、ダイスのステータスを"Stop"にする
            else if (rollingFlag == "Rolling")
            {
                rollingFlag = "Stop";
            }

            //フラグが"Rolling"の間、サイコロの回転を継続させる
            if (rollingFlag == "Rolling")
            {
                //回転速度は乱数生成・UpDateで更新される
                //もっと自然なロールの仕方になる設定があれば、調整予定
                rotateX = Random.Range(0, 360);
                rotateY = Random.Range(0, 360);
                rotateZ = Random.Range(0, 360);
                transform.Rotate(rotateX, rotateY, rotateZ);
            }

            //ダイスのステータスが"Stop"なら、つまりサイコロを振り終わった直後
            if (rollingFlag == "Stop")
            {
                //サイコロを消す処理
                Destroy(this.gameObject);
            }
        }
    }
}