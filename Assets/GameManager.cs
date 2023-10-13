// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Launcher.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in "PUN Basic tutorial" to handle typical game management requirements
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement; 

using ExitGames.Client.Photon;

namespace ExitGames.Demos.DemoAnimator
{
	/// <summary>
	/// Game manager.
	/// Connects and watch Photon Status, Instantiate Player
	/// Deals with quiting the room and the game
	/// Deals with level loading (outside the in room synchronization)
	/// </summary>
	public class GameManager : Photon.PunBehaviour {

		#region Public Variables

		static public GameManager Instance;
		public Vector2 schermo;
		[Tooltip("The prefab to use for representing the player")]
		public GameObject playerPrefab;

		#endregion

		#region Private Variables

		private GameObject instance;

		#endregion

		#region MonoBehaviour CallBacks

		void Start()
		{
			schermo = Camera.main.ViewportToWorldPoint(Vector2.one);
			Instance = this;

			// in case we started this demo with the wrong scene being active, simply load the menu scene
			if (!PhotonNetwork.connected)
			{
				SceneManager.LoadScene(0);

				return;
			}

			if (playerPrefab == null) { // #Tip Never assume public properties of Components are filled up properly, always check and inform the developer of it.
				
				Debug.LogError("<Color=Red><b>Missing</b></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
			} else {

				if (SceneManager.GetActiveScene().buildIndex == 2)
				{
					if (PlayerPrefs.GetInt("audio") == 0)
                    {
						Destroy(FindObjectOfType<AudioSource>());
                    }
					if (movimento.LocalPlayerInstance == null)
					{
						Debug.Log("We are Instantiating LocalPlayer from " + SceneManagerHelper.ActiveSceneName);
						GameObject s = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector2(schermo.x, 0), Quaternion.identity, 0);
						if (SceneManager.GetActiveScene().buildIndex == 1)
						{
							s.GetComponent<Animator>().applyRootMotion = false;
							s.transform.position = Vector2.zero;
						}
					}
					else
					{

						Debug.Log("Ignoring scene load for " + SceneManagerHelper.ActiveSceneName);
					}

					for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                    {
						if (PhotonNetwork.playerList[i].IsMasterClient)
                        {
							nome1.text = PhotonNetwork.playerList[i].NickName;
							if (string.IsNullOrEmpty(nome1.text))
							{
								nome1.text = "Player " + PhotonNetwork.playerList[i].ID;
							}
						}
						else
                        {
							nome2.text = PhotonNetwork.playerList[i].NickName;
							if (string.IsNullOrEmpty(nome2.text))
							{
								nome2.text = "Player " + PhotonNetwork.playerList[i].ID;
							}
						}
					}

					if (!PhotonNetwork.isMasterClient)
                    {
						nome1.color = Color.red;
						nome2.color = Color.green;
                    }
				}
			}
		}
		#endregion
		public UnityEngine.UI.Text nome1, nome2;

		#region Photon Messages
		

		/// <summary>
		/// Called when the local player left the room. We need to load the launcher scene.
		/// </summary>
		public override void OnLeftRoom()
		{
			PhotonNetwork.Disconnect();
			SceneManager.LoadScene(0);
		}

		#endregion

		#region Public Methods

		public void LeaveRoom()
		{
			PhotonNetwork.LeaveRoom();
			PhotonNetwork.Disconnect();
		}

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
			if (PhotonNetwork.isMasterClient)
			{
				PhotonNetwork.LoadLevel(2);
			}
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
			PhotonNetwork.Disconnect();
        }
        #endregion
        /*
		#region Private Methods
		
		void LoadArena()
		{
			if ( ! PhotonNetwork.isMasterClient ) 
			{
				Debug.LogError( "PhotonNetwork : Trying to Load a level but we are not the master Client" );
			}

			Debug.Log( "PhotonNetwork : Loading Level : " + PhotonNetwork.room.PlayerCount ); 

			PhotonNetwork.LoadLevel("PunBasics-Room for "+PhotonNetwork.room.PlayerCount);
		}

		#endregion*/

    }

}