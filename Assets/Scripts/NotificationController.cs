using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationController : MonoBehaviour
{
    public GameObject notification;
    public Text display;
    private GameObject[] players;
    private bool displayingNotification;
    private List<string> receivedMessages = new List<string>();
    private bool countingDown;

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player Info");
        notification.transform.localPosition = new Vector2(55, 20);
    }

    void Update()
    {
        for (int i = 1; i < players.Length; i++)
        {
            //if (!displayingNotification && players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MessageRecipient == "all" || players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MessageRecipient == players[0].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name)
            //{
            //    CreateNotification(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Message);
            //}
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MessageRecipient == "all" || players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MessageRecipient == players[0].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name)
            {
                bool newMessage = true;
                for (int j = 0; j < receivedMessages.Count; j++)
                {
                    if (receivedMessages[j] == players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Message)
                        newMessage = false;
                }
                if (newMessage)
                {
                    receivedMessages.Add(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Message);
                }
            }
        }
        if (receivedMessages.Count > 0 && !displayingNotification)
        {
            CreateNotification(receivedMessages[0]);
        }
    }

    public void CreateNotification(string message)
    {
        if (!displayingNotification)
        {
            display.text = message;
            StartCoroutine(NotificationAnimation());
        }
        else
        {
            Debug.Log("Already displaying notification cannot display:");
            Debug.Log(message);
            receivedMessages.Add(message);
        }
    }

    public void SendNotification(string target, string message)
    {
        players[0].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MessageRecipient = target;
        players[0].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Message = message;
        StartCoroutine(MessageLinger());
    }

    IEnumerator NotificationAnimation()
    {
        float holdTime = 3f;
        float loopTime = .025f;
        float moveAmount = 1f;

        displayingNotification = true;

        //[takes (20/moveAmount) * loopTime to extend]
        while (notification.transform.localPosition.x > 35)
        {
            notification.transform.localPosition = new Vector2(notification.transform.localPosition.x - moveAmount, 20);
            yield return new WaitForSeconds(loopTime);
        }

        //stay extended for holdTime
        yield return new WaitForSeconds(holdTime);

        //[takes (20/moveAmount) * loopTime to retract]
        while (notification.transform.localPosition.x < 55)
        {
            notification.transform.localPosition = new Vector2(notification.transform.localPosition.x + moveAmount, 20);
            yield return new WaitForSeconds(loopTime);
        }

        if (receivedMessages.Count > 0)
        {
            if (display.text == receivedMessages[0])
                receivedMessages.Remove(receivedMessages[0]);
        }

        displayingNotification = false;
    }

    IEnumerator MessageLinger()
    {
        float duration = 1f;
        yield return new WaitForSeconds(duration);
        players[0].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MessageRecipient = null;
        players[0].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Message = null;
    }
}
