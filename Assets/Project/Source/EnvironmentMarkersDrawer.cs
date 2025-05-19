using Antilatency.SDK;
using System.Collections.Generic;
using UnityEngine;

namespace CookieNoir.VDayXR
{
    public class EnvironmentMarkersDrawer : MonoBehaviour
    {
        [SerializeField] private GameObject _markerPrefab;
        [SerializeField] private Transform _markersParent;
        [SerializeField] private AltEnvironmentComponent _environmentProvider;
        private readonly List<GameObject> _markerInstances = new();

        private void ClearMarkers()
        {
            int count = _markerInstances.Count;
            if (count == 0)
            {
                return;
            }
            for (int i = 0; i < count; ++i)
            {
                Destroy(_markerInstances[i]);
            }
            _markerInstances.Clear();
        }

        private void OnEnable()
        {
            if (_markerPrefab == null)
            {
                enabled = false;
            }
        }

        private void Update()
        {

            if (_environmentProvider == null)
            {
                ClearMarkers();
                return;
            }
            var environment = _environmentProvider.GetEnvironment();
            if (environment == null)
            {
                ClearMarkers();
                return;
            }
            var markers = environment.getMarkers();
            if (markers == null ||
                markers.Length == 0)
            {
                ClearMarkers();
                return;
            }
            int markersCount = markers.Length;
            int instancesCount = _markerInstances.Count;
            int instancesReady = System.Math.Min(markersCount, instancesCount);
            for (int i = 0; i < instancesReady; ++i)
            {
                var marker = markers[i];
                _markerInstances[i].transform.localPosition = marker;
            }
            if (markersCount == instancesCount)
            {
                return;
            }
            if (markersCount < instancesCount)
            {
                for (int i = instancesCount - 1; i >= instancesReady; --i)
                {
                    Destroy(_markerInstances[i]);
                    _markerInstances.RemoveAt(i);
                }
            }
            else
            {
                for (int i = instancesReady; i < markersCount; ++i)
                {
                    var instance = Instantiate(_markerPrefab, markers[i], Quaternion.identity, _markersParent);
                    _markerInstances.Add(instance);
                }
            }
        }

        private void OnDisable()
        {
            ClearMarkers();
        }
    }
}
