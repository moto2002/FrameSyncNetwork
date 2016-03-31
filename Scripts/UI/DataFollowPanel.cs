using UnityEngine;
using System.Collections;

public abstract class PanelAttachData<T> where T : PanelAttachData<T>
{
    public System.Action<T> onChange;
    public void FreshPanel()
    {
        if (onChange != null)
        {
            onChange(this as T);
        }
    }
}
public abstract class DataFollowPanel<T> : MonoBehaviour where T : PanelAttachData<T>
{
    protected T data;

    protected virtual void OnDestroy()
    {
        if (data != null) {
            data.onChange -= ShowContent;
        }
    }

    protected abstract void ShowContent(T data);

    public virtual void Load(T data, bool dataFollow = true)
    { 
        if(dataFollow)
            DataFollow(data);
        ShowContent(data);
    }

    void DataFollow(T data)
    {
        if (this.data != data) {
            if (this.data != null)
            {
                this.data.onChange -= ShowContent;
            }
            this.data = data;
            data.onChange += ShowContent;
        }
    }

}
