using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    #region singleton
    private static BGMManager _instance;

    public static BGMManager instance{
		get{return _instance;}
	}
	
    private void Awake()
    {
        if (_instance!=null && _instance!=this){
            Destroy(this.gameObject);
        }else{
            _instance=this;
			DontDestroyOnLoad(gameObject);
        }
    }
	#endregion
	
	[SerializeField] AudioSource[] audioSourceList;
	[SerializeField] AudioClips clips;
	Coroutine fade;
	List<string> bgmNameList=new List<string>();
	
	void Start(){
		foreach(AudioClip ac in clips.clip){
			bgmNameList.Add(ac.name);
		}
	}
	
	//音量変更 num->音量0-1 listNum->オーディオソースを複数使った場合にどのソースを変更するか
	public void ChangeVolume(float num,int listNum=0){
		if(listNum>=audioSourceList.Length){
			return;
		}
		
		audioSourceList[listNum].volume=num;
	}
	
	//再生停止 listNum->オーディオソースを複数使った場合にどのソースを変更するか
	public void StopBGM(int listNum=0){
		if(listNum>=audioSourceList.Length){
			return;
		}
		audioSourceList[listNum].Stop();
	}
	
	//音声変更 clipNum->オーディオクリップのリストでの番号 listNum->オーディオソースを複数使った場合にどのソースを変更するか
	public void PlayBGMClip(int clipNum,int listNum=0){
		if(listNum>=audioSourceList.Length || clipNum>=clips.clip.Length){
			return;
		}
		
		audioSourceList[listNum].Stop();
		audioSourceList[listNum].clip=clips.clip[clipNum];
		audioSourceList[listNum].Play();
	}
	
	//音声変更 clipName->オーディオクリップのリストでの名前 listNum->オーディオソースを複数使った場合にどのソースを変更するか
	public void PlayBGMClip(string clipName,int listNum=0){
		if(listNum>=audioSourceList.Length){
			return;
		}
		
		int clipNum=BGMSearch(clipName);
		if(clipNum==-1){
			return;
		}
		
		audioSourceList[listNum].Stop();
		audioSourceList[listNum].clip=clips.clip[clipNum];
		audioSourceList[listNum].Play();
	}
	
	//フェードアウトフェードイン clipNum->オーディオクリップのリストでの番号
	//type->どのような処理をするか 0:フェードアウトフェードイン両方 1:フェードアウトだけ(bgmは変更しない) 2:フェードインだけ
	//time->フェードアウトフェードインにかかる時間 listNum->オーディオソースを複数使った場合にどのソースを変更するか
	public void FadeInAndOutBGM(int clipNum,int type=0,float time=1.5f,int listNum=0){
		if(listNum>=audioSourceList.Length || clipNum>=clips.clip.Length){
			return;
		}
		
		if(fade!=null){
			return;
		}
		
		switch(type){
			case 0:
			fade=StartCoroutine(FadeInAndOut(clipNum,time,listNum));
			break;
			
			case 1:
			fade=StartCoroutine(FadeOutBGM(time,listNum));
			break;
			
			case 2:
			fade=StartCoroutine(FadeInBGM(clipNum,time,listNum));
			break;
		}
	}
	
	//フェードアウトフェードイン clipName->オーディオクリップのリストでの名前 type->どのような処理をするか 0:フェードアウトフェードイン両方 1:フェードアウトだけ(bgmは変更しない) 2:フェードインだけ
	//time->フェードアウトフェードインにかかる時間 listNum->オーディオソースを複数使った場合にどのソースを変更するか
	public void FadeInAndOutBGM(string clipName,int type=0,float time=1.5f,int listNum=0){
		if(listNum>=audioSourceList.Length){
			return;
		}
		
		if(fade!=null){
			return;
		}
		
		int clipNum=BGMSearch(clipName);
		if(clipNum==-1){
			return;
		}
		
		
		switch(type){
			case 0:
			fade=StartCoroutine(FadeInAndOut(clipNum,time,listNum));
			break;
			
			case 1:
			fade=StartCoroutine(FadeOutBGM(time,listNum));
			break;
			
			case 2:
			fade=StartCoroutine(FadeInBGM(clipNum,time,listNum));
			break;
		}
	}
	
	//フェードアウトフェードイン clipNum->オーディオクリップのリストでの番号 time->フェードアウトフェードインにかかる時間 listNum->オーディオソースを複数使った場合にどのソースを変更するか
	IEnumerator FadeInAndOut(int clipNum,float time,int listNum){
		
		yield return fade=StartCoroutine(FadeOutBGM(time,listNum));
		
		yield return fade=StartCoroutine(FadeInBGM(clipNum,time,listNum));
	}
	
	//フェードアウト clipNum->オーディオクリップのリストでの番号 time->フェードアウトにかかる時間 listNum->オーディオソースを複数使った場合にどのソースを変更するか
	IEnumerator FadeOutBGM(float time,int listNum){
		float t=time;
		float inverse=1f/time;
		
		while(t>0f){
			t-=Time.deltaTime;
			audioSourceList[listNum].volume=t*inverse;
			yield return null;
		}
		
		audioSourceList[listNum].volume=0f;
		audioSourceList[listNum].Stop();
		fade=null;
	}
	
	//フェードイン clipNum->オーディオクリップのリストでの番号 time->フェードインにかかる時間 listNum->オーディオソースを複数使った場合にどのソースを変更するか
	IEnumerator FadeInBGM(int clipNum,float time,int listNum){
		float t=0f;
		float inverse=1f/time;
		
		audioSourceList[listNum].volume=0f;
		audioSourceList[listNum].clip=clips.clip[clipNum];
		audioSourceList[listNum].Play();
		while(t<time){
			t+=Time.deltaTime;
			audioSourceList[listNum].volume=t*inverse;
			yield return null;
		}
		audioSourceList[listNum].volume=1f;
		fade=null;
	}
	
	//名前リストからリスト番号に変換(全部検索してごり押し変換)
	int BGMSearch(string s){
		int i=0;
		foreach(string _s in bgmNameList){
			if(s.Equals(_s)){
				return i;
			}
			i++;
		}
		return -1;
	}
}
