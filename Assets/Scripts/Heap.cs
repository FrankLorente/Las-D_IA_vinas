using UnityEngine;
using System.Collections;
using System;

public class Heap<T> where T : IHeapItem<T> //En lugar de monobehaviour, le decimos que usa un tipo T, y usamos el where T : iheapItem<T>
                                            //para asegurarnos de que usa la interfaz correspondiente
{

    T[] items;
    int currentItemCount; //contador del numero de items a los que accederemos a lo largo de la ejecucion

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    //Añade un item al heap
    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item); //Llamada al metodo SortUp para actualizar el heap
        currentItemCount++;
    }

    /*Remueve el item que ocupe el indice 0 del heap*/
    public T RemoveFirst() 
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]); //Llamada al metodo SortDown para actualizar el heap
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    /*Reestructura la posicion de cada item en el heap tras eliminar algun item para que siga cumpliendo la regla de que cada padre debe ser
      mayor que sus hijos */
    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1; //porque cada nodo tiene 2 hijos, nodo hijo izq = nodo * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2; //y nodo hijo izq = nodo * 2 + 1
                                  //ESTO SE VE CLARAMENTE CON UN EJEMPLO VISUAL DE LA ESTRUCTURA DEL HEAP, SINO SE ENTIENDE PREGUNTAR A PERE
            int swapIndex = 0; //indice del item a intercambiar y se actualizara mas adelante en el condicional de este metodo

            //si el INDICE HIJO IZQUIERDO es MENOR que la CANTIDAD DE ITEMS ACCEDIDOS(Esto pasara si el padre tiene almenos 1 hijo)
            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft; //indice del item a intercambiar sera el hijo izquierdo

                //si el INDICE HIJO DERECHO es MENOR que la CANTIDAD DE ITEMS ACCEDIDOS
                if (childIndexRight < currentItemCount)
                {

                    //IMPORTANTE, ESTE METODO COMPARETO NO ES EL METODO QUE ESTA IMPLICITO EN LA LIBRERIA DE UNITY, ES EL QUE ESTA DEFINIDO
                    //EN LA CLASE NODO!!!!!!!, asi que funciona distinto.
                    //MIRAR METODO EL METODO COMPARETO DENTRO DE NODE PARA ENTENDERLO, AHI LO EXPLICO

                    //1 si hijo IZQUIERDO es MENOR,
                    //0 si ambos hijos son IGUALES
                    //-1 si hijo IZQUIERDO es MAYOR que el derecho.
                    //asi que si el hijo izquierdo es menor que el derecho, 
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) // ASI QUE SI HIJO DERECHO ES MENOR QUE HIJO IZQUIERDO
                    {
                        swapIndex = childIndexRight; //NOS QUEDAMOS CON EL DERECHO PORQUE ES MENOR
                    }
                }

                //1 si ITEM es MENOR que ITEM A COMPARAR
                //0 si ambos ITEMS son IGUALES
                //-1 si ITEM es MAYOR que ITEM A COMPARAR.
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }

                //SINO, SE DEJA COMO ESTABA;
                else
                {
                    return;
                }

            }

            //SINO, SALIR DEL BUCLE (el padre no tiene hijos)
            else
            {
                return;
            }

        }
    }

    //Actualiza el heap y los indices que deba ocupar cada item tras la modificacion llamando al metodo SortDown */
    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2; /*
                                                     * Debido a que el heap es parecido a un arbol binario, pero con la diferencia
                                                     * de que cada padre debe ser menor que sus dos hijos, esta formula nos permite
                                                     * averiguar la posicion del padre en este arbol
                                                     */

        //Ira actualizandolo intercambiando padre con hijo hasta que se cumple que cada hijo sea mayor que su padre para todo el heap
        while (true)
        {
            T parentItem = items[parentIndex];
           
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2; /*Aqui, despues de intercambiarlos, actualiza la posicion del nuevo padre del item que
                                                     *estamos comprobando*/
        }
    }

    //Metodo para intercambiar los items
    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }



}

//INTERFAZ
public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}