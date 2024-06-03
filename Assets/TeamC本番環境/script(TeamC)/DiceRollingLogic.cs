using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//4/26 長崎作成
//ダイスが回るアニメーションと、出目を乱数生成するスクリプト
//現状ではダイスにアタッチして動く仕様

namespace TeamC
{
    public class DiceRollingLogic : MonoBehaviour
    {
        // デバッグ用
        private KeyCode[] _key = new KeyCode[]
        {
            KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2,
            KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
            KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8,
            KeyCode.Alpha9
        };

        //回転速度に関係する変数
        private int rotateX;
        private int rotateY;
        private int rotateZ;

        //サイコロの出目(Directorスクリプトと連携させる)
        public int numOfDice;

        //サイコロのステータスを表す文字列変数
        public string rollingFlag = "ReadyForStart";

        //後続のコードでコルーチンを使用するために、startメソッドもコルーチンに組み込む
        private IEnumerator Start()
        {
            numOfDice = Random.Range(1, 7);


            yield return null; // Startの実行が完了するまで1フレーム待って後続の処理を待機
        }
        // アイテム使用時を含めた出目を返すメソッド
        public int GetNumOfDice(string itemId)
        {
            numOfDice = Random.Range(1, 7);
            
            if (itemId == "dice_0") //偶数ダイス
            {
                numOfDice = Random.Range(1, 4) * 2;
                Debug.Log("偶数ダイス");
            }
            else if (itemId == "dice_1") //奇数ダイス
            {
                numOfDice = Random.Range(1, 4) * 2 - 1;
                Debug.Log("奇数ダイス");
            }
            else if (itemId == "dice_2") //*2数ダイス
            {
                numOfDice = Random.Range(1, 7) + Random.Range(1, 7);
                Debug.Log("×２ダイス");
            }

            return numOfDice;
        }

        private void Update()
        {
            //サイコロを振る前の状態で、かつEnterキーを押した場合に、ダイスのステータスを"Rolling"にし、
            //1秒後に、ダイスを止められるようにする処理を実行
            if (rollingFlag == "ReadyForStart" && 
                (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)))
            {
                rollingFlag = "Rolling";

                //コルーチン開始
                StartCoroutine(RollDice());
            }

            //1秒経っていれば、Enterキーが押された際に、ダイスのステータスを"Stop"にする(つまりアニメーションを止める)
            if (rollingFlag == "ReadyForStop" && (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)))
            {
                rollingFlag = "Stop";
            }

            if (Input.anyKeyDown)
            {
                Debug.Log("keydown");
                // デバッグ用　テンキーで出目操作
                for (int i = 0; i < _key.Length; i++)
                {
                    if (Input.GetKeyDown(_key[i]))
                    {
                        Debug.Log(_key[i]);
                        numOfDice = i;
                        rollingFlag = "Stop";
                    }
                }
            }

            //ダイスを回すアニメーションの処理
            if (rollingFlag == "Rolling" || rollingFlag == "ReadyForStop")
            {
                rotateX = Random.Range(0, 360);
                rotateY = Random.Range(0, 360);
                rotateZ = Random.Range(0, 360);
                transform.Rotate(rotateX, rotateY, rotateZ);
            }

            //ダイスのステータスがStopになった場合、サイコロごと消すことで、このスクリプトの処理も終了させる
            // 堀見追記　Director側で目を取得してからサイコロを削除
            if (rollingFlag == "Destroy")
            {
                //サイコロオブジェクトが消えることでnullになり、Director側で出目の受け渡し処理が発生
                Destroy(this.gameObject);
            }

            //検証用デバッグログ
            //Debug.Log("rollingFlag:" + rollingFlag);
        }

        //0.5秒待機してからrollingFlagを"ReadyForStop"に変更するコルーチンメソッド
        //ダイスのアニメーション開始と停止が同時に動かないようにするための処理
        private IEnumerator RollDice()
        {
            yield return new WaitForSeconds(0.5f); // 0.5秒待機
            rollingFlag = "ReadyForStop";
        }
    }
}