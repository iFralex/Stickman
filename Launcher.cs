// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Launcher.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in "PUN Basic tutorial" to connect, and join/create room automatically
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

/// <summary>
/// Launch manager. Connect, join a random room or create one if none or all full.
/// </summary>
namespace ExitGames.Demos.DemoAnimator
{
	public class Launcher : Photon.PunBehaviour {

		#region Public Variables

		[Tooltip("The Ui Panel to let the user enter name, connect and play")]
		public GameObject controlPanel;

		[Tooltip("The maximum number of players per room")]
		public byte maxPlayersPerRoom = 4;

		[Tooltip("The UI Loader Anime")]
		public LoaderAnime loaderAnime;

		#endregion

		#region Private Variables
		/// <summary>
		/// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon, 
		/// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
		/// </summary>
		bool isConnecting;

		/// <summary>
		/// This client's version number. Users are separated from each other by gameversion (which allows you to make breaking changes).
		/// </summary>
		string _gameVersion = "1";

		#endregion

		#region MonoBehaviour CallBacks

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity during early initialization phase.
		/// </summary>
		void Awake()
		{
			if (loaderAnime==null)
			{
				Debug.LogError("<Color=Red><b>Missing</b></Color> loaderAnime Reference.",this);
			}

			// #Critical
			// we don't join the lobby. There is no need to join a lobby to get the list of rooms.
			PhotonNetwork.autoJoinLobby = false;

			// #Critical
			// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
			PhotonNetwork.automaticallySyncScene = true;

			if (PlayerPrefs.HasKey("audio"))
            {
				if (PlayerPrefs.GetInt("audio") == 0)
                {
					audioBot.GetComponent<Image>().sprite = audioIc[0];
                }
				else
                {
					audioBot.GetComponent<Image>().sprite = audioIc[1];
				}
            }
			else
            {
				PlayerPrefs.SetInt("audio", 1);
            }
		}

		#endregion
		public Button audioBot;
		public Sprite[] audioIc;

		public void Audio()
		{
			if (audioBot.GetComponent<Image>().sprite == audioIc[1])
			{
				audioBot.GetComponent<Image>().sprite = audioIc[0];
				PlayerPrefs.DeleteKey("audio");
				PlayerPrefs.SetInt("audio", 0);
			}
			else
			{
				audioBot.GetComponent<Image>().sprite = audioIc[1];
				PlayerPrefs.DeleteKey("audio");
				PlayerPrefs.SetInt("audio", 1);
			}
		}
        #region Public Methods

        /// <summary>
        /// Start the connection process. 
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
		{
			// keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
			isConnecting = true;

			// hide the Play button for visual consistency
			controlPanel.SetActive(false);

			// start the loader animation for visual effect.
			if (loaderAnime!=null)
			{
				loaderAnime.StartLoaderAnimation();
			}

			// we check if we are connected or not, we join if we are , else we initiate the connection to the server.
			if (PhotonNetwork.connected)
			{
				LogFeedback("Joining Room...");
				// #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnPhotonRandomJoinFailed() and we'll create one.
				PhotonNetwork.JoinRandomRoom();
			}else{

				LogFeedback("Connecting...");
				
				// #Critical, we must first and foremost connect to Photon Online Server.
				PhotonNetwork.ConnectUsingSettings(_gameVersion);
			}
		}

		/// <summary>
		/// Logs the feedback in the UI view for the player, as opposed to inside the Unity Editor for the developer.
		/// </summary>
		/// <param name="message">Message.</param>
		void LogFeedback(string message)
		{
			Debug.Log(Environment.NewLine+message);
		}

		#endregion

		public bool conAmici, crea;
		public InputField nomeStanzaCreare, nomeStanzaEntra;
		public Text errore;

		public void ConAmici()
        {
			conAmici = true;
			Connect();
        }

		public void Create()
        {
			crea = true;
			Connect();
        }
		#region Photon.PunBehaviour CallBacks
		// below, we implement some callbacks of PUN
		// you can find PUN's callbacks in the class PunBehaviour or in enum PhotonNetworkingMessage


