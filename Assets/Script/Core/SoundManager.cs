using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

public class SoundManager : SingletonClass<SoundManager>
{
    //全局控制开关
    public bool IsPlayBGM = true;

    public bool IsPlaySE = true;

    private GameObject global;

    //控制音量
    public float _SoundVolumeBGM = 1.0f;

    public float _SoundVolumeSE = 1.0f;

    //2个观众 一个BGM 一个特效音 到时候互不影响
    private GameObject listenerBGM;

    private GameObject listenerSE;

    //映射表
    private Dictionary<uint, string> m_BankInfoDict = new Dictionary<uint, string>();

    //所有bank的List
    private List<string> m_LoadBankList = new List<string>();

    public void Init()
    {
        //初始化AkInitializer 加载AkWwiseInitializationSettings
        global = new GameObject("Wwise Global");

        GameObject root = GameObject.Find("GameProcess");

        global.transform.parent = root.transform;

        global.SetActive(false);

        AkInitializer akInitializer = global.AddComponent<AkInitializer>();

        object settingObj = Resources.Load("AkWwiseInitializationSettings");

        if (settingObj != null)
        {
            AkWwiseInitializationSettings settings = settingObj as AkWwiseInitializationSettings;

            akInitializer.InitializationSettings = settings;

            global.SetActive(true);

        }

        //做了一个0.1秒延迟
        Timer.One(.1f, InitCompoent);

    }

    private void InitCompoent()
    {
        //首先需要设置一个LoadBank的路径,否则API不知道该从哪里load bank
        //PathManager管理不同平台 iOS Android Windows
        AKRESULT result = AkSoundEngine.SetBasePath(PathManager.WwiseFilePath());

        if (result != AKRESULT.AK_Success)
        {
            Log.Error("不存在此音频路径");

            return;

        }

        //Log.Info("设置Wwise音源路径为:" + PathManager.WwiseFilePath());
        listenerBGM = new GameObject("listenerBGM");

        listenerBGM.transform.parent = global.transform;

        AkAudioListener akAudioListenerBGM = listenerBGM.AddComponent<AkAudioListener>();

        //不能Default否则会串音
        akAudioListenerBGM.SetIsDefaultListener(false);

        akAudioListenerBGM.listenerId = 1;

        listenerSE = new GameObject("listenerSE");

        listenerSE.transform.parent = global.transform;

        AkAudioListener akAudioListenerSE = listenerSE.AddComponent<AkAudioListener>();

        akAudioListenerSE.SetIsDefaultListener(false);

        akAudioListenerSE.listenerId = 2;

        //游戏设置模块需要用到
        InitSound();

        //重要的一个解析xml方法
        InitXML();

    }

    private void InitSound()
    {
        //IsPlayBGM = LocalDataSave.Instance.HasKey(LocalDataSave.Instance.Int_BGM_Open) ? (LocalDataSave.Instance.GetIntData(LocalDataSave.Instance.Int_BGM_Open) == 1) : true;
        IsPlayBGM = true;

        //float volume = LocalDataSave.Instance.HasKey(LocalDataSave.Instance.Float_BGM_Volume) ? LocalDataSave.Instance.GetFloatData(LocalDataSave.Instance.Float_BGM_Volume) : 1;
        float volume = 1;

        float val = IsPlayBGM ? volume : 0;

        SoundVolumeBGM = val;

        //IsPlayBGM = LocalDataSave.Instance.HasKey(LocalDataSave.Instance.Int_SE_Open) ? (LocalDataSave.Instance.GetIntData(LocalDataSave.Instance.Int_SE_Open) == 1) : true;
        IsPlayBGM = true;

        //volume = LocalDataSave.Instance.HasKey(LocalDataSave.Instance.Float_SE_Volume) ? LocalDataSave.Instance.GetFloatData(LocalDataSave.Instance.Float_SE_Volume) : 1;
        volume = 1;

        val = IsPlaySE ? volume : 0;

        SoundVolumeSE = val;
    }

    public float SoundVolumeBGM
    {
        set
        {
            _SoundVolumeBGM = value;

            AkSoundEngine.SetGameObjectOutputBusVolume(listenerBGM, listenerBGM, value);
        }

        get
        {
            return _SoundVolumeBGM;
        }

    }

    public float SoundVolumeSE
    {
        set
        {
            _SoundVolumeSE = value;

            AkSoundEngine.SetGameObjectOutputBusVolume(listenerSE, listenerSE, value);
        }

        get
        {
            return _SoundVolumeSE;
        }

    }

    #region 解析xml

