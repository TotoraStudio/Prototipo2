using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;  //Nos permite usar listas
using Random = UnityEngine.Random;  //Menciona el uso del Random en Unity y generar el número aleatorio

namespace Completed
{

    public class BoardManager : MonoBehaviour
    {

        //Usando Serializable nos permite convertir un objeto en una secuencia de bytes y luego almacenarlo en un objeto o transmitirlo a memoria, una base de datos, o en un archivo. 
        [Serializable]
        public class Count {
            public int minimum;             //Minimo valor para nuestra clase Count.
            public int maximum;             //Maximo valor para nuestra clase Count.

            //Asignando el contructor
            public Count(int min, int max)
            {
                minimum = min;
                maximum = max;

            }


        }
        public int columns = 16;                               //Número de columnas en nuestro GameBoard
        public int rows = 16;                                   //Número de filas en nuestro GameBoard.
        public Count wallCount = new Count(9, 17);              //Limite de bajo y alto de nuestro numero aleatorio.
        public GameObject[] floorTiles;                         //Array de  floor prefabs.
        public GameObject[] wallTiles;                          //Array de   Wall Prefabs.              
        public GameObject[] outerWallTiles;                     //Array de outer tile prefabs.

        private Transform boardHolder;                  //Variable para almacenar una referencia al transform(posicion, escala,) de nuestro tablero de juego
        private List<Vector3> gridPositions = new List<Vector3>(); //Lista de posibles ubicaciones para colocar los Tiles


        //Limpia nuestra lista de gridPositions y se prepara para generar una nuevo tablero
        void InitialiseList() {
            //Limpiar nuestra lista gridPositions.
            gridPositions.Clear ();

            //Bucle a través del eje X (columnas)
            for (int x = 1; x < columns - 1; x++) {

                //Con cada Columna, hacer un recorrido por el bucle en el eje Y (filas).
                for (int y = 1; y < rows - 1; y++)
                {
                    //Cada índice adherido al new Vector3 de nuestra lista con las coordenadas X e Y de nuestra posicion.
                    gridPositions.Add(new Vector3(x, y, 0f));

                }

            }
        }

        //Estableciendo las paredes exteriores de nuestro piso(fondo) del tablero de juego.
        void BoardSetup()
        {
            //Inicializando el tablero y el marco en una transform
            boardHolder = new GameObject("Board").transform;

            //bucle a lo largo del eje X, a partir de -1(para rellenar el borde) con floor o bordes del tablero
            for (int x = -1; x < columns + 1; x++)
            {
                //bucle a lo largo del eje X, a partir de -1 con floor o bordes del tablero
                for (int y = -1; y < rows + 1; y++)
                {
                    //Elegir un tile random de un array del floor tile prefabs y preparar la instancia
                    GameObject toInstantiate = floorTiles[Random.Range(0,floorTiles.Length)];
                    
                    //Comprobar si la posición actual está en el borde de la tabla, si es así escoger un valor random del wall prefab  del array outer wall tiles.
                    if(x==-1||x==columns || y == -1 || y== rows)
                        toInstantiate = outerWallTiles[Random.Range(0,outerWallTiles.Length)];

                    //Instanciar el GameObject instance utilizando el prefab elegido para la parte superior Inicializado en el Vector3 correspondiente a la posicion actual del grid  en el bucle, y luego colocarlo en e GAmeObject
                    GameObject instance =
                        Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                    //Establecer la cabeza de nuestra instancia de objeto recién instanciado a boardHolder,esto hace que se saturen las jerarquías
                    instance.transform.SetParent(boardHolder);

                }

            }


        }

        //RandomPosition devuelve una posicion aleatoria de nuestra lista de gridPosition
        Vector3 RandomPosition ()
        {
            //Declarar un entero randomIndex, del conjunto obtenido del numero random entre 0 y el número de items del List gridPositions.
            int randomIndex = Random.Range(0, gridPositions.Count);

            //Declarar la variable de tipo Vector3 llamada randomPosition, conjunto de valor de entrada de randomIndex a gridPositions.
            Vector3 randomPosition = gridPositions[randomIndex];

            //Remover la entrada dada por randomIndex de la lista de estos sino puede ser reusado
            gridPositions.RemoveAt(randomIndex);

            //Devolver de forma aleatoria la posición seleccionada en Vector3
            return randomPosition;

        }

        //LayoutObjectAtRandom acepta en el array de Game objects la opción de escoger el minimo y maximo rango de numeros de objetos creados 
        void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
        {
            //Escoger una numero aleatorio de la instancia de objetos en tre el limite maximo y minimo
            int objectCount = Random.Range(minimum, maximum +1);

            //Instanciar objetos hasta que aleatoriamente  el limite de objectcount sea alcanzado
            for (int i = 0; i < objectCount; i++)
            {
                //Escoger una posicion de randomPosition obtenido de la posicion aleatoria de nuestra lista de vector3s almacenadas en gridPosition
                Vector3 randomPosition = RandomPosition();

                //Escoger un random tile del tileArray y asiganrlo al tilechoice
                GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

                //Inicializar el tileChoice dentro de una posicion devuelta a RandomPosition sin ningún cambio de la rotacion
                Instantiate(tileChoice, randomPosition, Quaternion.identity);

                
            }

        }

        //Configurar el nivel y llamar a las funciones anteriores para diseñar el tablero de juego
        public void SetupScene(int level)
        {
            //Crear los outer walls y pizos.
            BoardSetup();

            //Resetear la lista de los gridPositions.
            InitialiseList();

            //Instanciar un numero random de wall tiles basado en un minimo y maximo, dentro de posiciones random.
            LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);


        }

    }
}