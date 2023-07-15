# SoundManager
Simple Sound Manager for Unity  
Unity向けの簡易音声管理オブジェクトです。
# 使い方
### SoundSlider
こちらではそこそこ簡単に音量の操作ができます。  
1. Prefabフォルダ内に入っているSoundSliderをシーンのキャンバスに配置します。
![selectSoundSlider](https://github.com/ryosanhin/SoundManager/assets/90621212/089e309e-6f79-4b66-9906-c19c29597bd2)

### SimleSoundManager
こちらではちょっと簡単にBGMの切り替え、SEを鳴らすということができます。  
1. Prefabフォルダ内に入っているSimpleSoundManagerをシーンに配置します。
![selectSimpleSoundManaer](https://github.com/ryosanhin/SoundManager/assets/90621212/e05d04ca-7efd-4f85-89e0-be446fee7a6d)
2. BGMFile、SEFileに鳴らしたいオーディオクリップを登録します。
![DD_AudioSource_to_BGMFile](https://github.com/ryosanhin/SoundManager/assets/90621212/d36a58c5-0d28-4177-bf71-fb6a9f6eb18a)
3. スクリプト内で呼び出します。  
   スクリプトは以下の項目を参照してください。
# スクリプト
public void PlayBGMClip(int clipNum=0);  
public void PlayBGMClip(string clipName);
```
//BGMFileでの0番目のオーディオクリップに変更
SimpleSoundManager.instance.PlayBGMClip(0);

//BGMFileで「hogehoge」という名前のオーディオクリップに変更
SimpleSoundManager.instance.PlayBGMClip("hogehoge");
```

public void StopBGM();
```
//BGMを停止
SimpleSoundManager.instance.StopBGM();
```

public void FadeInAndOutBGM(int clipNum,int type=0,float time=1.5f);  
public void FadeInAndOutBGM(string clipName,int type=0,float time=1.5f);
```
//フェードアウトとフェードアウトをしながら音声を変更
//type=0 フェードアウトとフェードイン両方、type=1 フェードアウトのみ、BGMの変更なし、type=2 フェードインのみ
//timeに指定された時間をかけてフェードアウトとフェードインをする

//BGMFileでの0番目のオーディオクリップに変更
SimpleSoundManager.instance.FadeInAndOutBGM(1);

//BGMFileで「hogehoge」という名前のオーディオクリップに変更
SimpleSoundManager.instance.FadeInAndOutBGM("hogehoge");
```
public void PlaySEClip(int num);  
public void PlaySEClip(string s);  
public void PlaySEClip(AudioClip clip);  
```
//SEFileの0番目のSEを再生
SimpleSoundManager.instance.PlaySEClip(0);

//SEFileの「hogehoge」という名前のSEを再生
SimpleSoundManager.instance.PlaySEClip("hogehoge");

//直接AudioClipを渡して再生も可
SimpleSoundManager.instance.PlaySEClip(someAudioClip);
```
### やや面倒な管理方法
BGMを複数個同時に使いたい場合はBGMとSEを別々に管理するBGMManager、SEManagerもPrefabフォルダ内に入っています。  
大体はSimpleSoundManagerと使い方が同じですが、BGMManagerだけすべての関数の引数の最後にint型のlistNumが追加されています。  
  
【例】public void FadeInAndOutBGM(int clipNum,int type=0,float time=1.5f,int listNum=0);  
  
これは複数個のオーディオソースを使用するときにどのオーディオソースを変更するかというものの指定に使います。  
規定値は0なので引数を指定しなくてもオーディオソースを一つだけしか使わないという場合には特に気にしなくて大丈夫です。  
というか、BGMのオーディオソースが一つならSimpleSoundManagerを使った方が楽です。
