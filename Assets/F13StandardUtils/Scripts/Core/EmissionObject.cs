using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.Scripts.Core
{
    public class EmissionObject : MonoBehaviour
    {
        [SerializeField,ReadOnly] private Renderer _renderer;
        [SerializeField,OnValueChanged(nameof(UpdateEmissionColor))] private Color _emissionColor = Color.red;
        [SerializeField,OnValueChanged(nameof(UpdateShine)),HideInEditorMode] private bool _isShine = false;


        public bool IsShine
        {
            get => _isShine;
            set
            {
                if (_isShine == value) return;
                _isShine = value;
                UpdateShine();
            }
        }

        private void Reset()
        {
            _renderer = GetComponent<Renderer>();
        }
    
        private void Awake()
        {
            UpdateEmissionColor();
        }
        
        
        private void UpdateEmissionColor()
        {
            if (!Application.isPlaying) return;
            foreach (var m in _renderer.materials)
            {
                m.SetColor("_EmissionColor", _emissionColor);
            }
            UpdateShine();
        }
        
        private void UpdateShine()
        {
            if (!Application.isPlaying) return;
            foreach (var m in _renderer.materials)
            {
                if (_isShine) m.EnableKeyword("_EMISSION");
                else m.DisableKeyword("_EMISSION");
            }
        }

        public void SetEmissionColor(Color color)
        {
            _emissionColor = color;
            UpdateEmissionColor();
        }

        [Button,HideInEditorMode]
        public void SetShine(bool state,Color emissionColor)
        {
            _isShine = state;
            SetEmissionColor(emissionColor);
        }
    
    }
}
