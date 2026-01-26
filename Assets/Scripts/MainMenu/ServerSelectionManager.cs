using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Clients;

public class ServerSelectionManager : MonoBehaviour
{
    public GameObject serverListContent;
    public GameObject serverItemPrefab;
    public InputField serverNameInput;
    public InputField serverAddressInput;
    public InputField serverPasswordInput;
    public Text statusText;
    public MainMenuManager mainMenuManager;

    private List<GameObject> serverListItems = new List<GameObject>();

    void Start()
    {
        RefreshServerList();
    }

    void OnEnable()
    {
        RefreshServerList();
    }

    public void RefreshServerList()
    {
        foreach (var item in serverListItems)
        {
            Destroy(item);
        }
        serverListItems.Clear();

        var servers = ServerConfig.GetServers();
        foreach (var server in servers)
        {
            CreateServerListItem(server);
        }
    }

    private void CreateServerListItem(ServerInfo server)
    {
        if (serverItemPrefab == null || serverListContent == null)
        {
            Debug.LogWarning("Server item prefab or list content is null");
            return;
        }

        GameObject item = Instantiate(serverItemPrefab, serverListContent.transform);
        serverListItems.Add(item);

        Text nameText = item.transform.Find("ServerName")?.GetComponent<Text>();
        if (nameText != null)
        {
            nameText.text = server.name;
        }

        Text addressText = item.transform.Find("ServerAddress")?.GetComponent<Text>();
        if (addressText != null)
        {
            addressText.text = server.address;
        }

        Toggle defaultToggle = item.transform.Find("DefaultToggle")?.GetComponent<Toggle>();
        if (defaultToggle != null)
        {
            defaultToggle.isOn = server.isDefault;
            string serverAddress = server.address;
            defaultToggle.onValueChanged.AddListener((isOn) => {
                if (isOn)
                {
                    OnSetDefaultServer(serverAddress);
                }
            });
        }

        Button selectButton = item.transform.Find("SelectButton")?.GetComponent<Button>();
        if (selectButton != null)
        {
            string serverAddress = server.address;
            selectButton.onClick.AddListener(() => OnSelectServer(serverAddress));
        }

        Button deleteButton = item.transform.Find("DeleteButton")?.GetComponent<Button>();
        if (deleteButton != null)
        {
            if (server.isDefault)
            {
                deleteButton.gameObject.SetActive(false);
            }
            else
            {
                string serverAddress = server.address;
                deleteButton.onClick.AddListener(() => OnDeleteServer(serverAddress));
            }
        }
    }

    public void OnAddServerButtonPressed()
    {
        string name = serverNameInput != null ? serverNameInput.text : "";
        string address = serverAddressInput != null ? serverAddressInput.text : "";
        string password = serverPasswordInput != null ? serverPasswordInput.text : "";

        if (string.IsNullOrEmpty(name))
        {
            SetStatus("Please enter a server name.");
            return;
        }

        if (string.IsNullOrEmpty(address))
        {
            SetStatus("Please enter a server address.");
            return;
        }

        if (!address.StartsWith("http://") && !address.StartsWith("https://"))
        {
            address = "https://" + address;
        }

        ServerConfig.AddServer(name, address, password);
        
        if (serverNameInput != null) serverNameInput.text = "";
        if (serverAddressInput != null) serverAddressInput.text = "";
        if (serverPasswordInput != null) serverPasswordInput.text = "";

        SetStatus("Server added successfully!");
        RefreshServerList();
    }

    public void OnSelectServer(string address)
    {
        ServerConfig.SelectServer(address);
        SetStatus("Connected to server: " + address);
    }

    public void OnSetDefaultServer(string address)
    {
        ServerConfig.SetDefaultServer(address);
        SetStatus("Default server set: " + address);
        RefreshServerList();
    }

    public void OnDeleteServer(string address)
    {
        ServerConfig.RemoveServer(address);
        SetStatus("Server removed.");
        RefreshServerList();
    }

    public void OnBackButtonPressed()
    {
        if (mainMenuManager != null)
        {
            mainMenuManager.OnBackToMainMenu();
        }
    }

    private void SetStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
        Debug.Log("Server Selection: " + message);
    }
}
