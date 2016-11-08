using UnityEngine;
using System.Collections;

public class FloorController : MonoBehaviour
{

    /*
    C# Interface for the JS controller FloorControllerJS.js
    */
    private static FloorController _instance;
    private FloorControllerJS JSController;

    public static FloorController Moving
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<FloorController>();
            return _instance;
        }
    }

    void Awake()
    {
        JSController = this.GetComponent<FloorControllerJS>();
    }

    void Start()
    {
        JSController.Start();
    }

    void Update()
    {
        JSController.Update();
    }

    public void OnDisable()
    {
        JSController.OnDisable();
    }

    public void OnGUI()
    {
        JSController.OnGUI();
    }

    public void enable()
    {
        JSController.enable();
    }

    public void disable()
    {
        JSController.disable();
    }

    public void resetFloor()
    {
        JSController.resetFloor();
    }

    public float getVoltage(int index)
    {
        return JSController.getVoltage(index);
    }

    public void lowerFloor()
    {
        JSController.lowerFloor();
    }

    public void moveOne(int index, float voltage)
    {
        JSController.moveOne(index, voltage);
    }

    public void moveAll(float voltage)
    {
        JSController.moveAll(voltage);
    }

    public void raiseFront(float voltage)
    {
        JSController.raiseFront(voltage);
    }

    public void raiseBack(float voltage)
    {
        JSController.raiseBack(voltage);
    }

    public void raiseRight(float voltage)
    {
        JSController.raiseRight(voltage);
    }

    public void raiseLeft(float voltage)
    {
        JSController.raiseLeft(voltage);
    }
}
