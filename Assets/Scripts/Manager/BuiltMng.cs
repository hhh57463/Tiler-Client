using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuiltMng : MonoBehaviour
{
    public ACTIVITY act = ACTIVITY.NONE;

    public GameObject[] unitobj = null;

    [SerializeField]
    private GameObject AirDropobj = null;

    private int nAirDropCount = 0;


    void Update()
    {

        if (Input.GetMouseButtonDown(0) && act != ACTIVITY.ACTING && GameMng.I._UnitGM.act == ACTIVITY.NONE && !EventSystem.current.IsPointerOverGameObject())
        {
            switch (act)
            {
                case ACTIVITY.WORKER_UNIT_CREATE:
                    CreateUnit(Forest_Worker.cost, (int)UNIT.FOREST_WORKER + (int)(NetworkMng.getInstance.myTribe) * 6);
                    break;
                case ACTIVITY.SOLDIER_0_UNIT_CREATE:
                    CreateUnit(Forest_Soldier_0.cost, (int)UNIT.FOREST_SOLDIER_0 + (int)(NetworkMng.getInstance.myTribe) * 6);
                    break;
                case ACTIVITY.SOLDIER_1_UNIT_CREATE:
                    CreateUnit(Forest_Soldier_1.cost, (int)UNIT.FOREST_SOLDIER_1 + (int)(NetworkMng.getInstance.myTribe) * 6);
                    break;
            }
            GameMng.I._range.SelectTileSetting(true);
        }


        if (Input.GetMouseButtonDown(0) && GameMng.I._UnitGM.act == ACTIVITY.NONE && act == ACTIVITY.NONE && !EventSystem.current.IsPointerOverGameObject())
        {
            GameMng.I._range.AttackrangeTileReset();                                                     //클릭시 터렛 공격 범위 초기화
            GameMng.I.mouseRaycast();
            if (GameMng.I.selectedTile)
                if (GameMng.I.selectedTile._builtObj != null)
                {
                    if (GameMng.I.selectedTile._code == (int)BUILT.ATTACK_BUILDING)
                    {
                        GameMng.I.selectedTile._builtObj.GetComponent<Turret>().Attack();
                    }
                }
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            CreateAirDrop();
        }
    }

    /**
     * @brief 유닛을 생성함 (클라 전용)
     * @param cost 사용된 코스트
     * @param index 유닛 코드
     */
    public void CreateUnit(int cost, int index)
    {

        GameMng.I.minGold(3);       // TODO : 코스트로 변경
        GameMng.I.mouseRaycast(true);                       //캐릭터 정보와 타일 정보를 알아와야해서 false에서 true로 변경
        if (GameMng.I.targetTile._builtObj == null && GameMng.I.targetTile._code < (int)TILE.CAN_MOVE && GameMng.I.targetTile._unitObj == null && Vector2.Distance(GameMng.I.selectedTile.transform.localPosition, GameMng.I.targetTile.transform.localPosition) <= 1.5f)
        {
            if (GameMng.I._gold >= cost)
            {
                if (index > (int)UNIT.FOREST_WORKER && (int)UNIT.SEA_WORKER > index)
                {
                    GameMng.I.selectedTile._builtObj._bActAccess = true;
                }
                GameObject Child = Instantiate(unitobj[index - 300], GameMng.I.targetTile.transform) as GameObject;                 // enum 값 - 300
                Child.transform.parent = transform.parent;
                GameMng.I.targetTile._unitObj = Child.GetComponent<Unit>();
                GameMng.I.targetTile._code = index;       // 문제는 Awake다
                GameMng.I.targetTile._unitObj._uniqueNumber = NetworkMng.getInstance.uniqueNumber;
                GameMng.I._range.rangeTileReset();
                //act = ACTIVITY.ACTING;
                act = ACTIVITY.NONE;

                NetworkMng.getInstance.SendMsg(string.Format("CREATE_UNIT:{0}:{1}:{2}:{3}", GameMng.I.targetTile.PosX, GameMng.I.targetTile.PosZ, index, NetworkMng.getInstance.uniqueNumber));

                GameMng.I.cleanActList();
                GameMng.I.cleanSelected();

                NetworkMng.getInstance.SendMsg("TURN");
            }
        }
        else                                     // 범위가 아닌 다른 곳을 누름
        {
            act = ACTIVITY.NONE;
            GameMng.I.selectedTile = GameMng.I.targetTile;
            GameMng.I.targetTile = null;
            GameMng.I._range.rangeTileReset();
        }
    }

    /**
     * @brief 유닛을 생성함 (서버에서 클라로 정보를 보낼때 호출됨)
     * @param posX 생성할 X 위치
     * @param posY 생성할 Y 위치
     * @param index 유닛 코드
     * @param uniqueNumber 생성자
     */
    public void CreateUnit(int posX, int posY, int index, int uniqueNumber)
    {
        GameObject Child = Instantiate(unitobj[index - 300], GameMng.I._hextile.GetCell(posX, posY).transform) as GameObject;
        Child.transform.parent = transform.parent;
        GameMng.I._hextile.GetCell(posX, posY)._unitObj = Child.GetComponent<Unit>();
        GameMng.I._hextile.GetCell(posX, posY)._code = GameMng.I._hextile.GetCell(posX, posY)._unitObj._code;
        GameMng.I._hextile.GetCell(posX, posY)._unitObj._uniqueNumber = uniqueNumber;
    }

    /**
     * @brief 보급 생성
     */
    public void CreateAirDrop()
    {
        Debug.Log(nAirDropCount);
        int nPosX, nPosY;
        nPosX = Random.Range(0, GameMng.I.GetMapWidth);
        nPosY = Random.Range(0, GameMng.I.GetMapHeight);
        if (GameMng.I._hextile.GetCell(nPosX, nPosY)._builtObj == null && GameMng.I._hextile.GetCell(nPosX, nPosY)._unitObj == null && GameMng.I._hextile.GetCell(nPosX, nPosY)._code < (int)TILE.CAN_MOVE)
        {
            GameObject Child = Instantiate(AirDropobj, GameMng.I._hextile.GetCell(nPosX, nPosY).transform) as GameObject;
            GameMng.I._hextile.GetCell(nPosX, nPosY)._code = (int)TILE.CAN_MOVE;
            GameMng.I._hextile.GetCell(nPosX, nPosY)._builtObj = Child.GetComponent<AirDrop>();
        }
        else
        {
            if (nAirDropCount < 5)
            {
                Debug.Log("위치 재 선정");
                nAirDropCount++;
                CreateAirDrop();
            }
            else
                nAirDropCount = 0;
        }
        Debug.Log(nPosY + " , " + nPosX);

    }

    /**
     * @brief 건물 파괴될때 호출됨
     */
    public void DestroyBuilt()
    {
        NetworkMng.getInstance.SendMsg(string.Format("DESTROY_BUILT:{0}:{1}", GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosZ));
        GameMng.I.selectedTile._builtObj.DestroyMyself();
        //Destroy(GameMng.I.selectedTile._builtObj.gameObject);
        if (GameMng.I.selectedTile._builtObj._code == (int)BUILT.ATTACK_BUILDING)
        {
            GameMng.I._range.AttackrangeTileReset();
        }
        act = ACTIVITY.NONE;
        GameMng.I.selectedTile._builtObj = null;
        Debug.Log("여기 수정해야함!!!!!");
        GameMng.I.selectedTile._code = (int)TILE.GRASS;                                                             // 나중에 원래 타일 알아오는법 가져오기
        GameMng.I._range.SelectTileSetting(true);
        GameMng.I.cleanActList();
        GameMng.I.cleanSelected();
    }

    /**
     * @brief 건물 파괴될때 호출됨
     */
    public void DestroyBuilt(int posX, int posY)
    {
        GameMng.I._hextile.GetCell(posX, posY)._builtObj.DestroyMyself();
        //Destroy(GameMng.I._hextile.GetCell(posX, posY)._builtObj.gameObject);
        GameMng.I._hextile.GetCell(posX, posY)._builtObj = null;
        GameMng.I._hextile.GetCell(posX, posY)._code = (int)TILE.GRASS;
    }
}
