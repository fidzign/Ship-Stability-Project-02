using System;
//using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Net;


public class ServerApp : MonoBehaviour {


    private TcpListener tcpListener;
    private Thread listenThread;
    private int connectedClients = 0;

    public GameObject ShipTarget;

    public GameObject[] Tangki;


    public static string messageToDisplay;

   // public Text txtMsg;

    private delegate void WriteMessageDelegate(string msg);

    private string msg;

    private string[] splitData;


   // private delegate void WriteMessageDelegate(string msg);

	// Use this for initialization

   
	void Start () {

        Server();
	
	}

    private void Server()
    {
        this.tcpListener = new TcpListener(IPAddress.Loopback, 3000); // Change to IPAddress.Any for internet wide Communication
        this.listenThread = new Thread(new ThreadStart(ListenForClients));
        this.listenThread.Start();
    }

    private void ListenForClients()
    {
        this.tcpListener.Start();

        while (true) // Never ends until the Server is closed.
        {
            //blocks until a client has connected to the server
            TcpClient client = this.tcpListener.AcceptTcpClient();

            //create a thread to handle communication 
            //with connected client
            connectedClients++; // Increment the number of clients that have communicated with us.

           // lblNumberOfConnections.Text = connectedClients.ToString();//diganti dengan UI

            Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
            clientThread.Start(client);
         
        }
    }

    private void ShipTransform()
    {

        ShipTarget.transform.position = new Vector3(0, -float.Parse(splitData[0]) / 10, 0);//for draft
        ShipTarget.transform.localEulerAngles = new Vector3(float.Parse(splitData[1]), 0, -float.Parse(splitData[2]));//roll pitch


        //isi tangki port
        Tangki[0].transform.localScale = new Vector3(1, float.Parse(splitData[3]) / 100, 1);//COT 1P
        Tangki[1].transform.localScale = new Vector3(1, float.Parse(splitData[4]) / 100, 1);//COT 2P
        Tangki[2].transform.localScale = new Vector3(1, float.Parse(splitData[5]) / 100, 1);//COT 3P
        Tangki[3].transform.localScale = new Vector3(1, float.Parse(splitData[6]) / 100, 1);//COT 4P
        Tangki[4].transform.localScale = new Vector3(1, float.Parse(splitData[7]) / 100, 1);//COT 5P


        //isi tangki starboard
        Tangki[5].transform.localScale = new Vector3(1, float.Parse(splitData[8]) / 100, 1);//COT 1S
        Tangki[6].transform.localScale = new Vector3(1, float.Parse(splitData[9]) / 100, 1);//COT 2S
        Tangki[7].transform.localScale = new Vector3(1, float.Parse(splitData[10]) / 100, 1);//COT 3S
        Tangki[8].transform.localScale = new Vector3(1, float.Parse(splitData[11]) / 100, 1);//COT 4S
        Tangki[9].transform.localScale = new Vector3(1, float.Parse(splitData[12]) / 100, 1);//COT 5S


    }

    private void HandleClientComm(object client)
    {
        TcpClient tcpClient = (TcpClient)client;
        NetworkStream clientStream = tcpClient.GetStream();

        byte[] message = new byte[4096];
        int bytesRead;

        while (true)
        {
            bytesRead = 0;

            try
            {
                //blocks until a client sends a message
                bytesRead = clientStream.Read(message, 0, 4096);
            }
            catch
            {
                //a socket error has occured
                break;
            }

            if (bytesRead == 0)
            {
                //the client has disconnected from the server
                connectedClients--;
               // lblNumberOfConnections.Text = connectedClients.ToString();//diganti dengan UI
               // Debug.Log("value sent :" + connectedClients.ToString());

                break;
            }

            //message has successfully been received
            ASCIIEncoding encoder = new ASCIIEncoding();

            // Convert the Bytes received to a string and display it on the Server Screen
             msg = encoder.GetString(message, 0, bytesRead);
           // WriteMessage(msg);
             splitData = msg.Split(',');

             print("heel val" + splitData[0]);



             

            print("Message : " + msg);
            // Now Echo the message back
           Echo(msg, encoder, clientStream);
        
           
        }
     
        tcpClient.Close();
    }

    private void Echo(string msg, ASCIIEncoding encoder, NetworkStream clientStream)
    {
        // Now Echo the message back
        byte[] buffer = encoder.GetBytes(msg);

        clientStream.Write(buffer, 0, buffer.Length);

        clientStream.Flush();
    }

    //private void WriteMessage(string msg)
    //{
    //    if (this.txtMsg.)
    //    {
    //        WriteMessageDelegate d = new WriteMessageDelegate(WriteMessage);
    //        //this.txtMsg.Invoke(d, new object[] { msg });
    //    }
    //    else
    //    {
    //        messageToDisplay += msg + Environment.NewLine;
    //    }
    //}
	
	// Update is called once per frame
    

	void Update () {

        ShipTransform();
       // txtMsg.text = messageToDisplay;
	}

    public void loadApp()
    {

        
        //System.Diagnostics.Process.Start("D:/PROJECT/2017/SHIP STABILITY/ShipStability_COT_VS2012_06/bin/Release/ShipStability.exe");
        System.Diagnostics.Process.Start("D:/BP2IP_Padang_Ship_Stability/tanker/ShipStability.exe");
    }
}
