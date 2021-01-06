using UnityEngine.UI;

public class Alert : DialogBase<Alert>
{
    public Text text;

    public static string url = "Dialog/Alert";

    void Awake()
    {
        
    }

    void Start()
    {

    }
    void Update()
    {
        
    }

    public void onClickHide()
    {
        Hide();
    }

    /// <summary>
    /// 显示提示
    /// </summary>
    /// <param name="tips">提示文字</param>
    public static void Show(string tips)
    {
        Show((alert) => {
            alert.text.text = tips;
        });
    }
}
