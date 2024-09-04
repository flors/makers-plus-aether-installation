using System.Collections;
using UnityEngine;
using System;


public class ArduinoManager : Singleton<ArduinoManager>
{

    private bool receiveDataEnabled;

    private bool sensorAOn;
    private bool sensorBOn;

    public SerialController serialController;


    public void SetupArduinoManager()
    {
        Debug.Log("Serial Arduino --> " + serialController.portName);
        serialController.ConnectToArduino();

        receiveDataEnabled = false;

        sensorAOn = false;
        sensorBOn = false;

    }



    public void StartReceivingData()
    {
        receiveDataEnabled = true;
    }

    public void StoptReceivingData()
    {
        receiveDataEnabled = false;
    }


    public bool IsSensorAOn()
    {
        return sensorAOn;
    }

    public bool IsSensorBOn()
    {
        return sensorBOn;
    }


    private void Update()
    {
        //DEBUG STATE MACHINE
        if (Input.GetKeyDown(KeyCode.A))
            sensorAOn = true;


        if (Input.GetKeyDown(KeyCode.B))
            sensorBOn = true;
            
    }


    //Example send message
    //serialController.SendSerialMessage("A");

    public void SendMessageToArduino(string _message) {

        if (serialController != null)
        {
            Debug.Log("Sending message to Arduino:  " + _message);
            serialController.SendSerialMessage(_message);
        }
      
    }


    public void TurnOffSymbols()
    {
       
        if (serialController != null)
        {
            serialController.SendSerialMessage("0");
        }
    }


    //----------ARDUINO EVENTS------
    // Invoked when a line of data is received from the serial device.
    void OnMessageArrived(string msg)
    {
        if (receiveDataEnabled)
        {
            Debug.Log("[ARDUINO MANAGER] Message arrived: " + msg);
            if (msg.Contains("pulseraA"))
            {
                //string stateStr = msg.Split("/")[1];
                //Debug.Log("--> " + stateStr);

                sensorAOn = msg.Contains("1") ?  true : false;

                Debug.Log("pulseraA is active? " + sensorAOn);
            }

            if (msg.Contains("pulseraB"))
            {
                //string stateStr = msg.Split("/")[1];
                //Debug.Log("--> " + stateStr);

                sensorBOn = msg.Contains("1") ? true : false;

                Debug.Log("pulseraA is active? " + sensorBOn);
            }
        }
        
    }

    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {
        if (success)
            Debug.Log("Connection established");
        else
            Debug.Log("Connection attempt failed or disconnection detected");
    }
    


}
