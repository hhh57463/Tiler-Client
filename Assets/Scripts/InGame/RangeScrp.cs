using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeScrp : MonoBehaviour
{
    public Unit UnitSc = null;

    [SerializeField]
    private Transform[] MoveRangeTr = new Transform[6];                                                             //0: �»�, 1: ���, 2: ��, 3: ��, 4: ����, 5: ����
    [SerializeField]
    private Transform[] AttackRangeTr = new Transform[6];                                                            //0: �»�, 1: ���, 2: ��, 3: ��, 4: ����, 5: ����

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveRange();
        AttackRange();
    }

    /**
   * @brief �̵� ���� ���
   */
    void MoveRange()
    {
        //�Ϲ����� Ÿ�ϵ�
        if ((UnitSc.NowTile.PosX - 1) >= 0 && (UnitSc.NowTile.PosY + 1) <= 19)                     //�»�
        {
            MoveRangeTr[0].position = GameMng.I.mapTile[UnitSc.NowTile.PosX - 1, UnitSc.NowTile.PosY + 1].transform.position;
        }
        if ((UnitSc.NowTile.PosX - 1) >= 0 && (UnitSc.NowTile.PosY - 1) >= 0)                     //����
        {
            MoveRangeTr[4].position = GameMng.I.mapTile[UnitSc.NowTile.PosX - 1, UnitSc.NowTile.PosY - 1].transform.position;
        }
        if ((UnitSc.NowTile.PosX + 1) <= 4 && (UnitSc.NowTile.PosY + 1) <= 19)                     //���
        {
            MoveRangeTr[1].position = GameMng.I.mapTile[UnitSc.NowTile.PosX + 1, UnitSc.NowTile.PosY + 1].transform.position;
        }
        if ((UnitSc.NowTile.PosX + 1) <= 4 && (UnitSc.NowTile.PosY - 1) >= 0)                     //����
        {
            MoveRangeTr[5].position = GameMng.I.mapTile[UnitSc.NowTile.PosX + 1, UnitSc.NowTile.PosY - 1].transform.position;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if ((UnitSc.NowTile.PosY % 2) == 1)                                                                             //Ÿ���� PosY�� Ȧ���϶�
        {
            if ((UnitSc.NowTile.PosX - 1) >= 0 && (UnitSc.NowTile.PosY + 1) <= 19)                     //�»�
            {
                MoveRangeTr[0].position = GameMng.I.mapTile[UnitSc.NowTile.PosX, UnitSc.NowTile.PosY + 1].transform.position;
            }
            if ((UnitSc.NowTile.PosX - 1) >= 0 && (UnitSc.NowTile.PosY - 1) >= 0)                     //����
            {
                MoveRangeTr[4].position = GameMng.I.mapTile[UnitSc.NowTile.PosX, UnitSc.NowTile.PosY - 1].transform.position;
            }
            if (UnitSc.NowTile.PosX == 0)
            {
                if (UnitSc.NowTile.PosY != 19)
                    MoveRangeTr[0].position = GameMng.I.mapTile[UnitSc.NowTile.PosX, UnitSc.NowTile.PosY + 1].transform.position;
                MoveRangeTr[4].position = GameMng.I.mapTile[UnitSc.NowTile.PosX, UnitSc.NowTile.PosY - 1].transform.position;
            }

        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        else                                                                                                              //Ÿ���� PosY�� ¦���϶�
        {
            if ((UnitSc.NowTile.PosX + 1) <= 4 && (UnitSc.NowTile.PosY + 1) <= 19)                     //���
            {
                MoveRangeTr[1].position = GameMng.I.mapTile[UnitSc.NowTile.PosX, UnitSc.NowTile.PosY + 1].transform.position;
            }
            if ((UnitSc.NowTile.PosX + 1) <= 4 && (UnitSc.NowTile.PosY - 1) >= 0)                     //����
            {
                MoveRangeTr[5].position = GameMng.I.mapTile[UnitSc.NowTile.PosX, UnitSc.NowTile.PosY - 1].transform.position;
            }
            if (UnitSc.NowTile.PosX == 4)
            {
                MoveRangeTr[1].position = GameMng.I.mapTile[UnitSc.NowTile.PosX, UnitSc.NowTile.PosY + 1].transform.position;
                if (UnitSc.NowTile.PosY != 0)
                    MoveRangeTr[5].position = GameMng.I.mapTile[UnitSc.NowTile.PosX, UnitSc.NowTile.PosY - 1].transform.position;
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if ((UnitSc.NowTile.PosX - 1) >= 0)                                                         //��
        {
            MoveRangeTr[2].position = GameMng.I.mapTile[UnitSc.NowTile.PosX - 1, UnitSc.NowTile.PosY].transform.position;
        }
        if ((UnitSc.NowTile.PosX + 1) <= 4)                                                        //��
        {
            MoveRangeTr[3].position = GameMng.I.mapTile[UnitSc.NowTile.PosX + 1, UnitSc.NowTile.PosY].transform.position;
        }

        if (!UnitSc.bCharMove)                                                                   //�̵��� ����Ǹ�
        {
            for (int i = 0; i < MoveRangeTr.Length; i++)
            {
                MoveRangeTr[i].transform.localPosition = Vector2.zero;                           //����Ÿ�� ��ġ �ʱ�ȭ
            }
        }
    }
    /**
* @brief ���� ���� ���
*/
    void AttackRange()
    {
        
    }

}