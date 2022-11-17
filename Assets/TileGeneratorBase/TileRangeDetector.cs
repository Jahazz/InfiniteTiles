using System.Collections.Generic;
using UnityEngine;

namespace TileGenerator
{
    public class TileRangeDetector : MonoBehaviour
    {
        [field: SerializeField]
        private bool ShowIndicator { get; set; }
        [field: SerializeField]
        public float BoundsCameraExtension { get; private set; }
        public List<Transform> SideRangeDictionary { get; private set; } = new List<Transform>();

        public Rect RangeDetectorSize { get; private set; }
        private Rect RangeDetectorWithOffset { get; set; }
        public Vector3 BottomLeft { get; private set; }
        public Vector3 BottomRight { get; private set; }
        public Vector3 TopLeft { get; private set; }
        public Vector3 TopRight { get; private set; }

        private void Awake ()
        {
            InitializeDetector(9.0f);
        }

        public Vector3 GetPointAtHeight (Ray ray, float height)
        {
            return ray.origin + (((ray.origin.y - height) / -ray.direction.y) * ray.direction);
        }

        private void InitializeDetector (float sideSize)
        {
            Ray bottomLeftRay = Camera.main.ViewportPointToRay(new Vector3(0, 0, 0));
            Ray topLeftRay = Camera.main.ViewportPointToRay(new Vector3(0, 1, 0));
            Ray topRightRay = Camera.main.ViewportPointToRay(new Vector3(1, 1, 0));
            Ray bottomRightRay = Camera.main.ViewportPointToRay(new Vector3(1, 0, 0));

            BottomLeft = GetPointAtHeight(bottomLeftRay, 0);
            TopLeft = GetPointAtHeight(topLeftRay, 0);
            TopRight = GetPointAtHeight(topRightRay, 0);
            BottomRight = GetPointAtHeight(bottomRightRay, 0);




            float distanceBetweenTopAndBottom = Mathf.Abs(TopLeft.z - BottomLeft.z);
            float longerWidth = Vector3.Distance(TopLeft, TopRight);
            float shorterWidth = Vector3.Distance(BottomLeft, BottomRight);
            float halfWidthDifference = (longerWidth - shorterWidth) / 2;
            Vector3 extendedLeft = new Vector3(BottomLeft.x - halfWidthDifference, BottomLeft.y, BottomLeft.z) - TopLeft;
            Vector3 extendedLeftNormalized = extendedLeft.normalized;
            Vector3 extendedRightNormalized = (new Vector3(BottomRight.x + halfWidthDifference, BottomRight.y, BottomRight.z) - TopRight).normalized;

            RangeDetectorSize = new Rect(BottomLeft.x - halfWidthDifference, BottomLeft.z, longerWidth, distanceBetweenTopAndBottom);
            float boundsCameraExtensionDouble = (BoundsCameraExtension * 2);
            RangeDetectorWithOffset = new Rect(RangeDetectorSize.x - BoundsCameraExtension, RangeDetectorSize.y- BoundsCameraExtension, RangeDetectorSize.width + boundsCameraExtensionDouble, RangeDetectorSize.height+ boundsCameraExtensionDouble);
            GeneratePointsOnRect(RangeDetectorWithOffset, sideSize);

            int numberOfPoints = Mathf.CeilToInt(distanceBetweenTopAndBottom / sideSize);
            float distanceBetweenPoints = extendedLeft.magnitude / numberOfPoints;

            //SpawnPointsOnVerticalLine(numberOfPoints, distanceBetweenPoints, sideSize, extendedLeftNormalized, extendedRightNormalized);






            //Vector2Int detectorSizeInCells = new Vector2Int(Mathf.CeilToInt(requestedDetectorSize.x / sideSize), Mathf.CeilToInt(requestedDetectorSize.y / sideSize)) + Vector2Int.one;
            //Vector2 offset = requestedDetectorSize / 2;

            //for (int currentXIndex = 0; currentXIndex < detectorSizeInCells.x; currentXIndex++)
            //{
            //    for (int currentYIndex = 0; currentYIndex < detectorSizeInCells.y; currentYIndex++)
            //    {
            //        SideRangeDictionary.Add(Instantiate(new GameObject(), new Vector3((currentXIndex * sideSize) - offset.x, 0, (currentYIndex * sideSize) - offset.y), Quaternion.identity, transform).transform);
            //    }
            //}
        }

