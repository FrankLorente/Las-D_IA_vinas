using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node>
{

    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;
    //public int movementPenalty;

    public int gCost; //DISTANCIA entre el NODO INICIAL hasta el nodo actual
    public int hCost; //Coste heuristico -> DISTANCIA entre el NODO OBJETIVO hasta el nodo actual 
    public Node parent;
    int heapIndex;

    //public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY, int _penalty)
    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        //movementPenalty = _penalty;
    }

    //DISTANCIA RESULTANTE DE gCost + hCost
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    //ACCESO Y ACTUALIZACION DEL CONTENIDO DE UN INDICE DEL HEAP
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    //ESTE METODO USA OTRO COMPARE TO, PERO SE TRATA DE UN METODO DISTINTO QUE FORMA PARTE DE LA LIBRERIA DE UNITY,
    //ASI QUE NO ES RECURSIVO NI NADA PARECIDO, EL DE UNITY USA ENTEROS COMO OBJETO Y ARGUMENTO Y LOS COMPARA,

    //NOSOTROS USAREMOS COMO OBJETO A UN NODO Y COMO ARGUMENTO A OTRO NODO
    //COMPARAMOS SUS FCOST, Y SI ESTAS SON IGUALES, PASA A COMPARAR SUS HCOST
    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);

        //-1 si ATRIBUTO es MENOR que el valor pasado como ARGUMENTO,
        //0 si ambos son IGUALES
        //1 si ATRIBUTO es MAYOR que el valor pasado como ARGUMENTO,

        //En el caso de que ambos nodos tengan el mismo fCost, compararemos sus hCost para quedarnos con el mejor 
        if (compare == 0)
        {
            //-1 si ATRIBUTO es MENOR que el valor pasado como ARGUMENTO,
            //0 si ambos son IGUALES
            //1 si ATRIBUTO es MAYOR que el valor pasado como ARGUMENTO,
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare; //Para que al llamarlo en heap, se quede con el menor, hace que sea un poco mas legible el codigo del heap
                         //con este pequeño cambio, pero se puede cambiar luego si lo quereis de otra forma.
    }
}