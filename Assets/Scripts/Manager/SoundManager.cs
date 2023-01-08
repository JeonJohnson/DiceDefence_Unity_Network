//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Audio;
//using Enums;
//using UnityEngine.SceneManagement;

//public class SoundManager : Manager<SoundManager>
//{

//    [HideInInspector]
//    public AudioClip[] bgmClips;
//    [HideInInspector]
//    public Dictionary<string, AudioClip> ambientClipDic;

//    public Dictionary<string, AudioClip> effectClipDic;

//    //public Dictionary<string, AudioClip> weaponClipDic;
//    //public List<AudioClip>[][] weaponClips;

//    public List<Dictionary<string, AudioClip>> weaponClipList;

//    //무기들은 그냥 각자 사운드 들고 있을까...?;
//    public GameObject audioSourceBox;
//    public AudioSource bgmAus;
//    public AudioSource ambientAus;
//    public AudioSource weaponAus;

//    public GameObject tempAusParent;
//    public Queue<AudioSource> tempAusQueue;

//	#region BgmManager
//	public void PlayBgm(eScenes sceneNum)
//    {
        
//        bgmAus.PlayOneShot(bgmClips[(int)sceneNum]);
//    }

//    public void StopBgm()
//    {
//        bgmAus.Stop();
//    }
//	#endregion

//	#region AmbientManager
//	public void PlayAmbient(string name)
//    {
//        ambientAus.PlayOneShot(ambientClipDic[name]);
//    }

//    public void StopAmbient()
//    {
//        ambientAus.Stop();
//    }
//    #endregion


//    public void PlayWeaponSound(AudioClip clip)
//    {
//        weaponAus.PlayOneShot(clip);

//    }

//    public void PlayWeaponSound(eWeaponName name, string actionName)
//    {
//        if (weaponClipList.Count < (int)name)
//        {
//            goto IsNull;
//        }

//        if (weaponClipList[(int)name][actionName] == null)
//        {
//            goto IsNull;
//        }

//        AudioClip clip = weaponClipList[(int)name][actionName];

//        if (clip == null)
//        {
//            goto IsNull;
//        }

//        weaponAus.PlayOneShot(clip);
//        return;

//    IsNull:
//        Debug.LogWarning(Funcs.GetEnumName<eWeaponName>((int)name) + " : " + actionName + "\nhas not Exist");
//        return;
//    }

    

//    public void PlaySound(GameObject singer, AudioClip sound)
//    {
//        AudioSource aus = singer.transform.root.GetComponent<AudioSource>();

//        if (aus == null)
//        {
//            PlaySound(singer.transform.position, sound);
//            //aus = singer.AddComponent<AudioSource>();
//        }

//        aus.PlayOneShot(sound);
//    }

//    public void PlaySound(Vector3 pos, AudioClip sound)
//    {

//        AudioSource temp = tempAusQueue.Dequeue();

//        if (!temp.isPlaying)
//        {
//            temp.transform.position = pos;
//            temp.PlayOneShot(sound);
//            tempAusQueue.Enqueue(temp);
//        }
//        else 
//        {
//            GameObject newObj = new GameObject();
//            newObj.name = "tempAus_" + tempAusQueue.Count.ToString();
//            newObj.transform.parent = tempAusParent.transform;
//            AudioSource newAus = newObj.AddComponent<AudioSource>();

//            newAus.spatialBlend = 1f;
//            newAus.transform.position = pos;
//            newAus.PlayOneShot(sound);
//            tempAusQueue.Enqueue(newAus);
//        }


//        //for (int i = 0; i < tempAusList.Count; ++i)
//        //{
//        //    if (!tempAusList[i].isPlaying)
//        //    {
//        //        tempAusList[i].transform.position = pos;
//        //        tempAusList[i].PlayOneShot(sound);

//        //        return;
//        //    }
//        //    else
//        //    {
//        //        continue;
//        //    }
//        //}

//        //AudioSource temp = Funcs.CheckComponentExist<AudioSource>("tempAus_" + tempAusList.Count.ToString());
//        //temp.spatialBlend = 1f;
//        ////temp.rolloffMode = AudioRolloffMode.Linear;
//        //temp.transform.position = pos;
//        //temp.PlayOneShot(sound);
//        //tempAusList.Add(temp);
//        ////AudioSource.PlayClipAtPoint(sound, pos);
//    }

