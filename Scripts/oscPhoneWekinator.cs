using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

//Receive info from phone, log it and redirect it to Wekinator
public class oscPhoneWekinator : MonoBehaviour {
    
   	public OSC osc;

    public OSC oscReference;

    float gravityX, gravityY, gravityZ = 0;
    float linearX, linearY, linearZ = 0;

    //Info CSV
    private SaveData save;
    private float elapsed;

	void Start () {

        //CSV
        int numberOfColumns = 7;
		string[] headerRow = new string[]{"ElapsedTime", 
                                          "GravityX", "GravityY", "GravityZ", 
                                          "LinearX", "LinearY","LinearZ"};

		save = new SaveData(headerRow, numberOfColumns, "PhoneInput");
      
        //Gravity
        osc.SetAddressHandler("/accelerometer/gravity/x", OnReceiveGravityX);
        osc.SetAddressHandler("/accelerometer/gravity/y", OnReceiveGravityY);
        osc.SetAddressHandler("/accelerometer/gravity/z", OnReceiveGravityZ);

        //Linear Acceleration
        osc.SetAddressHandler("/accelerometer/linear/x", OnReceiveLinearX);
        osc.SetAddressHandler("/accelerometer/linear/y", OnReceiveLinearY);
        osc.SetAddressHandler("/accelerometer/linear/z", OnReceiveLinearZ);

    }

    

    public float[] getAllOSCValues(){
        return new float[]{gravityX, gravityY, gravityZ, linearX, linearY, linearZ};
    }

    void Update(){

        sendMessage("/unity/accelerometer");

        

        elapsed += Time.deltaTime;
        save.addRow(new string[]{elapsed.ToString(),
                                 gravityX.ToString(), gravityY.ToString(), gravityZ.ToString(),
                                 linearX.ToString(), linearY.ToString(), linearZ.ToString() });

    }
	
	void FixedUpdate () {
    
	}

    private void LateUpdate() {
        if (Input.GetKey("g")) save.exportCSV();
    }

    void sendMessage(string address){
        OscMessage message = new OscMessage();
        message.address = address;
        message.values.Add(gravityX);
        message.values.Add(gravityY);
        message.values.Add(gravityZ);
        message.values.Add(linearX);
        message.values.Add(linearY);
        message.values.Add(linearZ);
        oscReference.Send(message);
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
