using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SocketTool
{
    public class ServerSocketTab
    {
        internal void OnGUI()
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(new GUIContent("Server"));

            if (GUILayout.Button("开启", GUILayout.MaxWidth(75f)))
                SocketOpen();

            GUILayout.EndHorizontal();
        }

        private void SocketOpen()
        {
            ServerSocketManager.Open(SocketUtils.GetLocalIP(),5555);
        }
    }
}
