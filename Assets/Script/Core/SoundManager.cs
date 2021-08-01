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

    //Bank映射表
    private Dictionary<uint, string> m_BankInfoDict = new Dictionary<uint, string>();
    //Id映射表
    private Dictionary<string, uint> m_IdDict = new Dictionary<string, uint>();

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

        // //做了一个0.1秒延迟
        //Timer.One(.1f, InitCompoent);
        InitCompoent();
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

                Log.Info(bankName);

                //判断SingleNode存在与否,比如Init.bak就没这个
                XmlNode eventNode = node.SelectSingleNode("IncludedEvents");

                if (eventNode != null)
                {
                    //拿到其中所有的event做一个映射
                    XmlNodeList eventList = eventNode.SelectNodes("Event");

                    foreach (XmlElement x1e in eventList)

                    {
                        m_BankInfoDict.Add(uint.Parse(x1e.Attributes["Id"].Value), bankName);
                        m_IdDict.Add(x1e.Attributes["Name"].Value, uint.Parse(x1e.Attributes["Id"].Value));
                    }
                }
            }
        });
    }

    #endregion


    private List<uint> playingIdList = new List<uint>();

    public uint PlayBGMSound(uint soundID, AkCallbackManager.EventCallback AkCallback = null, object cookie = null)
    {
        if (listenerBGM == null)
        {
            Log.Error("BGM还未初始化");

            return 0;
        }

        string bankName;

        if (!m_BankInfoDict.TryGetValue(soundID, out bankName))
        {
            Log.Error(string.Format("加载event（{0}）失败,没有找到所属的SoundEvent", soundID));
        }

        LoadSound(bankName);

        uint id = 0;

        AkCallbackManager.EventCallback AkCallbackBGM = (in_cookie, in_type, in_info) =>
        {
            playingIdList.Remove(id);
            Log.Info(string.Format("播放BGM结束,队列Id:{0}, Id: {1}", id, soundID));
        };

        id = AkSoundEngine.PostEvent(soundID, listenerBGM, 1, AkCallback + AkCallbackBGM, cookie);
        playingIdList.Add(id);

        if (id > 0)
        {
            Log.Info(string.Format("播放BGM开始,队列Id:{0}, Id: {1}", id, soundID));
        }

        return id;

    }

    public uint PlayBGMSound(string soundName, AkCallbackManager.EventCallback AkCallback = null, object cookie = null)
    {
        if (listenerBGM == null)
        {
            Log.Error("BGM还未初始化");

            return 0;
        }

        uint soundID;

        if (!m_IdDict.TryGetValue(soundName, out soundID))
        {
            Log.Error(string.Format("加载event（{0}）失败,没有找到所属的SoundEvent", soundName));
        }

        uint id = PlayBGMSound(soundID, AkCallback, cookie);

        if (id > 0)
        {
            Log.Info(string.Format("播放BGM开始,队列Id:{0}, Name: {1}", id, soundName));
        }

        return id;
    }

    #region 加载SoundBank

    private bool LoadSound(string soundName)
    {
        if (string.IsNullOrEmpty(soundName))
        {
            Log.Error("soundName不能为空");

            return false;
        }

        if (!m_LoadBankList.Contains(soundName))
        {
            //加载SoundBank
            AkBankManager.LoadBank(soundName, false, false);

            m_LoadBankList.Add(soundName);
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
    }

    public void StopBMGSound()
    {
        AkSoundEngine.StopAll(listenerBGM);

    }
}
