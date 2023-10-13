using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimento : Photon.MonoBehaviour
{
    public AnimatorClipInfo animazione;
    bool vai = true;
    public bool vittoria, fine;
    public UnityEngine.UI.Text vinto;
    public static GameObject LocalPlayerInstance;

    void Start()
    {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instanciation when levels are synchronized
        if (photonView.isMine)
        {
            LocalPlayerInstance = gameObject;
        }

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (GetComponent<PhotonView>().owner.IsMasterClient)
            {
                transform.position *= -1;
            }
            UnityEngine.UI.Text[] o = FindObjectsOfType<UnityEngine.UI.Text>();
            for (int i = 0; i < o.Length; i++)
            {
                if (o[i].name == "vittoria")
                {
                    vinto = o[i];
                }
            }
        }
    }

    void Update()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (!fine)
            {
                if (photonView.isMine)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        GetComponent<Animator>().SetTrigger("salta");
                        if (vai)
                        {
                            StartCoroutine(avanti());
                        }
                    }
                }

                if (transform.position.x <= 0.1f && transform.position.x >= -0.1f)
                {
                    vittoria = true;
                    if (photonView.isMine)
                    {
                        vinto.text = "VICTORY";
                        vinto.color = Color.blue;
                    }
                    else
                    {
                        vinto.text = "lose";
                        vinto.color = Color.red;
                    }
                    movimento[] al = FindObjectsOfType<movimento>();
                    if (al[0] != this)
                    {
                        al[0].fine = true;
                    }
                    else
                    {
                        al[1].fine = true;
                    }
                    StartCoroutine(disconnetti());
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<Animator>().SetTrigger("salta");
                if (vai)
                {
                    StartCoroutine(avanti());
                }
            }
        }
    }

    IEnumerator avanti()
    {
        vai = false;
        yield return new WaitForSeconds(.1f);
        if (transform.position.x != 0)
        {
            transform.Translate(new Vector2(transform.position.x / Mathf.Abs(transform.position.x) * -.2f, 0));
        }
        transform.Translate(new Vector2(0, .3f));
        yield return new WaitForSeconds(.1f);
        transform.Translate(new Vector2(0, -.3f));
        vai = true;
    }

    public IEnumerator disconnetti()
    {
        fine = true;
        yield return new WaitForSeconds(3);
        PhotonNetwork.Disconnect();
    }
}
