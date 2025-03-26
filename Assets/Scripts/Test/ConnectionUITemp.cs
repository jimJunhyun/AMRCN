using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionUITemp : MonoBehaviour
{
    public Button serv;
    public Button host;
    public Button cli;


	private void Awake()
	{
		serv = GameObject.Find("Serv").GetComponent<Button>();
		host = GameObject.Find("Host").GetComponent<Button>();
		cli = GameObject.Find("Cli").GetComponent<Button>();

		serv.onClick.AddListener(()=> NetworkManager.Singleton.StartServer());
		host.onClick.AddListener(()=> NetworkManager.Singleton.StartHost());
		cli.onClick.AddListener(()=> NetworkManager.Singleton.StartClient());
	}
}
