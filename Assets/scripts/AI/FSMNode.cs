using UnityEngine;

public class FSMNode : MonoBehaviour {
    public FSMNode[] Nodes;
    public bool IsDoor;
    public bool IsOffice;
    public bool IsOccupied;
    public bool IsColinRunNode;
    public GameObject[] Accepts;
    public Door Door;
    public FSMNode Office;
}
