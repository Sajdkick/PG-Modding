using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PythonTest : MonoBehaviour {

    private PythonEnvironment m_pyEnv;
    private string m_pyCode;

    private const string INITIALIZATION_CODE =
    @"
    import clr
    clr.AddReference('UnityEngine')
    import UnityEngine
    ";

    float timer = 0;

    List<GameObject> spheres;
    PythonInput input;

    // Use this for initialization
    void Start () {

        spheres = new List<GameObject>();

        m_pyEnv = new PythonEnvironment();
        m_pyEnv.RunCommand(INITIALIZATION_CODE);
        m_pyEnv.RunCommand("from UnityEngine import GameObject, Vector3, PrimitiveType, Mathf");

        PythonEnvironment.CommandResult r = m_pyEnv.RunCommand("from json import decoder");
        if (r.exception != null)
            Debug.Log(r.exception.ToString());

        m_pyEnv.ExposeVariable("Behavior", typeof(PyTest.PyBehaviorBase));

        spheres.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere));

        m_pyEnv.ExposeVariable("timer", timer);

        m_pyCode = System.IO.File.ReadAllText("Test.py");

        input = new PythonInput();

        input.index = new int[20];
        for (int i = 0; i < input.index.Length; i++)
            input.index[i] = i;

        input.timer = new float[20];
        for (int i = 0; i < input.index.Length; i++)
            input.timer[i] = i * 1f;

        string json = JsonUtility.ToJson(input);

    }

    // Update is called once per frame
    void Update () {

        for(int i = 0; i < 1; i++)
        {

            m_pyEnv.ExposeVariable("input", JsonUtility.ToJson(input));

            PythonEnvironment.CommandResult r = m_pyEnv.RunCommand(m_pyCode);



            if(r.exception != null)
                Debug.Log(r.exception.ToString());

        }
	}
}
