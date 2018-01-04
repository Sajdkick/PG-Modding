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
    // Use this for initialization
    void Start () {

        m_pyEnv = new PythonEnvironment();
        m_pyEnv.RunCommand(INITIALIZATION_CODE);
        m_pyEnv.RunCommand("from UnityEngine import GameObject, Vector3, PrimitiveType, Mathf");
        m_pyEnv.ExposeVariable("Behavior", typeof(PyTest.PyBehaviorBase));

        for (int i = 0; i < 20; i++)
            m_pyEnv.ExposeVariable("sphere" + i, GameObject.CreatePrimitive(PrimitiveType.Sphere));

        m_pyEnv.ExposeVariable("timer", timer);

        m_pyCode = System.IO.File.ReadAllText("Test.py");

    }

// Update is called once per frame
void Update () {

        for(int i = 0; i < 1; i++)
        {

            timer += Time.deltaTime;
            m_pyEnv.ExposeVariable("timer", timer);
            PythonEnvironment.CommandResult r = m_pyEnv.RunCommand(m_pyCode);

            if(r.exception != null)
                Debug.Log(r.exception.ToString());

        }
	}
}
