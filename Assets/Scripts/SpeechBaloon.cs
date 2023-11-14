using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SpeechBaloon : MonoBehaviour
{
    [SerializeField]
    Text girlSpeechBaloonText;
    [SerializeField]
    Image faceImage;
    [SerializeField]
    Sprite[] GirlfaceSprites;
    [SerializeField]
    GameObject boySpeechBaloon;
    [SerializeField]
    TextMeshProUGUI boySpeechBaloonText;

    // perfectキスト内容
    string[] perfectWord = { "世界一愛してる", "愛してる", "君しか考えられない" };
    // greatテキスト内容
    string[] greatWord = { "好きだ！", "あの日会った時から", };
    // goodテキスト内容
    string[] goodWord = { "す…" };

    int randomNumber;

    public void BoyGoodText()
	{
        randomNumber = Random.Range(0, goodWord.Length);
        boySpeechBaloonText.text = goodWord[randomNumber];
        // 吹き出しの大きさのエフェクトをつける
        boySpeechBaloon.transform.DOScale(new Vector3(0, 0, 0), 0.1f);
        boySpeechBaloon.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.2f);
    }
    public void BoyGreatText()
    {
        randomNumber = Random.Range(0, greatWord.Length);
        boySpeechBaloonText.text = greatWord[randomNumber];
        boySpeechBaloon.transform.DOScale(new Vector3(0, 0, 0), 0.1f);
        boySpeechBaloon.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.2f);


    }
    public void BoyPerfectText()
    {
        randomNumber = Random.Range(0, perfectWord.Length);
        boySpeechBaloonText.text = perfectWord[randomNumber];
        boySpeechBaloon.transform.DOScale(new Vector3(0, 0, 0), 0.1f);
        boySpeechBaloon.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.2f);

    }
    // 2わりから4わり
    public void GirlBadText()
    {
        faceImage.sprite = GirlfaceSprites[0];
        girlSpeechBaloonText.text = "";
        girlSpeechBaloonText.DOText("何言ってんのよ！！",1);
    }
    // 4わり以上6わり未満
    public void GirlGoodText()
    {
        faceImage.sprite = GirlfaceSprites[1];
        girlSpeechBaloonText.text = "";
        girlSpeechBaloonText.DOText("私は好きじゃないわ！", 1);
    }
    // 6わり以上8わり未満
    public void GirlGreatText()
    {
        faceImage.sprite = GirlfaceSprites[2];
        girlSpeechBaloonText.text = "";
        girlSpeechBaloonText.DOText("ほ、ほんとに言ってんの！？",1);
    }
    // 8わり以上
    public void GirlPerfectText()
    {
        faceImage.sprite = GirlfaceSprites[3];
        girlSpeechBaloonText.text = "";
        girlSpeechBaloonText.DOText("................", 1);
    }

}