//	#region AllSoundManager
//	//public void AllSoundPause()
// //   {
// //       bgmAus.Pause();
// //       ambientAus.Pause();
// //       weaponAus.Pause();

// //       for (int i = 0; i < tempAusList.Count; ++i)
// //       {
// //           if (tempAusList[i].isPlaying)
// //           { tempAusList[i].Pause(); }
// //       }
// //   }

//    public void AllSoundPlay()
//    {
//        bgmAus.Play();
//        ambientAus.Play();
//        weaponAus.Play();

//        foreach (AudioSource aus in tempAusQueue)
//        {
//            aus.Play();
//        }

//        //for (int i = 0; i < tempAusQueue.Count; ++i)
//        //{
//        //    tempAusQueue[i].Play();
//        //}

//    }

//    public void AllSoundStop()
//    {
//        bgmAus.Stop();
//        ambientAus.Stop();


//        foreach (AudioSource aus in tempAusQueue)
//        {
//            if (aus.isPlaying)
//            {
//                aus.Stop();
//            }
//        }

//        //for (int i = 0; i < tempAusList.Count; ++i)
//        //{
//        //    if (tempAusList[i].isPlaying)
//        //    { tempAusList[i].Stop(); }
//        //}
//    }

//    public void AllSoundSpd(float spd)
//    {
//        bgmAus.pitch = spd;
//        ambientAus.pitch = spd;
//        weaponAus.pitch = spd;

//        foreach (AudioSource aus in tempAusQueue)
//        {
//            if (aus.isPlaying)
//            {
//                aus.pitch = spd;
//            }
//        }

//        //for (int i = 0; i < tempAusList.Count; ++i)
//        //{
//        //    if (tempAusList[i].isPlaying)
//        //    { tempAusList[i].pitch = spd; }
//        //}
//    }
//    #endregion



    

//	private void TempAudioSourceInitialize(int count)
//    {
//        //tempAusParent = GameObject.Find("TempAudioSources");
    

//        tempAusParent = Funcs.CheckGameObjectExist("TempAudioSources");
//        //DontDestroyOnLoad(tempAusParent);

//        tempAusQueue = new Queue<AudioSource>();
//        for (int i = 0; i < count; ++i)
//        {
//            GameObject tempObj = new GameObject();
//            tempObj.name = "tempAus_" + i.ToString();
//            tempObj.transform.parent = tempAusParent.transform;    

//            AudioSource aus = tempObj.AddComponent<AudioSource>();
//            aus.spatialBlend = 1f;

//            tempAusQueue.Enqueue(aus);
//        }

//        //DontDestroyOnLoad(tempAusParent);
//    }


//    #region AudioClipLoad

    

//    public void FindAudioClip(string path, string name, ref Dictionary<string,AudioClip> dic)
//    {
//        AudioClip ac = Resources.Load<AudioClip>(path);

//        if (ac == null)
//        {
//            Debug.Log("Sound Manager Notice!\n" + path + " file has not exist");
//            return;
//        }

//        dic.Add(name, ac);
//    }

//    public void BgmAudioClipsLoad()
//    {
//        bgmClips[(int)eScenes.Title] = Funcs.FindResourceFile<AudioClip>("Audio/BGM/KF1_MainTheme");
//        bgmClips[(int)eScenes.InGame] = Funcs.FindResourceFile<AudioClip>("Audio/BGM/InGameBGM");
//    }

//    public void AmbAudioClipsLoad()
//    {
//        FindAudioClip("Audio/Ambient/Country_Ambience", "Country_Ambience", ref ambientClipDic);
//        FindAudioClip("Audio/Ambient/FlyBuzz", "FlyBuzz", ref ambientClipDic);
//        FindAudioClip("Audio/Ambient/Owl", "Owl", ref ambientClipDic);
//    }



//    public void WeaponAudioClipsLoad()
//    {
//        #region Primary
//        { //FN FAL
//            Dictionary<string, AudioClip> dic = weaponClipList[(int)eWeaponName.FN_FAL];

//            FindAudioClip("Audio/Weapon/FN FAL/FNFAL_Select", "Select", ref dic);

//            FindAudioClip("Audio/Weapon/FN FAL/FNFAL_Fire", "Fire", ref dic);

//            FindAudioClip("Audio/Weapon/FN FAL/FNFAL_DryFire", "Dry_Fire", ref dic);