        private void GeneratePointsOnRect (Rect rectWithOffset, float sideSize)
        {
            int numberOfPointsOnX = Mathf.CeilToInt(rectWithOffset.width / sideSize);
            int numberOfPointsOnY = Mathf.CeilToInt(rectWithOffset.height / sideSize);
            float pointDistanceOnX = rectWithOffset.width / numberOfPointsOnX;
            float pointDistanceOnY = rectWithOffset.height / numberOfPointsOnY;

            for (int currentYIndex = 0; currentYIndex < numberOfPointsOnY + 1; currentYIndex++)
            {
                for (int currentXIndex = 0; currentXIndex < numberOfPointsOnX + 1; currentXIndex++)
                {
                    Vector3 position = new Vector3(rectWithOffset.xMin + (currentXIndex * pointDistanceOnX), 0, rectWithOffset.yMax - (currentYIndex * pointDistanceOnY));
                    SideRangeDictionary.Add(Instantiate(new GameObject(), position, Quaternion.identity, transform).transform);
                }
            }

        }

        private void SpawnPointsOnVerticalLine (int numberOfPoints, float distanceBetweenPoints, float sideSize, Vector3 extendedLeftNormalized, Vector3 extendedRightNormalized)
        {
            for (int currentYIndex = 0; currentYIndex < numberOfPoints + 1; currentYIndex++)
            {
                Vector3 startPosition = TopLeft + (extendedLeftNormalized * (distanceBetweenPoints * currentYIndex));
                Vector3 endPosition = TopRight + (extendedRightNormalized * (distanceBetweenPoints * currentYIndex));
                SpawnPointsOHorizontalnLine(startPosition, endPosition, sideSize);
            }
        }

        private void SpawnPointsOHorizontalnLine (Vector3 startPosition, Vector3 endPosition, float sideSize)
        {

            Vector3 startEndPosition = endPosition - startPosition;
            Vector3 normalizedStartEnd = startEndPosition.normalized;

            float distance = startEndPosition.magnitude;
            int numberOfPoints = Mathf.CeilToInt(distance / sideSize);
            float pointDistance = distance / numberOfPoints;

            for (int currentXIndex = 0; currentXIndex < numberOfPoints + 1; currentXIndex++)
            {
                Vector3 position = startPosition + (normalizedStartEnd * (pointDistance * currentXIndex));
                SideRangeDictionary.Add(Instantiate(new GameObject(), position, Quaternion.identity, transform).transform);
            }
        }

        private void OnDrawGizmos ()
        {
#if UNITY_EDITOR
            if (ShowIndicator == true)
            {
                DrawRect(RangeDetectorSize);
                Gizmos.color = Color.cyan;
                DrawRect(RangeDetectorWithOffset);
                foreach (var item in SideRangeDictionary)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(item.position, 1);
                    //DrawString(item.WorldSide.ToString(), item.DetectorTransform.position, Color.red);
                }

                Gizmos.color = Color.green;
                Gizmos.DrawLine(BottomLeft, BottomRight);
                Gizmos.DrawLine(BottomRight, TopRight);
                Gizmos.DrawLine(TopRight, TopLeft);
                Gizmos.DrawLine(TopLeft, BottomLeft);
            }
#endif
        }


        //proudly stolen from https://answers.unity.com/questions/44848/how-to-draw-debug-text-into-scene.html
        static public void DrawString (string text, Vector3 worldPos, Color? colour = null)
        {
            UnityEditor.Handles.BeginGUI();

            var restoreColor = GUI.color;

            if (colour.HasValue)
                GUI.color = colour.Value;
            var view = UnityEditor.SceneView.currentDrawingSceneView;
            Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);

            if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
            {
                GUI.color = restoreColor;
                UnityEditor.Handles.EndGUI();
                return;
            }

            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
            GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);
            GUI.color = restoreColor;
            UnityEditor.Handles.EndGUI();
        }
        private void DrawRect (Rect rect)
        {
            Gizmos.DrawWireCube(new Vector3(rect.center.x, 0, rect.center.y), new Vector3(rect.width, 0, rect.height));
        }
    }
}