    private void InitXML()
    {
        UnityWebRequest request = UnityWebRequest.Get(PathManager.WwiseFilePath() + "SoundbanksInfo.xml");
        //DownloadHandlerBuffer Download = new DownloadHandlerBuffer();
        //request.downloadHandler = Download;
        WebRequestManager.Request(request, (wr)=> {
            XmlDocument XmlDoc = new XmlDocument();

            //Log.Info(request.downloadHandler.text);

            XmlDoc.LoadXml(request.downloadHandler.text);

            //首先获取xml中所有的SoundBank
            XmlNodeList soundBankList = XmlDoc.GetElementsByTagName("SoundBank");

            foreach (XmlNode node in soundBankList)
            {
                XmlNode bankNameNode = node.SelectSingleNode("ShortName");

                string bankName = bankNameNode.InnerText;

                //判断SingleNode存在与否,比如Init.bak就没这个
                XmlNode eventNode = node.SelectSingleNode("IncludedEvents");

                if (eventNode != null)
                {
                    //拿到其中所有的event做一个映射
                    XmlNodeList eventList = eventNode.SelectNodes("Event");

                    foreach (XmlElement x1e in eventList)

                    {
                        m_BankInfoDict.Add(uint.Parse(x1e.Attributes["Id"].Value), bankName);
                    }
                }
            }
        });
    }

    #endregion

    public void ClearLaseBGMSoundID()
    {
        lastBGMSoundID = 0;
    }

    private uint lastBGMSoundID = 0;

    private uint bGMPlayingID = 0;

    public uint PlayBGMSound(uint soundID, AkCallbackManager.EventCallback AkCallback = null, object cookie = null)
    {
        if (listenerBGM == null)
        {
            Log.Error("BGM还未初始化");

            return 0;
        }

        LoadSound(soundID);

        if (lastBGMSoundID == soundID)
        {
            Log.Info("重复播放BGM,id==" + soundID);

            return bGMPlayingID;
        }

        lastBGMSoundID = soundID;

        bGMPlayingID = AkSoundEngine.PostEvent(soundID, listenerBGM, 1, AkCallback + AkCallbackBGM, cookie);

        Log.Info("开始播放BGM,id==" + soundID);

        return bGMPlayingID;

    }

    private void AkCallbackBGM(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
    {
        lastBGMSoundID = 0;
    }

    private uint lastSESoundID = 0;

    private uint sEPlayingID = 0;

    public uint PlaySESound(uint soundID)
    {
        if (listenerSE == null)
        {
            Log.Error("SE还未初始化");

            return 0;

        }

        LoadSound(soundID);

        if (lastSESoundID == soundID)
        {
            Log.Info("重复播放SE,id==" + soundID);

            return sEPlayingID;
        }

        lastSESoundID = soundID;

        sEPlayingID = AkSoundEngine.PostEvent(soundID, listenerSE, 1, AkCallbackSE, null);

        return sEPlayingID;

    }

    private void AkCallbackSE(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
    {
        lastSESoundID = 0;

    }

    #region 加载SoundBank

    private bool LoadSound(uint soundID)
    {
        if (soundID == 0)
        {
            Log.Error("soundID不能为0");

            return false;
        }

        string bankName;

        if (!m_BankInfoDict.TryGetValue(soundID, out bankName))
        {
            Log.Error(string.Format("加载event（{0}）失败,没有找到所属的SoundEvent", soundID));

            return false;
        }

        if (!m_LoadBankList.Contains(bankName))
        {
            //加载SoundBank
            AkBankManager.LoadBank(bankName, false, false);

            m_LoadBankList.Add(bankName);
        }

        return true;

    }

    #endregion

    /// <summary>
    /// 根据ID停止声音
    /// </summary>
    /// <param name="id"></param> playingid
    /// <param name="dura"></param> duration time
    public void StopPlayingID(uint id, int dura = 0)
    {
        AkSoundEngine.StopPlayingID(id, dura);

        if (id == bGMPlayingID)

        {
            lastBGMSoundID = 0;

        }

        if (id == sEPlayingID)

        {
            lastSESoundID = 0;

        }
    }

    public void StopBMGSound()
    {
        AkSoundEngine.StopAll(listenerBGM);

        lastBGMSoundID = 0;

    }

    public void StopSESound()
    {
        AkSoundEngine.StopAll(listenerSE);

        lastSESoundID = 0;

    }

    public void StopAll()
    {
        StopBMGSound();

        StopSESound();

    }

    //看剧情长短或者篇章考虑下需要不需要清理bank吧
    public void ClearBanks()
    {
        AkSoundEngine.ClearBanks();

        m_LoadBankList.Clear();

        lastBGMSoundID = 0;

        lastSESoundID = 0;
    }

}
