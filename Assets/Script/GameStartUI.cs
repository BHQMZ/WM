//using System.Text;
//using UnityEngine;
//using UnityEngine.UI;

//public class GameStartUI : MonoBehaviour
//{
//    public Text outputText;
//    public InputField inputField;
//    public Button sendButton;

//    // Start is called before the first frame update
//    void Start()
//    {
//        SocketManager.Instance().Link();

//        SocketManager.Instance().Receive((bs) =>
//        {
//            outputText.text += new UTF8Encoding().GetString(bs).Replace("\0","") + "\n";
//        });

//        sendButton.onClick.AddListener(()=> {
//            SocketManager.Instance().SendString(inputField.text);
//            inputField.text = "";
//        });
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}
