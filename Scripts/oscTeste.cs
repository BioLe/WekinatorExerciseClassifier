using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

//Receive wekinator info
public class ostTeste : MonoBehaviour {
    
   	public OSC osc;

    public OSC oscReference;

    float gravityX, gravityY, gravityZ = 0;
    float linearX, linearY, linearZ = 0;

    //Info CSV
    private List<string[]> rowData = new List<string[]>();
    private static int rowDataTempSize = 12;
    string[] rowDataTemp;

	void Start () {
      
        //Gravity
        osc.SetAddressHandler("/accelerometer/gravity/x", OnReceiveGravityX);
        osc.SetAddressHandler("/accelerometer/gravity/y", OnReceiveGravityY);
        osc.SetAddressHandler("/accelerometer/gravity/z", OnReceiveGravityZ);

        //Linear Acceleration
        osc.SetAddressHandler("/accelerometer/linear/x", OnReceiveLinearX);
        osc.SetAddressHandler("/accelerometer/linear/y", OnReceiveLinearY);
        osc.SetAddressHandler("/accelerometer/linear/z", OnReceiveLinearZ);

        rowDataTemp = new string[rowDataTempSize];
        //Header csv
        //rowData.Add(rowDataTemp);
    }

    public float[] getAllOSCValues(){
        return new float[]{gravityX, gravityY, gravityZ, linearX, linearY, linearZ};
    }

    void Update(){

        OscMessage message = new OscMessage();
        message.address = "/unity/accelerometer";
        message.values.Add(gravityX);
        //oscReference.Send(message);

        //message.address = "/unity/accelerometer/gravity/y";
        message.values.Add(gravityY);
        //oscReference.Send(message);

        //message.address = "/unity/accelerometer/gravity/z";
        message.values.Add(gravityZ);
        //oscReference.Send(message);

        //message.address = "/unity/accelerometer/linear/x";
        message.values.Add(linearX);
        //oscReference.Send(message);

        //message.address = "/unity/accelerometer/linear/y";
        message.values.Add(linearY);
        //oscReference.Send(message);

        //message.address = "/unity/accelerometer/linear/z";
        message.values.Add(linearZ);
        oscReference.Send(message);


    }
	
	void FixedUpdate () {
    
	}

    private void LateUpdate() {
        //if (Input.GetKey("g")) exportCSV();
    }

    void OnReceiveGravityX(OscMessage message) {
        float x = message.GetFloat(0);
        gravityX = x;
        //Debug.Log("Recebi GravityX: " + x);
    }

    void OnReceiveGravityY(OscMessage message) {
        float y = message.GetFloat(0);
        gravityY = y;
        //Debug.Log("Recebi GravityY: " + y);
    }

    void OnReceiveGravityZ(OscMessage message) {
        float z = message.GetFloat(0);
        gravityZ = z;
        //Debug.Log("Recebi GravityZ: " + z);
    }

    void OnReceiveLinearX(OscMessage message) {
        float x = message.GetFloat(0);
        linearX = x;
        //Debug.Log("Recebi LinearX: " + x);
    }

    void OnReceiveLinearY(OscMessage message) {
        float y = message.GetFloat(0);
        linearY = y;
        //Debug.Log("Recebi LinearY: " + y);
    }

    void OnReceiveLinearZ(OscMessage message) {
        float z = message.GetFloat(0);
        linearZ = z;
        //Debug.Log("Recebi LinearZ: " + z);
    }

}
