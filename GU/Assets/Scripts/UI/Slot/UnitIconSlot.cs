using UnityEngine;
using System.Collections;

public class UnitIconSlot : MonoBehaviour {

    public GameObject Center, GradeGroup;
    public UISprite Icon, SelectSprite;
    public UILabel NameLbl, LvLbl;

    public UnitDataBase.SlotData SlotData = null;

    public void Init(UnitDataBase.SlotData _data)
    {
        SetSelect(false);
        if (_data == null)
        {
            Center.SetActive(false);
        }
        else
        {
            Center.SetActive(true);
            SlotData = _data;
            DataUpdate();
        }
    }

    public void DataUpdate()
    {
        if (SlotData != null)
        {
            UnitDataBase.Data Unit = DataManager.GetUnitData(SlotData.id);
            if (Unit != null)
            {
                string name = DataManager.GetName(Unit.stringid);
                NameLbl.text = name;
                Icon.spriteName = "icon_" + name;
            }

            LvLbl.text = SlotData.lv.ToString();
            SetGrade();
        }
    }

    void SetGrade()
    {
        for (int i = 0; i < 6; ++i)
            GradeGroup.transform.FindChild("star" + i).gameObject.SetActive(SlotData.grade > i);
    }

    public void SetSelect(bool _bSelect)
    {
        if (SelectSprite != null)
            SelectSprite.gameObject.SetActive(_bSelect);
    }	
}
