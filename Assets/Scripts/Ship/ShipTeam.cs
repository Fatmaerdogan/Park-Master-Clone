using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTeam : MonoBehaviour
{
    public ColorType colorType;
    [SerializeField] private int teamId;
    [SerializeField] private List<Color>colors = new List<Color>();
    public Ship ship;
    public FinishPoint finishPoint;
    public StartPoint startPoint;
    public Path path;

    public int TeamId
    {
        get { return teamId; }
    }

    private void Start()
    {
        path.MaterialCreat(colors[(int)colorType]);
        finishPoint.MaterialCreat(colors[(int)colorType]);
        ship.MaterialCreat(colors[(int)colorType]);

        path.TeamId= finishPoint.TeamId=startPoint.TeamId = ship.TeamId = teamId;
    }
}
