using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Utility;

[UseNetBehaviour]
public class PlayerPlane : FrameBehaviour {
    public Color fullHealthColor = Color.green;
    public Color noHealthColor = Color.red;
    public Transform body;
    public Image hpProgress;
    public float maxHealth
    {
        get;
        private set;
    }
    public float currentHealth
    {
        get;
        private set;
    }
    public MovingState movingState
    {
        get;
        private set;
    }
    UdpNetBehaviour network;
    bool IsMine;
    float lastShootTime;
    public int group
    {
        get;
        private set;
    }
    protected override void OnInit()
    {
        base.OnInit();
        currentHealth = maxHealth = 100;
        SetHpProgress();
        network = this.GetUdpNetwork();
        IsMine = (network.ownerIndex == UserInfo.Instance.Index);
        int index = FrameController.Instance.ListIndexOfPlayer(network.ownerIndex);
        TerrainManager.Instance.SetPos(index, transform);
        group = index % 2;
        if (group == 1) {
            body.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        lastShootTime = 0;
        movingState = MovingState.Idle;
    }

    public Rect GetRect {
        get {
            RectTransform rectTrans = transform as RectTransform;
            var localPos = rectTrans.localPosition;
            var rect = rectTrans.rect;
            rect.center = localPos;
            return rect;
        }
    }

    [UdpRpc]
    public void StopMove()
    {
        this.movingState = MovingState.Idle;
    }

    [UdpRpc]
    public void StartMoveUp()
    {
        this.movingState = MovingState.Up;
    }

    [UdpRpc]
    public void StartMoveDown()
    {
        this.movingState = MovingState.Down;
    }

    [UdpRpc]
    public void Shoot() {
        var go = GameObject.Instantiate(Resources.Load("Bullet")) as GameObject;
        var bullet = go.GetComponent<PlaneBullet>();
        bullet.owner = this;
        bullet.Init();
        if (group == 0)
        {
            bullet.dir = Vector3.right;
        }
        else {
            bullet.dir = Vector3.left;
        }
    }

    public void GotDamage(float damage)
    {
        this.currentHealth -= damage;
        if (this.currentHealth <= 0)
        {
            this.currentHealth = 0;
            this.Die();
        }
        SetHpProgress();
    }

    void SetHpProgress()
    {
        float value = hpProgress.fillAmount = currentHealth / maxHealth;
        hpProgress.color = Color.Lerp(noHealthColor, fullHealthColor, value);
    }
    protected void Die()
    { 

    }

    public override void FrameUpdate()
    {
        switch(this.movingState){
            case MovingState.Down:
                this.transform.localPosition += Vector3.down * 2;
                break;
            case MovingState.Up:
                this.transform.localPosition += Vector3.up * 2;
                break;
        }
        if (IsMine) {
            if (FrameController.Instance.Frame < FrameController.ExecuteFrame)
                return;
            int exeFrame = FrameController.Instance.GetExecuteFrame;
            if (exeFrame == UdpNetManager.Instance.FutureFrame)
                return;
            if (this.movingState != InputManager.Instance.movingState)
            {
                switch (InputManager.Instance.movingState)
                {
                    case MovingState.Idle:
                        network.RPC("StopMove", RPCMode.All);
                        break;
                    case MovingState.Up:
                        network.RPC("StartMoveUp", RPCMode.All);
                        break;
                    case MovingState.Down:
                        network.RPC("StartMoveDown", RPCMode.All);
                        break;
                }
            }else { 
                if(InputManager.Instance.IsShotting){
                    if (FrameController.Instance.CurrentTime - lastShootTime > 0.2f)
                    {
                        lastShootTime = FrameController.Instance.CurrentTime;
                        network.RPC("Shoot", RPCMode.All);
                    }
                }
            }
        }
    }
}
