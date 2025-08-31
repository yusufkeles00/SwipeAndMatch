using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MainBoardSc : MonoBehaviour
{
	public GameObject[,] board;
	public GameObject[,] dotsBoard;
	public GameObject[] dotsObjects;   
	public GameObject slotObject;

	public int horizontalSize;
	public int verticalSize;
	public int clickDotCount = 0; // 0 means: it will be a first touch --- 1 means: it will be a second touch
	public int count = 0;


	private void Start()
	{
		board = new GameObject[horizontalSize, verticalSize];
		dotsBoard = new GameObject[horizontalSize, verticalSize];

		StartCoroutine("SetUpTheBoard");
	}

	IEnumerator SetUpTheBoard()
	{
		yield return new WaitForSeconds(2f);

		for (int i = 0; i < horizontalSize; i++)
		{
			for (int j = 0; j < verticalSize; j++)
			{
				Vector2 currentPos = new Vector2(i, j);
				GameObject tile = Instantiate(slotObject, currentPos, Quaternion.identity);
				tile.transform.parent = this.transform;
				tile.name = i + "," + j;


				//Place the dots
				int dotNum = Random.Range(0, dotsObjects.Length);

				while (CheckTheMatches(i, j, dotsObjects[dotNum]))
				{
					count++;
					dotNum = Random.Range(0, dotsObjects.Length);
				}

				GameObject dot = Instantiate(dotsObjects[dotNum], currentPos, Quaternion.identity);
				dot.name = i + "," + j;
				dot.GetComponent<DotSc>().column = i;
				dot.GetComponent<DotSc>().row = j;
				dotsBoard[i, j] = dot;
			}
		}
	}

	private bool CheckTheMatches(int column, int row, GameObject dot)
	{
		if (column > 1 && row > 1)
		{
			if (dotsBoard[column - 1, row].tag == dot.tag && dotsBoard[column - 2, row].tag == dot.tag)
			{
				return true;
			}
			if (dotsBoard[column, row - 1].tag == dot.tag && dotsBoard[column, row - 2].tag == dot.tag)
			{
				return true;
			}
		}
		else if (column <= 1 || row <= 1)
		{
			if(row > 1)
			{
				if (dotsBoard[column, row - 1].tag == dot.tag && dotsBoard[column, row - 2].tag == dot.tag)
				{
					return true;
				}
			}
			else if(column > 1)
			{
				if (dotsBoard[column - 1, row].tag == dot.tag && dotsBoard[column - 2, row].tag == dot.tag)
				{
					return true;
				}
			}

		}
		return false;
	}

}
