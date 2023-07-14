using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSoundManager : MonoBehaviour
{
    #region singleton
    private static SimpleSoundManager _instance;

    public static SimpleSoundManager instance{
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
	
	[SerializeField]　AudioSource bgmSource;
	[SerializeField]　AudioSource seSource;
	[SerializeField] AudioClips bgmClips;
	[SerializeField] AudioClips seClips;
	Coroutine fade;
	List<string> bgmNameList=new List<string>();
	List<string> seNameList=new List<string>();
	
	void Start(){
		foreach(AudioClip ac in bgmClips.clip){
			bgmNameList.Add(ac.name);
		}
		foreach(AudioClip ac in seClips.clip){
			seNameList.Add(ac.name);
		}
	}
	
	
	//音量変更 num->音量0-1
	public void ChangeVolume(float num=0){

		bgmSource.volume=num;
	}
	
	//再生停止
	public void StopBGM(){
		bgmSource.Stop();
	}
	
	//音声変更 clipNum->オーディオクリップのリストでの番号
	public void PlayBGMClip(int clipNum=0){
		if(clipNum>=bgmClips.clip.Length){
			return;
		}
		
		bgmSource.Stop();
		bgmSource.clip=bgmClips.clip[clipNum];
		bgmSource.Play();
	}
	
	//音声変更 clipName->オーディオクリップのリストでの名前
	public void PlayBGMClip(string clipName){
		int clipNum=BGMSearch(clipName);
		if(clipNum==-1){
			return;
		}
		
		bgmSource.Stop();
		bgmSource.clip=bgmClips.clip[clipNum];
		bgmSource.Play();
	}
	
	//フェードアウトフェードイン clipNum->オーディオクリップのリストでの番号
	//type->どのような処理をするか 0:フェードアウトフェードイン両方 1:フェードアウトだけ(bgmは変更しない) 2:フェードインだけ
	//time->フェードアウトフェードインにかかる時間
	public void FadeInAndOutBGM(int clipNum,int type=0,float time=1.5f){
		if(clipNum>=bgmClips.clip.Length){
			return;
		}
		
		if(fade!=null){
			return;
		}
		
		switch(type){
			case 0:
			fade=StartCoroutine(FadeInAndOut(clipNum,time));
			break;
			
			case 1:
			fade=StartCoroutine(FadeOutBGM(time));
			break;
			
			case 2:
			fade=StartCoroutine(FadeInBGM(clipNum,time));
			break;
		}
	}
	
	//フェードアウトフェードイン clipName->オーディオクリップのリストでの名前 type->どのような処理をするか 0:フェードアウトフェードイン両方 1:フェードアウトだけ(bgmは変更しない) 2:フェードインだけ
	//time->フェードアウトフェードインにかかる時間
	public void FadeInAndOutBGM(string clipName,int type=0,float time=1.5f){
		if(fade!=null){
			return;
		}
		
		int clipNum=BGMSearch(clipName);
		if(clipNum==-1){
			return;
		}
		
		
		switch(type){
			case 0:
			fade=StartCoroutine(FadeInAndOut(clipNum,time));
			break;
			
			case 1:
			fade=StartCoroutine(FadeOutBGM(time));
			break;
			
			case 2:
			fade=StartCoroutine(FadeInBGM(clipNum,time));
			break;
		}
	}
	
	//フェードアウトフェードイン clipNum->オーディオクリップのリストでの番号 time->フェードアウトフェードインにかかる時間
	IEnumerator FadeInAndOut(int clipNum,float time){
		
		yield return fade=StartCoroutine(FadeOutBGM(time));
		
		yield return fade=StartCoroutine(FadeInBGM(clipNum,time));
	}
	
	//フェードアウト clipNum->オーディオクリップのリストでの番号 time->フェードアウトにかかる時間
	IEnumerator FadeOutBGM(float time){
		float t=time;
		float inverse=1f/time;
		
		while(t>0f){
			t-=Time.deltaTime;
			bgmSource.volume=t*inverse;
			yield return null;
		}
		
		bgmSource.volume=0f;
		bgmSource.Stop();
		fade=null;
	}
	
	//フェードイン clipNum->オーディオクリップのリストでの番号 time->フェードインにかかる時間
	IEnumerator FadeInBGM(int clipNum,float time){
		float t=0f;
		float inverse=1f/time;
		
		bgmSource.volume=0f;
		bgmSource.clip=bgmClips.clip[clipNum];
		bgmSource.Play();
		while(t<time){
			t+=Time.deltaTime;
			bgmSource.volume=t*inverse;
			yield return null;
		}
		bgmSource.volume=1f;
		fade=null;
	}
	
	//
	//ここから上bgm
	//ここから下se
	//
	
	//音声再生 s->オーディオクリップのリストでの名前
	public void PlaySEClip(string s){
		int num=SESearch(s);
		if(num>=seClips.clip.Length || num==-1){
			return;
		}
		seSource.PlayOneShot(seClips.clip[num]);
	}
	
	//音声再生 num->オーディオクリップのリストでの番号
	public void PlaySEClip(int num){
		if(num>=seClips.clip.Length){
			return;
		}
		seSource.PlayOneShot(seClips.clip[num]);
	}
	
	//音声再生 clip->オーディオクリップを直接渡される場合
	public void PlaySEClip(AudioClip clip){
		seSource.PlayOneShot(clip);
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
	int SESearch(string s){
		int i=0;
		foreach(string _s in seNameList){
			if(s.Equals(_s)){
				return i;
			}
			i++;
		}
		return -1;
	}
}
