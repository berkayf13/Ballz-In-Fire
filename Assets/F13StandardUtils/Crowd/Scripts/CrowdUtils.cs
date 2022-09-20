using System;
using System.Linq;
using UnityEngine;

public class CrowdUtils : MonoBehaviour
{
    public static Vector3 RandomNormalizedVector(bool randX=true,bool randY=true,bool randZ=true)
    {
        var randVec = new Vector3(
            randX?UnityEngine.Random.Range(-1f, 1f):0f,
            randY?UnityEngine.Random.Range(-1f, 1f):0f,
            randZ?UnityEngine.Random.Range(-1f, 1f):0f);
        return randVec.normalized;
    }
    
        
    public static Vector3 CalculateCircularPositionWithIndex(int index,float interval=.80f)
    {
        if(index==0) return Vector3.zero;
        float tmp = index;
        int r = 2;
        var squareR = r * r;
        while ((tmp-squareR)>0)
        {
            tmp -= squareR;
            r++;
            squareR = r * r;
        }

        var angle = (tmp/squareR)*360;
        var forward = Vector3.forward*(r-1)*interval;
        var pos=Quaternion.Euler(0, angle, 0) * forward;
        return pos;
    }

    public static Vector3 CalculateFinishPositionWithIndex(int index,
        int totalCount,float maxInterval,int groupCount=2,
        float platformWidth=26,float groupInterval=4f)
    {
        var interval = CalculateIntervalDynamically(totalCount, maxInterval);
        var intervalCount = groupCount - 1;
        var platformActiveWidth = platformWidth - intervalCount * groupInterval;
        var groupActiveWidth = platformActiveWidth / groupCount;
        var groupRowCount = (int)(groupActiveWidth / interval);

        var groupIndex = index % groupCount;
        var inGroupIndex = index / groupCount;
        var groupRowIndex = inGroupIndex / groupRowCount;
        var groupColIndex = inGroupIndex % groupRowCount;

        var odd = groupIndex % 2;
        var xPos = 0f;
        var x =  (groupIndex - odd + 1f) / 2 * (groupInterval + groupActiveWidth);
        xPos += groupIndex % 2 == 0 ? -x : x;
        var smallX = ((groupColIndex+1) / 2) * interval;
        xPos += groupColIndex % 2 == 0 ? -smallX : smallX;
        var zPos = interval * groupRowIndex;
        return Vector3.forward * -zPos +Vector3.right * xPos +Vector3.left*interval*0.5f*((groupRowCount+1)%2);
    }

    public static Vector3 CalculateDynamicRectangularPosition(int index,float ratio,int limitRowCount=4,int originRowCount=12,float interval=1.7f, bool isLerp=false)
    {

        var limitVector = CalculateRectangularPosition(index, ratio, limitRowCount, interval);
        // var originVector = CalculateRectangularPosition(index, ratio, originRowCount, interval);
        var triInter = interval * 0.75f;
        var originVector =  CalculateTriangularPosition(index,triInter,interval);
        Vector3 pos;
        if(isLerp) 
            pos = Vector3.Lerp(originVector, limitVector,Mathf.Abs(ratio));
        else 
            pos = Math.Abs(ratio) > 0.5 ? limitVector : originVector;
        return pos;
    }

    public static Vector3 CalculateRectangularPosition(int index, float ratio, int rowCount, float interval)
    {
        var divide = index / rowCount;
        var mod = index % rowCount;
        var horizonMod = (mod % 2 == 0)  ? Vector3.left : Vector3.right;
        var vertDivide = (divide % 2 == 0)  ? Vector3.forward : Vector3.back;
        var amountMod = (mod + 1) / 2;
        var amounDivide = (divide + 1) / 2;
        var vertVector = horizonMod * amountMod * interval + vertDivide * amounDivide * interval;
        return vertVector;
    }

