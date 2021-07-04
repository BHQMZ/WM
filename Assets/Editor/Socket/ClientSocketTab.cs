using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SocketTool
{
    public class ClientSocketTab
    {
        [SerializeField]
        private Vector2 m_ScrollPosition;

        internal void OnGUI()
        {
            m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition);

            var centeredStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
            centeredStyle.alignment = TextAnchor.UpperCenter;
            GUILayout.Label(new GUIContent("Client"), centeredStyle);

            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();

            var ipUrl = EditorGUILayout.TextField("IP", SocketUtils.GetLocalIP());

            var port = EditorGUILayout.TextField("端口", "5555");

            EditorGUILayout.Space();

            if (GUILayout.Button("连接", GUILayout.MaxWidth(75f)))
                SocketLink(ipUrl, port);

            GUILayout.EndHorizontal();

            EditorGUILayout.EndScrollView();
        }

        private void SocketLink(string ip,string port)
        {
            ClientSocketManager.Link(new ClientSocketManager.LinkInfo() { ip = ip, port = int.Parse(port) });
        }
    }
}
