using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Note
{
	public int LPB;// 一拍を何等分するか
	public int num;// LPBの累計総数
	public int block;// レーン

	public float timeStamp;
	public GameObject gameObject;
}

[System.Serializable]
public class Song
{
	public string name;
	public int maxBlock;
	public int BPM;
	public int offset;
	public Note[] notes;
}

public struct Notes
{
	public Note[] lane1Notes;
	public Note[] lane2Notes;
}

public enum FAV_RATE{
	ZERO,
	BAD,
	GOOD,
	GREAT,
	PERFECT
}

public class GameManager : MonoBehaviour
{
	/* スコアの判定
	 * 曲をかける
	 */
	[SerializeField]
	GameObject startPanel;
	[SerializeField]
	string songName;
	// 曲を流すオーディオソース
	[SerializeField]
	private AudioSource audioSourse;
	[SerializeField]
	AudioClip se;
	[SerializeField]
	TextMeshProUGUI timeText;

	[SerializeField]
	TextMeshProUGUI tutawaridoText;
	[SerializeField]
	Tutawarido tutawarido;
	float tutawaridoPoint;
	float maxTutawaridoPoint;

	[SerializeField]
	SpeechBaloon speechBaloon;

	

	FAV_RATE favRate;

	Notes notes;


	private void Awake()
	{

	}
	private void Start()
	{
		PlaySong("パステルハウス");
	}

	private void Update()
	{
		// 曲の再生時間のデバッグ
		timeText.text = "曲の再生時間:" + Mathf.RoundToInt(audioSourse.time).ToString();
		if (Input.GetKeyDown(KeyCode.Q))
		{
			float tapTime = audioSourse.time;
			ScoreJudge(tapTime,notes.lane1Notes);
			audioSourse.PlayOneShot(se);
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			float tapTime = audioSourse.time;
			ScoreJudge(tapTime, notes.lane2Notes);
			audioSourse.PlayOneShot(se);
		}
		// 曲が終わったら
		if (!audioSourse.isPlaying)
		{
			ChangeScene();
		}
	}



	[SerializeField]
	GameObject notesParent;
	[SerializeField]
	GameObject notePref;
	[SerializeField]
	GameObject lane1, lane2;
	[SerializeField]
	float offset;

	public void OnGameStart()
	{
		startPanel.SetActive(false);
		notes = GenerateNotes(songName);
		PlaySong(songName);
		tutawaridoPoint = 0;
		maxTutawaridoPoint = 100;
		favRate = FAV_RATE.ZERO;
	}
	// 曲のロード
	Song loadSong(string songName)
	{
		string inputString = Resources.Load<TextAsset>("SongJson/" + songName).ToString();
		Song song = JsonUtility.FromJson<Song>(inputString);
		return song;
	}
	//ノーツの生成
	public Notes GenerateNotes(string songName)
	{

		Song song = loadSong(songName);

		float diffTime;//ノーツをおける最小時間幅
		diffTime = 60f / song.notes[0].LPB / song.BPM;
		Debug.Log(diffTime);
		float lane1Y = lane1.gameObject.transform.position.y;
		float lane2Y = lane2.gameObject.transform.position.y;

		int lane1Num = 0; int lane2Num = 0;
		for (int i = 0; i < song.notes.Length; i++)
		{
			// レーン１
			if (song.notes[i].block == 1)
			{
				lane1Num++;
			}// レーン2
			if (song.notes[i].block == 3)
			{
				lane2Num++;
			}

		}
		// 時間を収納した配列 判定に必要
		Note[] lane1Notes = new Note[lane1Num];
		Note[] lane2Notes = new Note[lane2Num];
		int k = 0; int l = 0;
		for (int i = 0; i < song.notes.Length; i++)
		{
			// レーン１
			if (song.notes[i].block == 1)
			{
				Note note = new Note();
				note.timeStamp = diffTime * song.notes[i].num;
				GameObject noteObj = Instantiate(notePref, new Vector2(Config.instance.speed * diffTime * song.notes[i].num - offset, lane1Y), Quaternion.identity);
				noteObj.transform.parent = notesParent.transform;
				note.gameObject = noteObj;
				lane1Notes[k] = note;
				k++;
			}
			// レーン2
			if (song.notes[i].block == 3)
			{
				Note note = new Note();
				note.timeStamp = diffTime * song.notes[i].num;
				GameObject noteObj = Instantiate(notePref, new Vector2(Config.instance.speed * diffTime * song.notes[i].num - offset, lane2Y), Quaternion.identity);
				noteObj.transform.parent = notesParent.transform;
				note.gameObject = noteObj;
				lane2Notes[l] = note;
				l++;
			}
		}
		Notes notes = new Notes();
		notes.lane1Notes = lane1Notes;
		notes.lane2Notes = lane2Notes;

		return notes;
	}

