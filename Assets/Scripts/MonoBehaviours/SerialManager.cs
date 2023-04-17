using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Text.RegularExpressions;

public class SerialManager : MonoBehaviour
{

    public static SerialManager sharedInstance = null;
    public string _portName = "COM5";

    public float x;
    public float y;
    public float buttonStateDash;
    public float buttonStateAttack;

    SerialPort serialPort;

    void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            sharedInstance = this;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        // Set the serial port settings
        serialPort = new SerialPort();
        serialPort.PortName = _portName;           // Change this to your serial port name
        serialPort.BaudRate = 9600;
        serialPort.Parity = Parity.None;
        serialPort.DataBits = 8;
        serialPort.StopBits = StopBits.One;
        serialPort.Handshake = Handshake.None;
        serialPort.Encoding = System.Text.Encoding.ASCII;
        serialPort.ReadTimeout = 20;

        // Open the serial port
        serialPort.Open();
    }

    // Update is called once per frame
    void Update()
    {
        // Read the serial data
        string serialData = serialPort.ReadLine();

        // Use Regex to extract the integer values for x and y and the boolean value for buttonState
        Match match = Regex.Match(serialData, @"(\d+),(\d+),(\d+),(\d+)");
        if (match.Success)
        {
            x = int.Parse(match.Groups[1].Value);
            y = int.Parse(match.Groups[2].Value);
            buttonStateDash = int.Parse(match.Groups[3].Value);
            buttonStateAttack = int.Parse(match.Groups[4].Value);

            //Normalize value x and y 
            x = Mathf.Round(x / 512 - 1);
            y = -Mathf.Round(y / 512 - 1);


            Debug.Log("x: " + x + ", y: " + y + ", buttonStateDash: " + buttonStateDash + ", buttonStateAttack: " + buttonStateAttack);
        }
    }
}
