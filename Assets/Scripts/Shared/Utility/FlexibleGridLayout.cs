using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Shared.Utility
{
    public class FlexibleGridLayout : LayoutGroup
    {
        public enum FitType
        {
            UNIFORM,
            WIDTH,
            HEIGHT,
            FIXEDROWS,
            FIXEDCOLUMNS
        }

        [Header("Flexible Grid")]
        public FitType fitType = FitType.UNIFORM;

        public int rows;
        public int columns;
        public Vector2 cellSize;
        public Vector2 spacing;

        public bool fitX;
        public bool fitY;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            if (fitType == FitType.WIDTH || fitType == FitType.HEIGHT || fitType == FitType.UNIFORM)
            {
                float squareRoot = Mathf.Sqrt(transform.childCount);
                rows = columns = Mathf.CeilToInt(squareRoot);
                switch (fitType)
                {
                    case FitType.WIDTH:
                        fitX = true;
                        fitY = false;
                        break;
                    case FitType.HEIGHT:
                        fitX = false;
                        fitY = true;
                        break;
                    case FitType.UNIFORM:
                        fitX = fitY = true;
                        break;
                }
            }

            if (fitType == FitType.WIDTH || fitType == FitType.FIXEDCOLUMNS)
            {
                rows = Mathf.CeilToInt(transform.childCount / (float)columns);
            }
            if (fitType == FitType.HEIGHT || fitType == FitType.FIXEDROWS)
            {
                columns = Mathf.CeilToInt(transform.childCount / (float)rows);
            }


            float parentWidth = rectTransform.rect.width * rectTransform.localScale.x;
            float parentHeight = rectTransform.rect.height * rectTransform.localScale.y;

            float cellWidth = parentWidth / columns - spacing.x / columns * (columns - 1)
                - padding.left / (float)columns - padding.right / (float)columns;
            float cellHeight = parentHeight / rows - spacing.y / rows * (rows - 1)
                - padding.top / (float)rows - padding.bottom / (float)rows; ;

            //float cellWidth = parentWidth / (float)columns;
            //float cellHeight = parentHeight / (float)rows;

            cellSize.x = fitX ? cellWidth : cellSize.x;
            cellSize.y = fitY ? cellHeight : cellSize.y;

            //cellSize.x = cellWidth;
            //cellSize.y = cellHeight;

            int columnCount, rowCount;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                rowCount = i / columns;
                columnCount = i % columns;

                if (fitType == FitType.WIDTH || fitType == FitType.HEIGHT)
                {
                    var rest = rows * columns - i;
                    if (i == rectChildren.Count - 1 && rest > 0)
                    {
                        cellSize.x *= rest;
                        columnCount /= rest;
                    }
                }

                var item = rectChildren[i];

                var xPos = cellSize.x * columnCount + spacing.x * columnCount + padding.left;
                var yPos = cellSize.y * rowCount + spacing.y * rowCount + padding.top;

                //var xPos = (cellSize.x * columnCount);
                //var yPos = (cellSize.y * rowCount);

                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);

            }
        }

        public override void CalculateLayoutInputVertical()
        {
        }

        public override void SetLayoutHorizontal()
        {
        }

        public override void SetLayoutVertical()
        {
        }
    }
}