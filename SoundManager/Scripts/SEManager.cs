using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    #region singleton
    private static SEManager _instance;

    public static SEManager instance{
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
	
	AudioSource audioSource;
	[SerializeField] AudioClips clips;
	List<string> seNameList=new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        audioSource=GetComponent<AudioSource>();
		foreach(AudioClip ac in clips.clip){
			seNameList.Add(ac.name);
		}
    }
	
	//音声再生 s->オーディオクリップのリストでの名前
	public void PlaySEClip(string s){
		int num=SESearch(s);
		if(num>=clips.clip.Length || num==-1){
			return;
		}
		audioSource.PlayOneShot(clips.clip[num]);
	}
	
	//音声再生 num->オーディオクリップのリストでの番号
	public void PlaySEClip(int num){
		if(num>=clips.clip.Length){
			return;
		}
		audioSource.PlayOneShot(clips.clip[num]);
	}
	
	//音声再生 clip->オーディオクリップを直接渡される場合
	public void PlaySEClip(AudioClip clip){
		audioSource.PlayOneShot(clip);
	}
	
	//名前リストからリスト番号に変換(全部検索してごり押し変換)
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
