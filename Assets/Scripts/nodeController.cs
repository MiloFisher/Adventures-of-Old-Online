using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nodeController : MonoBehaviour
{
    public GameObject testCollider;
    private Collider2D t1;
    private Collider2D t2;
    public GameObject test1;
    public GameObject test2;
    private float xmod = 73.5f / 27.0f;
    private float ymod = 42.0f / 18.0f;
    private GameObject[] nodes = new GameObject[6];
    public int newLength;
    private bool failed;
    private GameObject[] barriers;

    public void Spread(int length)
    {
        if (length == 0)
            Destroy(gameObject);
        else
        {
            List<string> data = new List<string>(gameObject.GetComponent<Text>().text.Split(','));
            int x = int.Parse(data[0]);
            int y = int.Parse(data[1]);
            int yspecial = 0;
            if (x % 2 == 1)
                yspecial = 1;

            GameObject[] allNodes = GameObject.FindGameObjectsWithTag("Node");
            //Debug.Log("Found Nodes: " + allNodes.Length);
            List<int> xs = new List<int>();
            List<int> ys = new List<int>();

            for (int i = 0; i < allNodes.Length; i++)
            {
                if (allNodes[i] != null)
                {
                    if (allNodes[i].name != "Node" && allNodes[i] != gameObject)
                    {
                        //for (int j = 0; j < allNodes.Length; j++)
                        //{
                        //    if (allNodes[i].GetComponent<Text>().text == allNodes[j].GetComponent<Text>().text && allNodes[i] != allNodes[j])
                        //    {
                        //        Destroy(allNodes[i]);
                        //    }
                        //}
                        if (allNodes[i] != null)
                        {
                            data = new List<string>(allNodes[i].GetComponent<Text>().text.Split(','));
                            xs.Add(int.Parse(data[0]));
                            ys.Add(int.Parse(data[1]));
                        }
                    }
                }
            }

            bool locationFail;
            newLength = length - 1;
            

            //above
            locationFail = false;
            for (int i = 0; i < xs.Count; i++)
            {
                if (x == xs[i] && y == ys[i] + 1)
                {
                    locationFail = true;
                }
            }
            //if (!locationFail)
            {
                var node1 = Instantiate(gameObject);
                node1.GetComponent<Text>().text = x + "," + (y + 1);
                node1.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + ymod);
                nodes[0] = node1;
            }

            //below
            locationFail = false;
            for (int i = 0; i < xs.Count; i++)
            {
                if (x == xs[i] && y == ys[i] - 1)
                {
                    locationFail = true;
                }
            }
            if (!locationFail)
            {
                var node2 = Instantiate(gameObject);
                node2.GetComponent<Text>().text = x + "," + (y - 1);
                node2.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - ymod);
                nodes[1] = node2;
            }

            //right top
            locationFail = false;
            for (int i = 0; i < xs.Count; i++)
            {
                if (x == xs[i] + 1 && y == ys[i] + 1 - yspecial)
                {
                    locationFail = true;
                }
            }
            //if (!locationFail)
            {
                var node3 = Instantiate(gameObject);
                node3.GetComponent<Text>().text = (x + 1) + "," + (y + 1 - yspecial);
                node3.transform.position = new Vector2(gameObject.transform.position.x + xmod, gameObject.transform.position.y + ymod / 2);
                nodes[2] = node3;
            }

            //right bottom
            locationFail = false;
            for (int i = 0; i < xs.Count; i++)
            {
                if (x == xs[i] + 1 && y == ys[i] - yspecial)
                {
                    locationFail = true;
                }
            }
            //if (!locationFail)
            {
                var node4 = Instantiate(gameObject);
                node4.GetComponent<Text>().text = (x + 1) + "," + (y - yspecial);
                node4.transform.position = new Vector2(gameObject.transform.position.x + xmod, gameObject.transform.position.y - ymod / 2);
                nodes[3] = node4;
            }

            //left top
            locationFail = false;
            for (int i = 0; i < xs.Count; i++)
            {
                if (x == xs[i] - 1 && y == ys[i] + 1 - yspecial)
                {
                    locationFail = true;
                }
            }
            //if (!locationFail)
            {
                var node5 = Instantiate(gameObject);
                node5.GetComponent<Text>().text = (x - 1) + "," + (y + 1 - yspecial);
                node5.transform.position = new Vector2(gameObject.transform.position.x - xmod, gameObject.transform.position.y + ymod / 2);
                nodes[4] = node5;
            }

            //left bottom
            locationFail = false;
            for (int i = 0; i < xs.Count; i++)
            {
                if (x == xs[i] - 1 && y == ys[i] - yspecial)
                {
                    locationFail = true;
                }
            }
            //if (!locationFail)
            {
                var node6 = Instantiate(gameObject);
                node6.GetComponent<Text>().text = (x - 1) + "," + (y - yspecial);
                node6.transform.position = new Vector2(gameObject.transform.position.x - xmod, gameObject.transform.position.y - ymod / 2);
                nodes[5] = node6;
            }
            //StartCoroutine(TimeGap(newLength,node1,node2,node3,node4,node5,node6));
            barriers = GameObject.FindGameObjectsWithTag("Barrier");
            

            test1 = Instantiate(testCollider,new Vector3(0,-200,0), Quaternion.identity);
            test1.transform.position = new Vector2(0, -100);
            t1 = test1.GetComponent<BoxCollider2D>();

            test2 = Instantiate(testCollider, new Vector3(0, -300, 0), Quaternion.identity);
            test2.transform.position = new Vector2(0, -100);
            t2 = test2.GetComponent<BoxCollider2D>();

            //Debug.Log(barriers.Length);
            //bool failed;
            //for (int i = 0; i < 6; i++)
            //{
            //    failed = false;
            //    for (int j = 0; j < barriers.Length; j++)
            //    {
            //        if (Physics2D.IsTouching(nodes[i].GetComponent<CircleCollider2D>(), barriers[j].GetComponent<BoxCollider2D>()) == true)
            //        {
            //            Debug.Log("Touching Detected: " + i + "," + j);
            //            failed = true;
            //        }
            //    }
            //    if(!failed)
            //        nodes[i].GetComponent<nodeController>().Spread(newLength);
            //}
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

    //IEnumerator TimeGap(int newLength, GameObject node1, GameObject node2, GameObject node3, GameObject node4, GameObject node5, GameObject node6)
    //{
    //    yield return new WaitForSeconds(.1f);
    //    if (node1 != null)
    //        node1.GetComponent<nodeController>().Spread(newLength);
    //    if (node2 != null)
    //        node2.GetComponent<nodeController>().Spread(newLength);
    //    if (node3 != null)
    //        node3.GetComponent<nodeController>().Spread(newLength);
    //    if (node4 != null)
    //        node4.GetComponent<nodeController>().Spread(newLength);
    //    if (node5 != null)
    //        node5.GetComponent<nodeController>().Spread(newLength);
    //    if (node6 != null)
    //        node6.GetComponent<nodeController>().Spread(newLength);
    //}

    void Update()
    {
        if (test1 != null && test2 != null)
        {
            if (Physics2D.IsTouching(t1, t2))
            {
                for (int i = 0; i < nodes.Length; i++)
                {
                    failed = false;
                    if (nodes[i] != null)
                    {
                        for (int j = 0; j < barriers.Length; j++)
                        {

                            if (Physics2D.IsTouching(nodes[i].GetComponent<CircleCollider2D>(), barriers[j].GetComponent<BoxCollider2D>()) == true)
                            {
                                failed = true;
                            }

                        }
                        if (!failed)
                        {
                            nodes[i].GetComponent<nodeController>().Spread(newLength);
                            Debug.Log(newLength);
                        }
                    }
                }
                //GameObject[] test = GameObject.FindGameObjectsWithTag("Test");
                //for (int i = 0; i < test.Length; i++)
                //{
                //    if(test[i].name != "Test Collider")
                //        Destroy(test[i]);
                //}
                
                Destroy(test1);
                Destroy(test2);
            }
        }
    }
}
