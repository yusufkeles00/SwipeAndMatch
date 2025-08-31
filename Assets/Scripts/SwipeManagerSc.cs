using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeManagerSc : MonoBehaviour
{
	private MainBoardSc mainBoard;

	public DotSc firstDot;
	public DotSc secondDot;

	public GameObject touchableArea;
	private GameObject tempTouchableArea;

	public Vector2 firstTouchPos;
	public Vector2 secondTouchPos;
	private Vector2 tempDotPos;

	public int oldFirstDotColumn;
	public int oldFirstDotRow;
	public int oldSecondDotColumn;
	public int oldSecondDotRow;


	public float swipeSpeed;

	public bool canSwipe;
	public bool canRefill = false;
	public bool hasMatched = false;

	private void Start()
	{
		mainBoard = FindObjectOfType<MainBoardSc>();
		canSwipe = false;
	}

	private void Update()
	{
		SwipeDots();
	}

	public void CalculateSwipeDirection()
	{
		hasMatched = false;

		if (Mathf.Abs(secondDot.column - firstDot.column) == 1)// for horizontal swipes
		{
			if (firstDot.row == secondDot.row && firstDot.column < secondDot.column && secondDot.column < mainBoard.horizontalSize)// swipe right
			{
				//Debug.Log("right");

				firstDot.column++;
				secondDot.column--;

				ChangeTheDotVariables();

			}
			else if (firstDot.row == secondDot.row && firstDot.column > secondDot.column && secondDot.column >= 0)// swipe left
			{
				//Debug.Log("left");

				firstDot.column--;
				secondDot.column++;

				ChangeTheDotVariables();
			}
		}
		else if (Mathf.Abs(secondDot.row - firstDot.row) == 1)// for vertical swipes
		{
			if (firstDot.column == secondDot.column && firstDot.row > secondDot.row && secondDot.row >= 0)// swipe down
			{
				//Debug.Log("down");

				firstDot.row--;
				secondDot.row++;

				ChangeTheDotVariables();
			}

			else if (firstDot.column == secondDot.column && firstDot.row < secondDot.row && secondDot.row < mainBoard.verticalSize)// swipe up
			{
				//Debug.Log("up");

				firstDot.row++;
				secondDot.row--;

				ChangeTheDotVariables();
			}
		}
		else
		{
			Debug.Log("Error!!!");
		}
	}

	void ChangeTheDotVariables()
	{
		// update the board array informations
		mainBoard.dotsBoard[firstDot.column, firstDot.row] = firstDot.gameObject;
		mainBoard.dotsBoard[secondDot.column, secondDot.row] = secondDot.gameObject;

		// update the dot names
		firstDot.name = firstDot.column + "," + firstDot.row;
		secondDot.name = secondDot.column + "," + secondDot.row;

		CheckTheMatches();

		canSwipe = true;
	}

	IEnumerator ChangeTheDotVariablesBack()
	{
		yield return new WaitForSeconds(0.75f);

		// update the board array informations
		mainBoard.dotsBoard[oldFirstDotColumn, oldFirstDotRow] = firstDot.gameObject;
		mainBoard.dotsBoard[oldSecondDotColumn, oldSecondDotRow] = secondDot.gameObject;

		firstDot.column = oldFirstDotColumn;
		firstDot.row = oldFirstDotRow;

		secondDot.column = oldSecondDotColumn;
		secondDot.row = oldSecondDotRow;

		// update the dot names
		firstDot.name = oldFirstDotColumn + "," + oldFirstDotRow;
		secondDot.name = oldSecondDotColumn + "," + oldSecondDotRow;

		canSwipe = true;

	}

	void SwipeDots()
	{
		if (canSwipe)
		{
			// change the positions
			firstDot.transform.position = Vector2.Lerp(firstDot.transform.position, new Vector2(firstDot.column, firstDot.row), swipeSpeed * Time.deltaTime);
			secondDot.transform.position = Vector2.Lerp(secondDot.transform.position, new Vector2(secondDot.column, secondDot.row), swipeSpeed * Time.deltaTime);

			if(hasMatched == false)
			{
				StartCoroutine("ChangeTheDotVariablesBack");
				hasMatched = true;
			}
		}
	}

	public IEnumerator RefillTheBoard()
	{
		yield return new WaitForSeconds(1f);

		for (int i = 0; i < mainBoard.horizontalSize; i++)
		{
			for (int j = 0; j < mainBoard.verticalSize; j++)
			{
				if (mainBoard.dotsBoard[i, j] == null)
				{
					Vector2 currentPos = new Vector2(i, j);

					int dotNum = Random.Range(0, mainBoard.dotsObjects.Length);

					GameObject dot = Instantiate(mainBoard.dotsObjects[dotNum], currentPos, Quaternion.identity);
					dot.name = i + "," + j;
					dot.GetComponent<DotSc>().column = i;
					dot.GetComponent<DotSc>().row = j;
					mainBoard.dotsBoard[i, j] = dot;
				}
			}
		}

		yield return new WaitForEndOfFrame();

		CheckTheMatches();
	}

	void CheckTheMatches()
	{
		DotSc[] Dots = FindObjectsOfType<DotSc>();

		for (int i = 0; i < Dots.Length; i++)
		{
			DotSc dot = Dots[i];

			//Debug.Log(dot.gameObject.name);
			//Debug.Log("column: " + dot.column + "   row: " + dot.row + "    h size: " + mainBoard.horizontalSize);
			dot.FindMatches();
		}
	}

	public void ShowTheTouchableArea()
	{
		tempTouchableArea = Instantiate(touchableArea, firstDot.transform.position, Quaternion.identity);

		firstDot.dotAnim.SetBool("isTouched", true);
	}

	public void HideTheTouchableArea()
	{
		firstDot.dotAnim.SetBool("isTouched", false);

		Destroy(tempTouchableArea);
	}

	
}