//            FindAudioClip("Audio/Weapon/FN FAL/Reload/FNFAL_Mag_Eject", "Reload_0", ref dic);
//            FindAudioClip("Audio/Weapon/FN FAL/Reload/FNFAL_Mag_Insert", "Reload_1", ref dic);
//            FindAudioClip("Audio/Weapon/FN FAL/Reload/FNFAL_Bolt_Back", "Reload_2", ref dic);
//            FindAudioClip("Audio/Weapon/FN FAL/Reload/FNFAL_Bolt_Forward", "Reload_3", ref dic);
//        }

//        {//Winchester
//            Dictionary<string, AudioClip> dic = weaponClipList[(int)eWeaponName.Winchester_M1894];

//            FindAudioClip("Audio/Weapon/Winchester/Winchester_Select", "Select", ref dic);

//            FindAudioClip("Audio/Weapon/Winchester/Fire/Winchester_Fire_1", "Fire_1", ref dic);
//            FindAudioClip("Audio/Weapon/Winchester/Fire/Winchester_Fire_2", "Fire_2", ref dic);
//            FindAudioClip("Audio/Weapon/Winchester/Fire/Winchester_Fire_3", "Fire_3", ref dic);
//            FindAudioClip("Audio/Weapon/Winchester/Fire/Winchester_Fire_0", "Fire_0", ref dic);

//            FindAudioClip("Audio/Weapon/Winchester/Fire/Winchester_DryFire", "DryFire", ref dic);

//            ////BulletInsert Sounds////
//            //Winchester_Reload_014
//            FindAudioClip("Audio/Weapon/Winchester/Reload/Winchester_Reload_014", "BulletInsert_0", ref dic);
//            FindAudioClip("Audio/Weapon/Winchester/Reload/Winchester_Reload_037", "BulletInsert_1", ref dic);
//            FindAudioClip("Audio/Weapon/Winchester/Reload/Winchester_Reload_060", "BulletInsert_2", ref dic);
//            FindAudioClip("Audio/Weapon/Winchester/Reload/Winchester_Reload_083", "BulletInsert_3", ref dic);
//            FindAudioClip("Audio/Weapon/Winchester/Reload/Winchester_Reload_107", "BulletInsert_4", ref dic);
//            FindAudioClip("Audio/Weapon/Winchester/Reload/Winchester_Reload_130", "BulletInsert_5", ref dic);
//            FindAudioClip("Audio/Weapon/Winchester/Reload/Winchester_Reload_151", "BulletInsert_6", ref dic);
//            FindAudioClip("Audio/Weapon/Winchester/Reload/Winchester_Reload_176", "BulletInsert_7", ref dic);
//            FindAudioClip("Audio/Weapon/Winchester/Reload/Winchester_Reload_199", "BulletInsert_8", ref dic);
//            FindAudioClip("Audio/Weapon/Winchester/Reload/Winchester_Reload_224", "BulletInsert_9", ref dic);
//            ////BulletInsert Sounds////


//            ////LeverAction Sounds////
//            FindAudioClip("Audio/Weapon/Winchester/LeverAction/Winchester_Reload1", "Lever_1", ref dic);
//            FindAudioClip("Audio/Weapon/Winchester/LeverAction/Winchester_Reload1_2", "Lever_1_2", ref dic);
            
//            FindAudioClip("Audio/Weapon/Winchester/LeverAction/Winchester_Reload2", "Lever_2", ref dic);
//            FindAudioClip("Audio/Weapon/Winchester/LeverAction/Winchester_Reload2_2", "Lever_2_2", ref dic);
            
//            FindAudioClip("Audio/Weapon/Winchester/LeverAction/Winchester_Reload3", "Lever_3", ref dic);
//            FindAudioClip("Audio/Weapon/Winchester/LeverAction/Winchester_Reload3_2", "Lever_3_2", ref dic);

//            FindAudioClip("Audio/Weapon/Winchester/LeverAction/Winchester_Reload4", "Lever_4", ref dic);
//            FindAudioClip("Audio/Weapon/Winchester/LeverAction/Winchester_Reload4_2", "Lever_4_2", ref dic);

//            ////LeverAction Sounds////

//        }
//        #endregion

//        #region Secondary
//        {//Beretta
//            Dictionary<string, AudioClip> dic = weaponClipList[(int)eWeaponName.Beretta_92FS];

//            //FindAudioClip("Audio/Weapon/Beretta/Beretta_Select", "Beretta 92FS_Select", ref dic);

