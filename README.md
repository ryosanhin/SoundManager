# SoundManager
Simple Sound Manager for Unity  
Unity向けの簡易音声管理オブジェクトです。
# 使い方
1. Prefabフォルダ内に入っているSimpleSoundManagerをシーンに配置します。
![selectPrefab](https://github.com/ryosanhin/SoundManager/assets/90621212/e05d04ca-7efd-4f85-89e0-be446fee7a6d)
2. BGMFile、SEFileに鳴らしたいオーディオクリップを登録します。
![DD_AudioSource_to_BGMFile](https://github.com/ryosanhin/SoundManager/assets/90621212/d36a58c5-0d28-4177-bf71-fb6a9f6eb18a)
3. スクリプト内で呼び出します。
   スクリプトは以下の項目を参照してください。
## スクリプト
public void PlayBGMClip(int clipNum=0);  
public void PlayBGMClip(string clipName);
```
//BGMFileでの0番目のオーディオクリップに変更
PlayBGMClip(0);

//BGMFileで「hogehoge」という名前のオーディオクリップに変更
PlayBGMClip("hogehoge");
```

public void FadeInAndOutBGM(int clipNum,int type=0,float time=1.5f);  
public void FadeInAndOutBGM(string clipName,int type=0,float time=1.5f);
```
//フェードアウトとフェードアウトをしながら音声を変更
//type=0 フェードアウトとフェードイン両方、type=1 フェードアウトのみ、BGMの変更なし、type=2 フェードインのみ
//timeに指定された時間をかけてフェードアウトとフェードインをする

//BGMFileでの0番目のオーディオクリップに変更
FadeInAndOutBGM(1);

//BGMFileで「hogehoge」という名前のオーディオクリップに変更
FadeInAndOutBGM("hogehoge");
```
public void PlaySEClip(int num);  
public void PlaySEClip(string s);  
public void PlaySEClip(AudioClip clip);  
```
//SEFileの0番目のSEを再生
PlaySEClip(0);

//SEFileの「hogehoge」という名前のSEを再生
PlaySEClip("hogehoge");

//直接AudioClipを渡して再生も可
PlaySEClip(someAudioClip);
```
