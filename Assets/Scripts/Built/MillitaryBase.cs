using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillitaryBase : Built
{
    public GameObject CreatingUnitobj = null;

    public static int cost = 10;   // 건설 비용

    public int maintenanceCost = 0;   // 유지 비용

    public int CreatingUnitX = 0;
    public int CreatingUnitY = 0;

    void Awake()
    {
        _name = "군사 기지";
        _max_hp = 10;
        _hp = _max_hp;
        _code = (int)BUILT.MILLITARY_BASE;
        maxCreateCount = 3;
        maintenanceCost = 3;
        _desc = "생성까지 " + (maxCreateCount - createCount) + "턴 남음";
        _emoteSide.color = GetUserColor(_uniqueNumber);

        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void init()
    {
        _activity.Add(ACTIVITY.SOLDIER_0_UNIT_CREATE);
        _activity.Add(ACTIVITY.SOLDIER_1_UNIT_CREATE);
        _activity.Add(ACTIVITY.SOLDIER_2_UNIT_CREATE);
        _activity.Add(ACTIVITY.WITCH_0_UNIT_CREATE);
        _activity.Add(ACTIVITY.WITCH_1_UNIT_CREATE);
        _activity.Add(ACTIVITY.DESTROY_BUILT);
    }

    public void waitingCreate()
    {
        createCount++;
        _desc = "생성까지 " + (maxCreateCount - createCount) + "턴 남음";
        // 2턴 후에 생성됨
        if (createCount > maxCreateCount - 1)
        {
            _desc = "병력들을 생성한다";

            _anim.SetTrigger("isSpawn");

            GameMng.I.RemoveDelegate(this.waitingCreate);

            GameMng.I._hextile.GetCell(SaveX, SaveY)._unitObj.GetComponent<Worker>()._bActAccess = true;
            GameMng.I._hextile.GetCell(SaveX, SaveY)._unitObj.GetComponent<Worker>()._anim.SetBool("isWorking", false);

            if (NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
            {
                init();
                GameMng.I.AddDelegate(maintenance);
            }
        }
    }

    public void maintenance()
    {
        GameMng.I.minGold(maintenanceCost);
    }


    public static void CreateAttackFirstUnitBtn()
    {
        if (GameMng.I.selectedTile._code == (int)BUILT.MILLITARY_BASE)
        {
            GameMng.I._BuiltGM.act = ACTIVITY.SOLDIER_0_UNIT_CREATE;
            GameMng.I._range.moveRange((int)UNIQEDISTANCE.DISTANCE);
        }

    }

    public static void CreateAttackSecondUnitBtn()
    {
        if (GameMng.I.selectedTile._code == (int)BUILT.MILLITARY_BASE)
        {
            GameMng.I._BuiltGM.act = ACTIVITY.SOLDIER_1_UNIT_CREATE;
            GameMng.I._range.moveRange((int)UNIQEDISTANCE.DISTANCE);
        }
    }

    public static void CreateAttackThirdUnitBtn()
    {
        if (GameMng.I.selectedTile._code == (int)BUILT.MILLITARY_BASE)
        {
            GameMng.I._BuiltGM.act = ACTIVITY.SOLDIER_2_UNIT_CREATE;
            GameMng.I._range.moveRange((int)UNIQEDISTANCE.DISTANCE);
        }
    }

    public static void CreateAttackFourthUnitBtn()
    {
        if (GameMng.I.selectedTile._code == (int)BUILT.MILLITARY_BASE)
        {
            GameMng.I._BuiltGM.act = ACTIVITY.WITCH_0_UNIT_CREATE;
            GameMng.I._range.moveRange((int)UNIQEDISTANCE.DISTANCE);
        }
    }

    public static void CreateAttackFifthUnitBtn()
    {
        if (GameMng.I.selectedTile._code == (int)BUILT.MILLITARY_BASE)
        {
            GameMng.I._BuiltGM.act = ACTIVITY.WITCH_1_UNIT_CREATE;
            GameMng.I._range.moveRange((int)UNIQEDISTANCE.DISTANCE);
        }
    }

    void OnDestroy()
    {
        if (createCount < maxCreateCount - 1)
            GameMng.I.RemoveDelegate(waitingCreate);
        else
            GameMng.I.RemoveDelegate(maintenance);

        if (CreatingUnitobj != null)
        {
            Destroy(CreatingUnitobj);
            GameMng.I._hextile.TilecodeClear(CreatingUnitX, CreatingUnitY);
        }
    }
}