//            //FindAudioClip("Audio/Weapon/Beretta/Fire/Beretta_Fire1", "Beretta 92FS_Fire1", ref dic);
//            //FindAudioClip("Audio/Weapon/Beretta/Fire/Beretta_Fire2", "Beretta 92FS_Fire2", ref dic);
//            //FindAudioClip("Audio/Weapon/Beretta/Fire/Beretta_Fire3", "Beretta 92FS_Fire3", ref dic);
//            //FindAudioClip("Audio/Weapon/Beretta/Fire/Beretta_Fire4", "Beretta 92FS_Fire4", ref dic);

//            //FindAudioClip("Audio/Weapon/Beretta/Fire/Beretta_DryFire", "Beretta 92FS_DryFire", ref dic);

//            //FindAudioClip("Audio/Weapon/Beretta/Reload/Beretta_Reload_000", "Beretta 92FS_Reload_0", ref dic);
//            //FindAudioClip("Audio/Weapon/Beretta/Reload/Beretta_Reload_026", "Beretta 92FS_Reload_1", ref dic);
//            //FindAudioClip("Audio/Weapon/Beretta/Reload/Beretta_Reload_036", "Beretta 92FS_Reload_2", ref dic);
//            //FindAudioClip("Audio/Weapon/Beretta/Reload/Beretta_Reload_049", "Beretta 92FS_Reload_3", ref dic);

//            FindAudioClip("Audio/Weapon/Beretta/Beretta_Select", "Select", ref dic);

//            FindAudioClip("Audio/Weapon/Beretta/Fire/Beretta_Fire_0", "Fire_0", ref dic);
//            FindAudioClip("Audio/Weapon/Beretta/Fire/Beretta_Fire_1", "Fire_1", ref dic);
//            FindAudioClip("Audio/Weapon/Beretta/Fire/Beretta_Fire_2", "Fire_2", ref dic);
//            FindAudioClip("Audio/Weapon/Beretta/Fire/Beretta_Fire_3", "Fire_3", ref dic);

//            FindAudioClip("Audio/Weapon/Beretta/Fire/Beretta_DryFire", "DryFire", ref dic);

//            FindAudioClip("Audio/Weapon/Beretta/Reload/Beretta_Reload_000", "Reload_0", ref dic);
//            FindAudioClip("Audio/Weapon/Beretta/Reload/Beretta_Reload_026", "Reload_1", ref dic);
//            FindAudioClip("Audio/Weapon/Beretta/Reload/Beretta_Reload_036", "Reload_2", ref dic);
//            FindAudioClip("Audio/Weapon/Beretta/Reload/Beretta_Reload_049", "Reload_3", ref dic);
//        }
//        #endregion

//        #region Melee

//        {//M9 knife
//            Dictionary<string, AudioClip> dic = weaponClipList[(int)eWeaponName.M9_Knife];

//            //FindAudioClip("Audio/Weapon/M9 Knife/Knife_Fire1", "M9 Knife_Fire1", ref dic);
//            //FindAudioClip("Audio/Weapon/M9 Knife/Knife_Fire2", "M9 Knife_Fire2", ref dic);
//            //FindAudioClip("Audio/Weapon/M9 Knife/Knife_Fire3", "M9 Knife_Fire3", ref dic);
//            //FindAudioClip("Audio/Weapon/M9 Knife/Knife_Fire4", "M9 Knife_Fire4", ref dic);

//            FindAudioClip("Audio/Weapon/M9 Knife/Select/Knife_Select_0", "Select_0", ref dic);
//            FindAudioClip("Audio/Weapon/M9 Knife/Select/Knife_Select_1", "Select_1", ref dic);
//            FindAudioClip("Audio/Weapon/M9 Knife/Select/Knife_Select_2", "Select_2", ref dic);
//            FindAudioClip("Audio/Weapon/M9 Knife/Select/Knife_Select_3", "Select_3", ref dic);


//            FindAudioClip("Audio/Weapon/M9 Knife/Knife_Fire_0", "Fire_0", ref dic);
//            FindAudioClip("Audio/Weapon/M9 Knife/Knife_Fire_1", "Fire_1", ref dic);
//            FindAudioClip("Audio/Weapon/M9 Knife/Knife_Fire_2", "Fire_2", ref dic);
//            FindAudioClip("Audio/Weapon/M9 Knife/Knife_Fire_3", "Fire_3", ref dic);

