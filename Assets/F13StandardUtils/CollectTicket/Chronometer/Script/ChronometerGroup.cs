using UnityEngine;

public enum ChronometerType
{
    Progress,
    Dialog,
    Order,
    Clean
}

public class ChronometerGroup : BaseEnumObject<ChronometerType,Chronometer>
{

}
