using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundSlider : MonoBehaviour
{
	private static float[] soundVolume={0.8f,0.8f,0.8f}; //最初は80%で
	[SerializeField] AudioMixer mixer;
	
	public enum SoundType{
		All,
		BGM,
		SE,
	}
	
	[SerializeField] SoundType type;
	
	int typeNum=0; //どのタイプが選ばれているか
	string[] typeName={"master","bgm","se"};　//ミキサーを選ぶのに名前が必要
	Slider slider;
	float volume=0f; //ボリューム一時置き場
	
	void Start(){
		typeNum=(int)type;
		
		volume=soundVolume[typeNum];
		
		slider=GetComponent<Slider>();
		slider.value=volume;
	}
	
	public void OnChangedSlider(float num=0f){
		if(num==0f){
			volume=slider.value;
		}else{
			volume-=num;
			
			if(volume>1f){
				volume=1f;
			}
			if(volume<0f){
				volume=0f;
			}
			
			slider.value=volume;
		}
		
		soundVolume[typeNum]=volume;
		mixer.SetFloat(typeName[typeNum],ClampValue(volume));
	}
	
	float ClampValue(float num){
		return -80f+100f*num;
	}
}