//        }
//        #endregion

//        #region Special

//        { //Barrett M99
//            Dictionary<string, AudioClip> dic = weaponClipList[(int)eWeaponName.Barrett_M99];

//            FindAudioClip("Audio/Weapon/M99/M99_Select", "Select", ref dic);

//            FindAudioClip("Audio/Weapon/M99/M99_Fire", "Fire", ref dic);

//            for (int i = 0; i < 6; ++i)
//            {
//                string name = "M99_";
//                string audioStr = "Reload_" + i.ToString();
//                FindAudioClip("Audio/Weapon/M99/Reload/"+name+audioStr, audioStr, ref dic);
//            }
            

//        }

//        #endregion

      

     
        

//        //FindAudioClip("Audio/Weapon/M99/M99_Fire", "M99_Fire", ref weaponClipDic);
        
//        //FindAudioClip("Audio/Weapon/Winchester/Winchester_Fire1", "Winchester_Fire1", ref weaponClipDic);
//        //FindAudioClip("Audio/Weapon/Winchester/Winchester_Fire2", "Winchester_Fire2", ref weaponClipDic);
//        //FindAudioClip("Audio/Weapon/Winchester/Winchester_Fire3", "Winchester_Fire3", ref weaponClipDic);
//        //FindAudioClip("Audio/Weapon/Winchester/Winchester_Fire4", "Winchester_Fire4", ref weaponClipDic);


//    }
//	#endregion




//    private void AudioSourceInitialize()
//    {
//        audioSourceBox = Funcs.CheckGameObjectExist("AuidoSourceBox");
//        DontDestroyOnLoad(audioSourceBox);

//        if (bgmAus == null)
//        {
//            bgmAus = Funcs.CheckComponentExist<AudioSource>("BgmAus");
//            bgmAus.loop = true;
//            bgmAus.transform.SetParent(audioSourceBox.transform);
//        }
//        //DontDestroyOnLoad(bgmAus.gameObject);

//        if (ambientAus == null)
//        {
//            ambientAus = Funcs.CheckComponentExist<AudioSource>("AmbientAus");
//            ambientAus.transform.SetParent(audioSourceBox.transform);
//        }
//        //DontDestroyOnLoad(ambientAus.gameObject);

//        if (weaponAus == null)
//        {
//            weaponAus = Funcs.CheckComponentExist<AudioSource>("WeaponAus");
//            weaponAus.volume = 0.35f;
//            weaponAus.transform.SetParent(audioSourceBox.transform);
//            //weaponAus.spatialBlend = 1f;
//            //총기 사운드는 어차피 계속 풀로 나오면 됨. 플레이어한테서 나오는 소리니까
//        }
//        //DontDestroyOnLoad(weaponAus.gameObject);

//        TempAudioSourceInitialize(50);
//        tempAusParent.transform.SetParent(audioSourceBox.transform);
//    }

//    private void AudioClipsInitialize()
//    {
//        bgmClips = new AudioClip[(int)eScenes.End];
//        ambientClipDic = new Dictionary<string, AudioClip>();
//        effectClipDic = new Dictionary<string, AudioClip>();

//        weaponClipList = new List<Dictionary<string, AudioClip>>();
//        for (int i = 0; i < (int)eWeaponName.End; ++i)
//        {
//            Dictionary<string, AudioClip> tempDic = new Dictionary<string, AudioClip>();
//            weaponClipList.Add(tempDic);
//        }
//    }

//    public void AuidoClipsLoad()
//    {
//        BgmAudioClipsLoad();
//        AmbAudioClipsLoad();

//        WeaponAudioClipsLoad();


//        FindAudioClip("Audio/Explosion/Explode01", "Grenade_Explosion", ref effectClipDic);
//    }

//    private void Awake()
//	{
//        DontDestroyOnLoad(this.gameObject);

//        AudioSourceInitialize();
        
//        AudioClipsInitialize();

//        AuidoClipsLoad();
//    }

//	void Start()
//    {
        
//    }

//    void Update()
//    {
        
//    }

//    public override void OnEnable()
//    {
//        base.OnEnable();

//    }

//    public override void OnDisable()
//    {
//        base.OnDisable();
//    }

//    public override void OnSceneChanged(Scene scene, LoadSceneMode mode)
//    {
//        //PlayBgm()
           
//    }


//}
