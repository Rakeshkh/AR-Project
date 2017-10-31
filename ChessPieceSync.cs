using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class ChessPieceSync : MonoBehaviour {

    public string kname = "c1";
    public bool setter = false;
    public Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);
    private string tmp = "";
    Thread receiveThread;
    // Use this for initialization
    void Start () {
        receiveThread = new Thread(new ThreadStart(go));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    string fromVec3(Vector3 x) {
        return x.x + "," + x.y + "," + x.z;
    }
    bool fromString(string x, ref Vector3 r)
    {
        string[] ss = x.Split(',');
        if (ss.Length == 3)
        {

            r.x = float.Parse(ss[0]);
            r.y = float.Parse(ss[1]);
            r.z = float.Parse(ss[2]);
            //Debug.Log("fromVec " + r);
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update () {
        if (setter)
        {
            tmp = fromVec3(this.gameObject.transform.position);
        }
        else
        {
            Vector3 t = new Vector3();
            if (fromString(tmp, ref t))
            {
                //Debug.Log("Update " + t);
                this.gameObject.transform.position = t + offset;
            }
        }
	}

    void go()
    {
        RakeshServer s = new RakeshServer();

        string key = kname;
        if (key.Length == 0) key = "_";

        while (true)
        {
            if (setter)
            {
                //Debug.Log("setting" + tmp);
                s.set(key, tmp);
            }
            else
            {
                tmp = s.get(key);
                //Debug.Log("getting" + tmp);
            }
            
        }
    }
}
