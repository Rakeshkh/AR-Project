
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



using System.Net.Sockets;
using System.Net;


public class RakeshServer {

    private string ip_address = "128.199.221.245";
    private int port_number = 6969;
    private Socket socket;

    /**
     * rules: 
     *   set =>   set key value
     *   get =>   get key
     */

    public RakeshServer() {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        System.Net.IPAddress remoteIPAddress = System.Net.IPAddress.Parse(ip_address);
        System.Net.IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(remoteIPAddress, port_number);
        socket.Connect(remoteEndPoint);
    }

    public void close() {
        socket.Close();
    }

    public string set(string name, string value){
        string msg = "set " + name + " " + value;
        byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(msg);
        socket.Send(utf8Bytes);
        byte[] buffer = new byte[200];
        int N = socket.Receive(buffer, buffer.Length, 0);
        if (N == 0) return "";
        return System.Text.Encoding.UTF8.GetString(buffer);
    }

    public string get(string name) {
        string msg = "get " + name;
        byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(msg);
        socket.Send(utf8Bytes);
        byte[] buffer = new byte[200];
        int N = socket.Receive(buffer, buffer.Length, 0);
        if (N == 0) return "";
        return System.Text.Encoding.UTF8.GetString(buffer);
    }
	
}
