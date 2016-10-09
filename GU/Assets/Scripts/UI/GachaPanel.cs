using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GachaPanel : UIBasePanel {

    public GameObject NormalBtn, HighBtn, SpecialBtn;
    const int NORMAL_GACHA_GOLD = 1000;
    const int HIGH_GACHA_GOLD = 5000;
    const int SPECIAL_GACHA_GOLD = 10000;


    public override void Init()
    {
        base.Init();

        UIEventListener.Get(NormalBtn).onClick = (sender) =>
        {
            List<int> UnitIdList = new List<int>() {
                101, 104, 107,
                201, 204, 207,
                301, 304, 307
            };

            // 골드 부족
            if (GameManager.Instance.Gold < NORMAL_GACHA_GOLD)
            {
                UIManager.Instance.OpenMessagePopup("골드가 부족 합니다.", delegate () {

                });
            }
            else
            {
                UIManager.Instance.OpenMessagePopup("1000골드를 사용해서 기본뽑기를 하시겠습니까?", delegate () {
                    // 리스트에 있는 유닛중에 랜덤 뽑기
                    int UnitId = UnitIdList[Random.Range(0, UnitIdList.Count)];
                    StartCoroutine(SqliteManager.Instance.RequestGetUnit(UnitId));
                    StartCoroutine(SqliteManager.Instance.RequestLoadMyUnit());
                    StartCoroutine(SqliteManager.Instance.RequestUpdateUserData(4, GameManager.Instance.Gold - NORMAL_GACHA_GOLD));
                }, delegate() { }, "뽑기", "취소");
            }
        };

        UIEventListener.Get(HighBtn).onClick = (sender) =>
        {
            List<int> UnitIdList = new List<int>() {
                101, 102, 104, 105, 107, 108,
                201, 202, 204, 205, 207, 208,
                301, 302, 304, 305, 307, 308
            };

            if (GameManager.Instance.Gold < HIGH_GACHA_GOLD)
            {
                UIManager.Instance.OpenMessagePopup("골드가 부족 합니다.", delegate ()
                {

                });
            }
            else
            {
                UIManager.Instance.OpenMessagePopup("5000골드를 사용해서 고급뽑기를 하시겠습니까?", delegate () {
                    // 리스트에 있는 유닛중에 랜덤 뽑기
                    int UnitId = UnitIdList[Random.Range(0, UnitIdList.Count)];
                    StartCoroutine(SqliteManager.Instance.RequestGetUnit(UnitId));
                    StartCoroutine(SqliteManager.Instance.RequestLoadMyUnit());
                    StartCoroutine(SqliteManager.Instance.RequestUpdateUserData(4, GameManager.Instance.Gold - HIGH_GACHA_GOLD));
                }, delegate () { }, "뽑기", "취소");
            }
        };

        UIEventListener.Get(SpecialBtn).onClick = (sender) =>
        {
            List<int> UnitIdList = new List<int>() {
                102, 103, 105, 106, 108, 109, 110,
                202, 203, 205, 206, 208, 209, 210,
                302, 303, 305, 306, 308, 309, 310
            };

            if (GameManager.Instance.Gold < SPECIAL_GACHA_GOLD)
            {
                UIManager.Instance.OpenMessagePopup("골드가 부족 합니다.", delegate ()
                {

                });
            }
            else
            {
                UIManager.Instance.OpenMessagePopup("10000골드를 사용해서 스페셜뽑기를 하시겠습니까?", delegate () {
                    // 리스트에 있는 유닛중에 랜덤 뽑기
                    int UnitId = UnitIdList[Random.Range(0, UnitIdList.Count)];
                    StartCoroutine(SqliteManager.Instance.RequestGetUnit(UnitId));
                    StartCoroutine(SqliteManager.Instance.RequestLoadMyUnit());
                    StartCoroutine(SqliteManager.Instance.RequestUpdateUserData(4, GameManager.Instance.Gold - SPECIAL_GACHA_GOLD));
                }, delegate () { }, "뽑기", "취소");
            }
        };
    }

    public override void LateInit()
    {
        base.LateInit();
    }
}