	void PlaySong(string songName)
    {
        string songPath = "Song/" + songName;
        AudioClip audioClip = Resources.Load(songPath) as AudioClip;
        audioSourse.clip = audioClip;
        audioSourse.Play();
    }
	
	void ScoreJudge(float tapTime,Note[] notes)
	{
		int nearestNoteNum = 0;
		float minTime = 100;

		for (int i = 0; i < notes.Length; i++)
		{
			// 音楽再生時間とノーツの時間を比べて一番近いものを見つける
			if (Mathf.Abs(tapTime - notes[i].timeStamp) < minTime)
			{
				minTime = Mathf.Abs(tapTime - notes[i].timeStamp);
				nearestNoteNum = i;
			}
		}
		// スコアの処理とゲームオブジェクトを消す
		// Perfect
		if (minTime < 0.1f)
		{
			Debug.Log("taptime"+tapTime+ notes[nearestNoteNum].timeStamp+ "Perfect");
			Debug.Log(nearestNoteNum);
			tutawaridoPoint += 0.35f;
			notes[nearestNoteNum].gameObject.SetActive(false);
			notes[nearestNoteNum].gameObject.transform.position = new Vector3(0, 0, 0);
			speechBaloon.BoyPerfectText();
			
		}else if (minTime <= 0.2f)
		{
			tutawaridoPoint += 0.25f;
			notes[nearestNoteNum].gameObject.SetActive(false);
			speechBaloon.BoyGreatText();

		}
		else if(minTime <= 0.3f)
		{
			tutawaridoPoint += 0.2f;
			notes[nearestNoteNum].gameObject.SetActive(false);
			speechBaloon.BoyGoodText();
		}
		if (tutawaridoPoint <= maxTutawaridoPoint)
		{
			tutawaridoText.text = tutawaridoPoint.ToString("F1") + "%";
		}
		tutawarido.TutawaridoUp(tutawaridoPoint, maxTutawaridoPoint);

		// 伝わり度によってステータスを変化させる
		float tutawaridoPercent = tutawaridoPoint / maxTutawaridoPoint;
		if(tutawaridoPercent != 0 && tutawaridoPercent < 0.4f)
		{
			if (favRate == FAV_RATE.ZERO)
			{
				speechBaloon.GirlBadText();
   			}
			favRate = FAV_RATE.BAD;
			
		}
		if (tutawaridoPercent >= 0.2f && tutawaridoPercent < 0.4f)
		{
			if(favRate == FAV_RATE.BAD)
			{
				speechBaloon.GirlGoodText();
			}
			favRate = FAV_RATE.GOOD;
		}else if (tutawaridoPercent >= 0.6f && tutawaridoPercent < 0.8f)
		{
			if (favRate == FAV_RATE.GOOD)
			{
				speechBaloon.GirlGreatText();
			}
			favRate = FAV_RATE.GREAT;
		}
		else if (tutawaridoPercent >= 0.8f)
		{
			if (favRate == FAV_RATE.GREAT)
			{
				speechBaloon.GirlPerfectText();
			}
			favRate = FAV_RATE.PERFECT;
		}

	}

	void ChangeScene()
	{
		if(favRate == FAV_RATE.ZERO)
		{
			SceneManager.LoadScene("End0");

		}else if(favRate == FAV_RATE.BAD)
		{
			SceneManager.LoadScene("End1");
		}else if(favRate == FAV_RATE.GOOD)
		{
			SceneManager.LoadScene("End2");
		}else if(favRate == FAV_RATE.GREAT)
		{
			SceneManager.LoadScene("End3");

		}
		else if (favRate == FAV_RATE.PERFECT)
		{
			SceneManager.LoadScene("End4");

		}
	}
}

