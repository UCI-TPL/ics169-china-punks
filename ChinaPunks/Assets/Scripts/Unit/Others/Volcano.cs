using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : Unit {

    public int SprayCD;
    int turn;
    public GameObject fire_prefab;
    public bool fire_moving;

    public GameObject fire1;
    public GameObject fire2;

    public GameObject BGCurve1;
    public GameObject BGCurve2;


    void Update()
    {
        //if(fire_moving){
        //    if (!fire1.GetComponent<Fire>().fire_moving && !fire2.GetComponent<Fire>().fire_moving)
        //        fire_moving = false;
        //}

        if(fire1 == null && fire2 == null){
            fire_moving = false;
        }

    }

    public void Spray(){
        List<int> allowed_tiles = new List<int>();
        for (int i = 0; i < mc.map_size * mc.map_size;++i){
            if(mc.units_state[i] == null
               && mc.map_tiles[i].GetComponent<Tile>().tile_type != "Snow"
               && mc.map_tiles[i].GetComponent<Tile>().tile_type != "Muddy"
              ){
                allowed_tiles.Add(i);
            }
        }

        int index1 = Random.Range(0, allowed_tiles.Count);
        int f1 = allowed_tiles[index1];
        allowed_tiles.Remove(f1);
        int index2 = Random.Range(0, allowed_tiles.Count);
        int f2 = allowed_tiles[index2];

        mc.map_tiles[f1].GetComponent<Tile>().on_fire = true;
        mc.map_tiles[f2].GetComponent<Tile>().on_fire = true;


        fire1 = Instantiate(fire_prefab,transform.position,transform.rotation);
        fire1.GetComponent<Fire>().pos = f1;

        fire2 = Instantiate(fire_prefab, transform.position, transform.rotation);
        fire2.GetComponent<Fire>().pos = f2;

        
        fire1.GetComponent<Fire>().fire_move();
        fire2.GetComponent<Fire>().fire_move();
        fire_moving = true;

        BGCurve1.GetComponent<BansheeGz.BGSpline.Components.BGCcTrs>().ObjectToManipulate = fire1.transform;
        BGCurve2.GetComponent<BansheeGz.BGSpline.Components.BGCcTrs>().ObjectToManipulate = fire2.transform;


        //move f1
        Vector3 xyPosition = mc.map_tiles[f1].transform.position;
        Vector3 end = new Vector3(xyPosition.x, xyPosition.y + 0.7f, xyPosition.z - 1.0f);
        Vector3 start = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        BansheeGz.BGSpline.Curve.BGCurvePoint point1 = BGCurve1.GetComponent<BansheeGz.BGSpline.Curve.BGCurve>().CreatePointFromWorldPosition(
            start, (BansheeGz.BGSpline.Curve.BGCurvePoint.ControlTypeEnum)1);
        BansheeGz.BGSpline.Curve.BGCurvePoint point2 = BGCurve1.GetComponent<BansheeGz.BGSpline.Curve.BGCurve>().CreatePointFromWorldPosition(
            end, (BansheeGz.BGSpline.Curve.BGCurvePoint.ControlTypeEnum)1);


        point1.ControlFirstLocal = new Vector3(0f, 0f);
        point1.ControlSecondLocal = new Vector3(0f, 0f);
        if (start.x > end.x)
        {
            point2.ControlFirstLocal = new Vector3(0.5f, 4f);
            point2.ControlSecondLocal = new Vector3(-0.5f, -4f);
        }
        else
        {
            point2.ControlFirstLocal = new Vector3(-0.5f, 4f);
            point2.ControlSecondLocal = new Vector3(0.5f, -4f);
        }
        BGCurve1.GetComponent<BansheeGz.BGSpline.Curve.BGCurve>().Clear();
        BGCurve1.GetComponent<BansheeGz.BGSpline.Curve.BGCurve>().AddPoint(point1);
        BGCurve1.GetComponent<BansheeGz.BGSpline.Curve.BGCurve>().AddPoint(point2);

        BGCurve1.GetComponent<BansheeGz.BGSpline.Components.BGCcTrs>().Distance = 0;


        //move f2
        Vector3 xyPosition2 = mc.map_tiles[f2].transform.position;
        Vector3 end2 = new Vector3(xyPosition2.x, xyPosition2.y + 0.7f, xyPosition2.z - 1.0f);
        Vector3 start2 = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        BansheeGz.BGSpline.Curve.BGCurvePoint point3 = BGCurve2.GetComponent<BansheeGz.BGSpline.Curve.BGCurve>().CreatePointFromWorldPosition(
            start2, (BansheeGz.BGSpline.Curve.BGCurvePoint.ControlTypeEnum)1);
        BansheeGz.BGSpline.Curve.BGCurvePoint point4 = BGCurve2.GetComponent<BansheeGz.BGSpline.Curve.BGCurve>().CreatePointFromWorldPosition(
            end2, (BansheeGz.BGSpline.Curve.BGCurvePoint.ControlTypeEnum)1);


        point3.ControlFirstLocal = new Vector3(0f, 0f);
        point3.ControlSecondLocal = new Vector3(0f, 0f);
        if (start2.x > end2.x)
        {
            point4.ControlFirstLocal = new Vector3(0.5f, 4f);
            point4.ControlSecondLocal = new Vector3(-0.5f, -4f);
        }
        else
        {
            point4.ControlFirstLocal = new Vector3(-0.5f, 4f);
            point4.ControlSecondLocal = new Vector3(0.5f, -4f);
        }
        BGCurve2.GetComponent<BansheeGz.BGSpline.Curve.BGCurve>().Clear();
        BGCurve2.GetComponent<BansheeGz.BGSpline.Curve.BGCurve>().AddPoint(point3);
        BGCurve2.GetComponent<BansheeGz.BGSpline.Curve.BGCurve>().AddPoint(point4);

        BGCurve2.GetComponent<BansheeGz.BGSpline.Components.BGCcTrs>().Distance = 0;


    }

}