		/// <summary>
		/// Called after the connection to the master is established and authenticated but only when PhotonNetwork.autoJoinLobby is false.
		/// </summary>
        
		public override void OnConnectedToMaster()
		{

			Debug.Log("Region:" + PhotonNetwork.networkingPeer.CloudRegion);

			// we don't want to do anything if we are not attempting to join a room. 
			if (isConnecting)
			{
				if (!conAmici)
				{
					if (!crea)
						PhotonNetwork.JoinRandomRoom();
					else
					{
						if (!string.IsNullOrEmpty(nomeStanzaCreare.text))
						{
							RoomOptions option = new RoomOptions();
							option.IsVisible = false;
							PhotonNetwork.CreateRoom(nomeStanzaCreare.text);
						}
						else
						{
							errore.text = "please enter the name in input field";
							PhotonNetwork.Disconnect();
							StartCoroutine(Disabilita());
						}
					}
				}
				else
				{
					if (!string.IsNullOrEmpty(nomeStanzaEntra.text))
						PhotonNetwork.JoinRoom(nomeStanzaEntra.text);
					else
                    {
						errore.text = "please enter the name in input field";
						PhotonNetwork.Disconnect();
						StartCoroutine(Disabilita());
                    }
				}
			}
		}

        /// <summary>
        /// Called when a JoinRandom() call failed. The parameter provides ErrorCode and message.
        /// </summary>
        /// <remarks>
        /// Most likely all rooms are full or no rooms are available. <br/>
        /// </remarks>
        /// <param name="codeAndMsg">codeAndMsg[0] is short ErrorCode. codeAndMsg[1] is string debug msg.</param>
        public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
			errore.text = (string)codeAndMsg[1];
			PhotonNetwork.Disconnect();
			StartCoroutine(Disabilita());
        }

		IEnumerator Disabilita()
        {
			yield return new WaitForSeconds(2);
			errore.text = "";
        }

		public override void OnCreatedRoom()
		{
			if (crea)
			{
				PhotonNetwork.room.IsVisible = false;
				//PhotonNetwork.JoinRoom(nomeStanzaCreare.text);
			}
        }

        public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
        {
			errore.text = (string)codeAndMsg[1];
			crea = false;
			PhotonNetwork.Disconnect();
			StartCoroutine(Disabilita());
        }

        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
		{
			LogFeedback("<Color=Red>OnPhotonRandomJoinFailed</Color>: Next -> Create a new Room");
			Debug.Log("DemoAnimator/Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");

			// #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
			PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = this.maxPlayersPerRoom}, null);
		}


        /// <summary>
        /// Called after disconnecting from the Photon server.
        /// </summary>
        /// <remarks>
        /// In some cases, other callbacks are called before OnDisconnectedFromPhoton is called.
        /// Examples: OnConnectionFail() and OnFailedToConnectToPhoton().
        /// </remarks>
        ///
        public override void OnFailedToConnectToPhoton(DisconnectCause cause)
        {
			errore.text = cause.ToString();
			StartCoroutine(Disabilita());
		}
        public override void OnDisconnectedFromPhoton()
		{
			LogFeedback("<Color=Red>OnDisconnectedFromPhoton</Color>");
			Debug.LogError("DemoAnimator/Launcher:Disconnected");

			// #Critical: we failed to connect or got disconnected. There is not much we can do. Typically, a UI system should be in place to let the user attemp to connect again.
			loaderAnime.StopLoaderAnimation();

			isConnecting = false;
			if (!conAmici && !crea)
			{
				controlPanel.SetActive(true);
			}
			else
            {
				conAmici = false;
            }
		}

		public override void OnJoinedRoom()
		{
			LogFeedback("<Color=Green>OnJoinedRoom</Color> with "+PhotonNetwork.room.PlayerCount+" Player(s)");
			Debug.Log("DemoAnimator/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
		
			// #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.automaticallySyncScene to sync our instance scene.
			if (PhotonNetwork.room.PlayerCount == 1)
			{
				Debug.Log("We load the 'Room for 1' ");

				// #Critical
				// Load the Room Level. 
				PhotonNetwork.LoadLevel(1);

			}
		}
        #endregion

    }
}