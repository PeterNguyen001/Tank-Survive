using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TankPart : MonoBehaviour
{
    protected TankPartData tankPart;
    private bool isDisable;

    [SerializeField]
    private float HP;
    public bool IsDisable { get => isDisable; set => isDisable = value; }

    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        // Initialization logic for TankPart can go here
    }
    void Start()
    {
        HP = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
    }

    public TankPartData GetTankPart() { return tankPart; }
}
