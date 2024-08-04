using UnityEngine;
using System.IO.Ports;
using TMPro;

public class SerialReader : MonoBehaviour // forsøk på seriell kobling med arduino uno. For dårlig kopling til å behalde flyt i spelet, men interessant konsept
{
    SerialPort stream;
    public string receivedString;

    public float sensor1Value;
    public string status1;


    public float sensor2Value;
    public string status2;

    public TextMeshProUGUI sensor1Text; 

    public TextMeshProUGUI sensor2Text; 

    void Start()
    {
        string portName = "/dev/cu.usbmodem2101"; //arduino port
        int baudRate = 9600;

        try
        {
            stream = new SerialPort(portName, baudRate);
            stream.Open(); 
            stream.ReadTimeout = 50; 
            //Debug.Log("Serial port opened successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error opening serial port {portName}: {e.Message}");
        }
    }

    void Update()
    {
        if (stream != null && stream.IsOpen)
        {
            try
            {
                receivedString = stream.ReadLine(); 
                Debug.Log(receivedString); 
                ParseData(receivedString); // Parse and display the data
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Error reading from serial port: {e.Message}");
            }
        }
    }

    void ParseData(string data)
    {
        //Data format: "Sensor1: value, Status1: status, Sensor2: value, Status2: status"
        string[] parts = data.Split(',');

        if (parts.Length == 4)
        {
            sensor1Value = float.Parse(parts[0].Split(':')[1].Trim());
            status1 = parts[1].Split(':')[1].Trim();
            sensor2Value = float.Parse(parts[2].Split(':')[1].Trim());
            status2 = parts[3].Split(':')[1].Trim();

            // oppdater ui element
            sensor1Text.text = $"Sensor 1 - Moisture Level: {sensor1Value}, Status: {status1}";
            sensor2Text.text = $"Sensor 2 - Moisture Level: {sensor2Value}, Status: {status2}";

            Debug.Log($"Sensor1 Value: {sensor1Value}, Status1: {status1}");
            Debug.Log($"Sensor2 Value: {sensor2Value}, Status2: {status2}");
        }
    }

    void OnDestroy()
    {
        if (stream != null && stream.IsOpen)
        {
            stream.Close(); // Close the port when the application ends
            Debug.Log("Serial port closed.");
        }
    }
}
