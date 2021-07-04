using UnityEditor;
using UnityEngine;

namespace SocketTool
{
    public class SocketToolMain : EditorWindow
    {
        private static SocketToolMain s_instance = null;
        internal static SocketToolMain instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = GetWindow<SocketToolMain>();
                return s_instance;
            }
        }

        [MenuItem("Window/SocketTool")]
        private static void Open()
        {
            s_instance = null;
            instance.titleContent = new GUIContent("SocketTool");
            instance.Show();
        }

        enum Mode
        {
            Server,
            Client,
        }

        [SerializeField]
        Mode m_Mode;

        [SerializeField]
        internal ServerSocketTab m_ServerTab;

        [SerializeField]
        internal ClientSocketTab m_ClientTab;

        const float k_ToolbarPadding = 15;
        const float k_MenubarPadding = 32;
        private Texture2D m_RefreshTexture;

        private void OnEnable()
        {

            if (m_ServerTab == null)
                m_ServerTab = new ServerSocketTab();
            //m_ServerTab.OnEnable(this);
            if (m_ClientTab == null)
                m_ClientTab = new ClientSocketTab();
            //m_ClientTab.OnEnable(this);

            m_RefreshTexture = EditorGUIUtility.FindTexture("Refresh");
        }

        private void OnGUI()
        {
            ModeToggle();

            switch (m_Mode)
            {
                case Mode.Client:
                    m_ClientTab.OnGUI();
                    break;
                case Mode.Server:
                default:
                    m_ServerTab.OnGUI();
                    break;
            }
        }

        private void ModeToggle()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(k_ToolbarPadding);
            GUILayout.Space(m_RefreshTexture.width + k_ToolbarPadding);
            float toolbarWidth = position.width - k_ToolbarPadding * 4 - m_RefreshTexture.width;
            string[] labels = new string[] { "服务器", "客户端" };
            m_Mode = (Mode)GUILayout.Toolbar((int)m_Mode, labels, "LargeButton", GUILayout.Width(toolbarWidth));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}