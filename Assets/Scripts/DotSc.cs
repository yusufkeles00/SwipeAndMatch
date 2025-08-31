using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotSc : MonoBehaviour
{
	[SerializeField]
	private MainBoardSc mainBoard;
	private SwipeManagerSc swipeManager;

	public Animator dotAnim;

	public GameObject dotDestroyParticle;

	public int column;
	public int row;

	private float matchDelayTime = 0.5f;

	public bool isMatched = false;

	private void Start()
	{
		dotAnim = GetComponent<Animator>();

		mainBoard = FindObjectOfType<MainBoardSc>();
		swipeManager = FindObjectOfType<SwipeManagerSc>();
	}

	private void Update()
	{
		//FindMatches();

		//StartCoroutine("DotMovement");

		if (isMatched)
		{
			StartCoroutine("Matched");
		}
	}

	private void OnMouseDown()
	{
		int clickCount = mainBoard.clickDotCount;

		if (clickCount == 0)
		{
			swipeManager.firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			RaycastHit2D dotInformation = Physics2D.Raycast(swipeManager.firstTouchPos, Camera.main.transform.forward);

			if (dotInformation.collider != null)
			{
				swipeManager.firstDot = dotInformation.transform.gameObject.GetComponent<DotSc>();
				swipeManager.oldFirstDotColumn = swipeManager.firstDot.column;
				swipeManager.oldFirstDotRow = swipeManager.firstDot.row;

				//Debug.Log("First: " + swipeManager.firstDot.gameObject.name + "   column: " + swipeManager.firstDot.column + "  row: " + swipeManager.firstDot.row);

				swipeManager.ShowTheTouchableArea();

				mainBoard.clickDotCount = 1;
			}
		}
		else if (clickCount == 1)
		{
			swipeManager.secondTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			RaycastHit2D dotInformation = Physics2D.Raycast(swipeManager.secondTouchPos, Camera.main.transform.forward);

			if (dotInformation.collider != null)
			{
				//Debug.Log(dotInformation.collider.name);
				swipeManager.secondDot = dotInformation.transform.gameObject.GetComponent<DotSc>();
				swipeManager.oldSecondDotColumn = swipeManager.secondDot.column;
				swipeManager.oldSecondDotRow = swipeManager.secondDot.row;

				//Debug.Log("Second: " + swipeManager.secondDot.gameObject.name + "   column: " + swipeManager.secondDot.column + "  row: " + swipeManager.secondDot.row);

				swipeManager.HideTheTouchableArea();
				swipeManager.CalculateSwipeDirection();
			}
			if (dotInformation.collider == null) //boþa týklama
			{
				Debug.Log("Invalid Touch");
			}


			mainBoard.clickDotCount = 0;
		}
	}

	public void FindMatches()
	{
		if (column >= 0 && column < mainBoard.horizontalSize - 2)
		{
			GameObject rightDot1 = mainBoard.dotsBoard[column + 1, row];
			GameObject rightDot2 = mainBoard.dotsBoard[column + 2, row];

			if (rightDot1 != null && rightDot2 != null)
			{
				if (this.gameObject.tag == rightDot1.tag && this.gameObject.tag == rightDot2.tag)
				{
					swipeManager.hasMatched = true;

					if (rightDot1.GetComponent<DotSc>().isMatched == false)
					{
						rightDot1.GetComponent<DotSc>().isMatched = true;
					}
					if (rightDot2.GetComponent<DotSc>().isMatched == false)
					{
						rightDot2.GetComponent<DotSc>().isMatched = true;
					}
					if (isMatched == false)
					{
						isMatched = true;
					}

					swipeManager.StartCoroutine(swipeManager.RefillTheBoard());
				}
			}
		}

		if (row >= 0 && row < mainBoard.verticalSize - 2)
		{
			GameObject upDot1 = mainBoard.dotsBoard[column, row + 1];
			GameObject upDot2 = mainBoard.dotsBoard[column, row + 2];

			if (upDot1 != null && upDot2 != null)
			{
				if (this.gameObject.tag == upDot1.tag && this.gameObject.tag == upDot2.tag)
				{
					swipeManager.hasMatched = true;

					if (upDot1.GetComponent<DotSc>().isMatched == false)
					{
						upDot1.GetComponent<DotSc>().isMatched = true;
					}
					if (upDot2.GetComponent<DotSc>().isMatched == false)
					{
						upDot2.GetComponent<DotSc>().isMatched = true;
					}
					if (isMatched == false)
					{
						isMatched = true;
					}

					swipeManager.StartCoroutine(swipeManager.RefillTheBoard());
				}
			}
		}


	}

	IEnumerator Matched() // that means we have to destroy the dot from dotsBoard array and the actual object. First put the NULL to the dotsBoard array
	{
		yield return new WaitForSeconds(matchDelayTime);

		SpriteRenderer dotSprite = GetComponent<SpriteRenderer>();
		dotSprite.color = new Color(0f, 0f, 0f, 0.2f);

		swipeManager.canSwipe = false;

		Instantiate(dotDestroyParticle, transform.position, Quaternion.identity);

		dotAnim.SetTrigger("Destroy");

		mainBoard.dotsBoard[column, row] = null;

		Destroy(gameObject);
	}
}
