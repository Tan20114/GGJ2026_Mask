using UnityEngine;

[CreateAssetMenu(fileName = "Dialog", menuName = "Scriptable Objects/Dialog")]
public class Dialog : ScriptableObject
{
    public string[] speech;
    public string[] abnormal;
}
