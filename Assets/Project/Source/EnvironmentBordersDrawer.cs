using Antilatency.SDK;
using UnityEngine;

namespace CookieNoir.VDayXR
{
    public class EnvironmentBordersDrawer : MonoBehaviour
    {
        [SerializeField] private AltEnvironmentComponent _environmentProvider;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _addition = 0f;
        [SerializeField, Min(0f)] private float _minExtents = 0.5f;
        private readonly Vector3[] _borders = new Vector3[4];

        private void OnEnable()
        {
            if (_lineRenderer == null)
            {
                enabled = false;
            }
            _lineRenderer.useWorldSpace = false;
            _lineRenderer.loop = true;
            _lineRenderer.positionCount = 4;
            _lineRenderer.SetPositions(_borders);
        }

        private void SetDefaultBorders()
        {
            float addition = System.Math.Max(_minExtents, _addition);
            _borders[0] = new Vector3(-addition, 0f, -addition);
            _borders[1] = new Vector3(addition, 0f, -addition);
            _borders[2] = new Vector3(addition, 0f, addition);
            _borders[3] = new Vector3(-addition, 0f, addition);
        }

        private void Update()
        {
            if (_environmentProvider == null)
            {
                SetDefaultBorders();
                return;
            }
            var environment = _environmentProvider.GetEnvironment();
            if (environment == null)
            {
                SetDefaultBorders();
                return;
            }
            var markers = environment.getMarkers();
            if (markers == null ||
                markers.Length == 0)
            {
                SetDefaultBorders();
                return;
            }
            var bounds = new Bounds();
            for (int i = 0; i < markers.Length; ++i)
            {
                bounds.Encapsulate(markers[i]);
            }
            var center = bounds.center;
            float centerX = center.x;
            float centerZ = center.z;
            float extentsX = System.Math.Max(_minExtents, bounds.extents.x + _addition);
            float extentsZ = System.Math.Max(_minExtents, bounds.extents.z + _addition);
            _borders[0] = new Vector3(centerX - extentsX, 0f, centerZ - extentsZ);
            _borders[1] = new Vector3(centerX + extentsX, 0f, centerZ - extentsZ);
            _borders[2] = new Vector3(centerX + extentsX, 0f, centerZ + extentsZ);
            _borders[3] = new Vector3(centerX - extentsX, 0f, centerZ + extentsZ);
            _lineRenderer.SetPositions(_borders);
        }
    }
}
