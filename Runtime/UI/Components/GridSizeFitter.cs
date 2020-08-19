using UnityEngine;
using UnityEngine.UI;

namespace Playrika.GameFoundation.UI.Components
{
    [RequireComponent(typeof(RectTransform)), RequireComponent(typeof(GridLayoutGroup)), AddComponentMenu("GameFoundation/UI/Components/GridSizeFitter")]
    public class GridSizeFitter : MonoBehaviour
    {
        [SerializeField] private bool _fitOnAwake;

        [SerializeField] private bool _forcedSquare;

        [Space] [SerializeField] private bool _fitHorizontal;

        [Min(1)] [SerializeField] private int _horizontalCellsCount = 1;

        [Space] [SerializeField] private bool _fitVertical;

        [Min(1)] [SerializeField] private int _verticalCellsCount = 1;


        public GridSizeFitter SetForcedSquare(bool value)
        {
            _forcedSquare = value;
            return this;
        }

        public GridSizeFitter SetFitHorizontal(bool value)
        {
            _fitHorizontal = value;
            return this;
        }

        public GridSizeFitter SetHorizontalCellsCount(int value)
        {
            if (value <= 0)
                value = 1;

            _horizontalCellsCount = value;
            return this;
        }

        public GridSizeFitter SetFitVertical(bool value)
        {
            _fitVertical = value;
            return this;
        }

        public GridSizeFitter SetVerticalCellsCount(int value)
        {
            if (value <= 0)
                value = 1;

            _verticalCellsCount = value;
            return this;
        }


        public void Fit()
        {
            var rectTransform = GetComponent<RectTransform>();
            var gridLayoutGroup = GetComponent<GridLayoutGroup>();
            var rect = rectTransform.rect;
            var spacing = gridLayoutGroup.spacing;
            var cellSize = gridLayoutGroup.cellSize;

            if (_fitHorizontal)
                cellSize.x = (rect.width - spacing.x * (_horizontalCellsCount + 1)) / _horizontalCellsCount;

            if (_fitVertical)
                cellSize.y = (rect.height - spacing.y * (_verticalCellsCount + 1)) / _verticalCellsCount;

            if (_forcedSquare)
            {
                if (_fitHorizontal)
                    cellSize.y = cellSize.x;
                else if (_fitVertical)
                    cellSize.x = cellSize.y;
                else
                {
                    if (cellSize.x > cellSize.y)
                        cellSize.y = cellSize.x;
                    else
                        cellSize.x = cellSize.y;
                }
            }

            gridLayoutGroup.cellSize = cellSize;
        }

        private void Awake()
        {
            if (_fitOnAwake)
                Fit();
        }
    }
}