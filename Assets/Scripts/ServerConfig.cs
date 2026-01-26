using System;
using System.Collections.Generic;
using UnityEngine;

namespace Clients
{
    [Serializable]
    public class ServerInfo
    {
        public string name;
        public string address;
        public string password;
        public bool isDefault;

        public ServerInfo(string name, string address, string password = "", bool isDefault = false)
        {
            this.name = name;
            this.address = address;
            this.password = password;
            this.isDefault = isDefault;
        }
    }

    [Serializable]
    public class ServerList
    {
        public List<ServerInfo> servers = new List<ServerInfo>();
    }

    public static class ServerConfig
    {
        private const string SERVERS_KEY = "SavedServers";
        private const string SELECTED_SERVER_KEY = "SelectedServerAddress";
        private const string DEFAULT_SERVER_NAME = "SimU Official";
        private const string DEFAULT_SERVER_ADDRESS = "https://api.simugameservice.lekina.me";

        private static ServerList _serverList;
        private static string _currentServerAddress;

        public static string CurrentServerAddress
        {
            get
            {
                if (string.IsNullOrEmpty(_currentServerAddress))
                {
                    _currentServerAddress = PlayerPrefs.GetString(SELECTED_SERVER_KEY, DEFAULT_SERVER_ADDRESS);
                }
                return _currentServerAddress;
            }
            set
            {
                _currentServerAddress = value;
                PlayerPrefs.SetString(SELECTED_SERVER_KEY, value);
                PlayerPrefs.Save();
            }
        }

        public static string SignalRUrl => $"{CurrentServerAddress}/unity";

        public static List<ServerInfo> GetServers()
        {
            if (_serverList == null)
            {
                LoadServers();
            }
            return _serverList.servers;
        }

        private static void LoadServers()
        {
            string json = PlayerPrefs.GetString(SERVERS_KEY, "");
            if (string.IsNullOrEmpty(json))
            {
                _serverList = new ServerList();
                _serverList.servers.Add(new ServerInfo(DEFAULT_SERVER_NAME, DEFAULT_SERVER_ADDRESS, "", true));
                SaveServers();
            }
            else
            {
                _serverList = JsonUtility.FromJson<ServerList>(json);
                if (_serverList == null || _serverList.servers == null || _serverList.servers.Count == 0)
                {
                    _serverList = new ServerList();
                    _serverList.servers.Add(new ServerInfo(DEFAULT_SERVER_NAME, DEFAULT_SERVER_ADDRESS, "", true));
                    SaveServers();
                }
            }
        }

        private static void SaveServers()
        {
            string json = JsonUtility.ToJson(_serverList);
            PlayerPrefs.SetString(SERVERS_KEY, json);
            PlayerPrefs.Save();
        }

        public static void AddServer(string name, string address, string password = "")
        {
            if (_serverList == null)
            {
                LoadServers();
            }

            foreach (var server in _serverList.servers)
            {
                if (server.address == address)
                {
                    server.name = name;
                    server.password = password;
                    SaveServers();
                    return;
                }
            }

            _serverList.servers.Add(new ServerInfo(name, address, password, false));
            SaveServers();
        }

        public static void RemoveServer(string address)
        {
            if (_serverList == null)
            {
                LoadServers();
            }

            _serverList.servers.RemoveAll(s => s.address == address && !s.isDefault);
            SaveServers();
        }

        public static void SetDefaultServer(string address)
        {
            if (_serverList == null)
            {
                LoadServers();
            }

            foreach (var server in _serverList.servers)
            {
                server.isDefault = (server.address == address);
            }
            SaveServers();
        }

        public static ServerInfo GetDefaultServer()
        {
            if (_serverList == null)
            {
                LoadServers();
            }

            foreach (var server in _serverList.servers)
            {
                if (server.isDefault)
                {
                    return server;
                }
            }

            if (_serverList.servers.Count > 0)
            {
                return _serverList.servers[0];
            }

            return new ServerInfo(DEFAULT_SERVER_NAME, DEFAULT_SERVER_ADDRESS, "", true);
        }

        public static void SelectServer(string address)
        {
            CurrentServerAddress = address;
            Debug.Log($"Server selected: {address}");
        }

        public static void SelectDefaultServer()
        {
            var defaultServer = GetDefaultServer();
            SelectServer(defaultServer.address);
        }
    }
}
