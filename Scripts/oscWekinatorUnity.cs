using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;


public class oscWekinatorUnity : MonoBehaviour {
    
   	public OSC osc;

    public float matchTreshold = 20;
    
    private float c1,c2,c3,c4;
    private float bestClass;

    //Making sure it's well classified
    private List<int> classValues = new List<int>();
    private int countVal = 0;
    private bool certeza = false;
    private int classeAtual = -1;

    //Info CSV
    private SaveData save;
    private float elapsed;

	void Start () {
      
        //Gravity
        osc.SetAddressHandler("/wek/outputs", OnReceiveOutputs);
        

        int numberOfColumns = 6;
		string[] headerRow = new string[]{"ElapsedTime",
                                          "Class1", "Class2", "Class3","Class4",
                                          "BestClass"};
		save = new SaveData(headerRow, numberOfColumns, "WekinatorOutput");
    }

    void Update(){
        //Debug.Log("("+c1+","+c2+","+c3+","+c4+")"); 
        elapsed += Time.deltaTime;
        save.addRow(new string[]{elapsed.ToString(),
                                 c1.ToString(), c2.ToString(), c3.ToString(), c4.ToString(),
                                 bestClass.ToString()});

    }
	
    GUIStyle guiStyle = new GUIStyle();
	void OnGUI(){
		guiStyle.fontSize = 25;
		guiStyle.normal.textColor = Color.white;
		GUI.BeginGroup(new Rect(10, 10, 250, 150));
		GUI.Box(new Rect(0,0,140,140), "Stats", guiStyle);
		GUI.Label(new Rect(10,25,200,30), string.Format("Time: {0:0.00}", elapsed), guiStyle);
		GUI.Label(new Rect(10,50,200,30), "Value class 1: " + c1, guiStyle);
		GUI.Label(new Rect(10,75,200,30), "Value class 2: " + c2, guiStyle);
        GUI.Label(new Rect(10,100,200,30), "Value class 3: " + c3, guiStyle);
        GUI.Label(new Rect(10,125,200,30), "Value class 4: " + c4, guiStyle);
        GUI.Label(new Rect(10,150,200,30), "Current estimation: " + bestClass, guiStyle);
		GUI.Label(new Rect(10,175,200,30), "Press G to save", guiStyle);
		GUI.EndGroup();
	}


	void FixedUpdate () {
    
	}

    private void LateUpdate() {
        if (Input.GetKey("g")) save.exportCSV();
    }

    public int Classify(float[] outputs){

        float minVal = Mathf.Min(outputs);
        int minIndex = System.Array.IndexOf(outputs, minVal);

        Debug.Log("minVal: " + minVal);
        Debug.Log("minIndex: " + minIndex);
        //Debug.Log("matchThreshold:" + matchTreshold);
        //Debug.Log(minVal <= matchTreshold);

        if(minIndex >= 0){
            switch (minIndex)
            {
                case(0):
                    Debug.Log("Está parado. ");
                    return 0;
                case(1):
                    Debug.Log("Está a fazer flexoes.");
                    return 1;
                case(2):
                    Debug.Log("Está a fazer tesouras.");
                    return 2;
                case(3):
                    Debug.Log("Está a fazer agachamento.");
                    return 3;
            }
        }

        return -1;
    }

    // 1 -> Flexão
    // 2 -> Burpee
    // 3 -> Agachamento

    //Check if value is infiny, if it is transform into -1. For plotting reasons.
    private float checkIfInfinity(float c){
        if(c == float.MaxValue){
            return -1.0f;
        }
        return c;
    }

    void OnReceiveOutputs(OscMessage message) {
        c1 = checkIfInfinity(message.GetFloat(0));
        c2 = checkIfInfinity(message.GetFloat(1));
        c3 = checkIfInfinity(message.GetFloat(2));
        c4 = checkIfInfinity(message.GetFloat(3));
        
        

        bestClass = Classify(new float[]{c1,c2,c3,c4}) ;

        Debug.Log("countVal: " + countVal);
        Debug.Log("certeza : " + certeza);
        Debug.Log("classeAtual: " + classeAtual);

        
    }

    
    

    void ConsecutiveValue(int classVal){
        if(classValues.Count == 0) classValues.Add(classVal);
        if(classValues[classValues.Count -1] == classVal) countVal += 1;
        if(countVal > 10){
            //dar reset ao countVal quando se usar a variavel para dar trigger à animação
            certeza = true;
            classeAtual = classVal;
        }

    }
    

}