    public static Vector3 CalculateTriangularPosition(int index, float horInter = 1.7f, float vertInter = 1.7f)
    {
        var degree = 0;
        var temp = index;
        while (temp>0)
        {
            degree++;
            temp -= 2 * degree + 1;
        }

        var y = (Math.Abs(temp)) / 2;
        var degreeIndex = temp + 2 * degree;
        var x = (degreeIndex % 2 == 0 ? -1:1) * (degreeIndex + 1) / 2;
        var pos = Vector3.right * x* horInter+ Vector3.forward * y * vertInter;
        return pos;
    }
    
    
    public static Vector3 CalculateStarPosition(int index, float horInter = 1.7f, float vertInter = 1.7f)
    {
        var degree = 0;
        var temp = index;
        var lineCount = 1;
        while (temp>0)
        {
            degree++;
            lineCount = (2 * degree + 1);
            temp -= 6*lineCount;
        }
        temp+=6*lineCount;
        var triangleOrder = temp / lineCount;
        var order = temp % lineCount;
        
        
        var pos = (Vector3.right * lineCount+Vector3.left*order)* horInter+ Vector3.forward * degree * vertInter;
        return Quaternion.Euler(0, triangleOrder*60, 0) * pos;
    }
    public static Vector3 CalculateHexagonPosition(int index, float inter = 1.7f)
    {
        if(index==0) 
            return Vector3.zero;
        var degree = 1;
        var temp = index;
        while (temp-6*(degree) > 0)
        {
            temp -= 6 * degree;
            degree++;
        }
        var triangleOrder = ((temp-1) / degree);
        var order = (temp-1) % degree;
        
        
        var pos = (Vector3.right * degree*0.5f +Vector3.left*order)* inter+ Vector3.back * degree * inter*Mathf.Sqrt(3)*0.5f;
        return Quaternion.Euler(0, triangleOrder*60, 0) * pos;
    }

    public static int CalculateCircleCount(int index,bool completed=false)
    {
        if(index==0) return 1;
        if (index == 1) return 1;
        float tmp = index;
        int r = 1;
        var squareR = r * r;
        while ((tmp-squareR)>0)
        {
            tmp -= squareR;
            r++;
            squareR = r * r;
        }

        if (completed && tmp-squareR < 0) 
            r--;
        
        return r;
    }

    private static float CalculateIntervalDynamically(int totalCount, float maxInterval)
    {
        var pow = Mathf.Sqrt((float)totalCount/200);
        return maxInterval / (1+ pow);
    }

    public static Vector3 CalculatePyramidPosition(int i, Vector3 actualSize)
    {
        var index = i;
        int x = 0;
        int y = 0;
        int z = 0;

        int currentIndex = index;
      
        while (currentIndex>=0)
        {
            y++;
            int squareEdgeLength = 2 * y - 1;
            int squareLength = squareEdgeLength * squareEdgeLength;
            if (squareLength <= currentIndex)
            {
                currentIndex -= squareLength;
            }
            else
            {
                x = currentIndex % squareEdgeLength;
                z = currentIndex / squareEdgeLength;
                x -= squareEdgeLength/2;
                z -= squareEdgeLength/2;

                break;
            }
         
        }

        var pos = new Vector3(x, y, z);

        return Vector3.Scale(actualSize, pos);

    }
    
    public static (int groupIndex, Vector3 pos) CalculateSplitGroupCircularPositionWithIndex(int index,float interval, int groupCount,
        params float[] ratios)
    {
        if(index>=groupCount) throw new ArgumentException("groupCount must be greater than groupCount");
        if(interval<=0f) throw new ArgumentException("interval must be greater than 0f");
        if (!ratios.Any()) ratios = new[] {1f};
        if(ratios.Contains(0f)) throw new ArgumentException("ratios cant contains 0f");

        var sum = ratios.Sum();
        for (var i = 0; i < ratios.Length; i++)
        {
            ratios[i] /= sum;
        }
            
        var a = index;
        var groupIndex = 0;
        var ratioList = ratios.ToList(); 

        while (a>ratioList.First()*groupCount)
        {
            a -= (int)(ratioList.First()*groupCount);
            ratioList.RemoveAt(0);
            groupIndex++;
        }
            
        var pos = CalculateCircularPositionWithIndex(a, interval);
        var r = 0;
        for (var g = 1; g <= groupIndex; g++)
        {
            r += CalculateCircleCount((int)(ratios[g-1]*groupCount));
            r += CalculateCircleCount((int)(ratios[g]*groupCount));
        }
        pos += Vector3.left * interval * r;
        return (groupIndex: groupIndex, pos: pos);
    }
}


