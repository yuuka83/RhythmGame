using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;


class MessageInfo
{
    public string j_name;
    public string faceFileName;
    public string faceIndex;
    public string message;
    public string background;

    // コンストラクタ
    public MessageInfo(string j_name, string faceFileName, string faceIndex, string message,string background)
    {
        this.j_name = j_name;
        this.faceFileName = faceFileName;
        this.faceIndex = faceIndex;
        this.message = message;
        this.background = background;
    }
}


public class ScenarioManager : MonoBehaviour
{
    [SerializeField]
    private string scenario_filePath; // シナリオのテキストファイル
    [SerializeField]
    private string nextSceneName; // 次のシーン
    [SerializeField]
    Text showingNameText;
    [SerializeField]
    Text showingMessageText;
    [SerializeField]
    private Image faceImage;
    [SerializeField]
    private Image backgroundImage;

    // 現在表示しているメッセージのID
    int messageInfoIndex = 0;
    // 現在表示使用しているメッセージ情報
    MessageInfo showingMessageInfo;

    // 全てのMessageの情報を格納しているリスト
    MessageInfo[] messageInfoList;
    private void Start()
    {
        messageInfoList = ReadCsvFile(scenario_filePath);
        showingMessageInfo = messageInfoList[0];
        showingNameText.text = messageInfoList[0].j_name;
        showingMessageText.DOText(messageInfoList[0].message, 1);
        Debug.Log("background/" + messageInfoList[0].background);
        backgroundImage.sprite = Resources.Load<Sprite>("background/" + messageInfoList[0].background);
		//backgroundImage.sprite = Resources.Load<Sprite>("background/back_black");

    }

    // テキストを読み込み，リストに変換する
    MessageInfo[] ReadCsvFile(string filePath)
    {
        MessageInfo tmpMessageInfo;
        TextAsset textAsset;
        // テキストファイルの内容全部
        string allText;
        // テキストファイルを１行ずつに区切った配列
        string[] allTextList;
        // １行から,で区切った配列
        string[] infoText;

        // CSVファイルの読み込み
        textAsset = (TextAsset)Resources.Load(filePath);
        allText = textAsset.text;
        allTextList = allText.Split('\n');

        // 0行目は無視する
        MessageInfo[] messageInfoList = new MessageInfo[allTextList.Length];

        for (int i = 1; i < allTextList.Length-1; i++)
        {
            // 
            infoText = allTextList[i].Split(',');
            //Debug.Log("0:" + infoText[0] + "1:" + infoText[1] + "2:" + infoText[2] + "3:" + infoText[3] + "4:" + infoText[4]);
            // tmpMessageInfoにまとめる
            tmpMessageInfo = new MessageInfo(infoText[0], infoText[1], infoText[2], infoText[3], infoText[4]);
            // MessageInfoListに代入
            messageInfoList[i - 1] = tmpMessageInfo;
        }

        return messageInfoList;

    }
    public void OnNextButton()
    {
        if (messageInfoIndex < messageInfoList.Length - 3)
        {
            Debug.Log(messageInfoIndex);
            // 表示テキストを次に表示するテキストに更新する
            messageInfoIndex++;
            showingMessageInfo = messageInfoList[messageInfoIndex];

            // 名前があったら名前の更新
            if (showingMessageInfo.j_name != "")
            {
                showingNameText.text = showingMessageInfo.j_name;
            }

            // 表示する画像があれば表示する
            if (showingMessageInfo.faceFileName != "")
            {
                // 名前と表情番号から表情の更新
                Debug.Log("faceImage/" + showingMessageInfo.faceFileName + "/" + showingMessageInfo.faceIndex);
                faceImage.sprite = Resources.Load<Sprite>("faceImage/" + showingMessageInfo.faceFileName + "/" + showingMessageInfo.faceIndex);
            }
            else
            {
                Debug.Log("faceImage / transparent");
                faceImage.sprite = Resources.Load<Sprite>("faceImage/transparent");
            }
            // 表示する画像があれば表示する
            if (showingMessageInfo.background != "")
            {
                // 名前と表情番号から背景の更新
                Debug.Log("background/" + showingMessageInfo.background);
                backgroundImage.sprite = Resources.Load<Sprite>("background/" + showingMessageInfo.background);
            }
            else
            {
                backgroundImage.sprite = Resources.Load<Sprite>("backgorund/back_hiru");
            }

            // メッセージテキストを表示する
            showingMessageText.text = "";
            showingMessageText.DOText(showingMessageInfo.message, 1);
		}
		else
		{
            // シーン遷移
            SceneManager.LoadScene(nextSceneName);
		}
    }
}
