using UnityEngine;

namespace nobodyworks.builder.cutscene
{
    public class CutscenesController
    {
        private readonly CutscenesSettings _settings;
        private readonly CutsceneManager[] _cutscenes;

        private CutsceneManager _current;

        public bool IsPlaying => _current != null;

        public CutscenesController(CutscenesSettings settings)
        {
            _settings = settings;
            _cutscenes = GameObject.FindObjectsByType<CutsceneManager>(FindObjectsSortMode.None);
        }

        public void Dispose()
        {
            Stop();
        }

        public void Play<T>() where T : CutsceneManager
        {
            if (_current != null)
            {
                Stop();
            }

            foreach (var cutscene in _cutscenes)
            {
                if (cutscene is not T match)
                {
                    continue;
                }

                _current = match;
                _current.OnFinished += CutsceneFinishedHandler;
                _current.Play();
                return;
            }
        }

        public void Stop()
        {
            if (_current == null)
            {
                return;
            }

            _current.OnFinished -= CutsceneFinishedHandler;
            _current.Stop();
            _current = null;
        }

        private void CutsceneFinishedHandler()
        {
            _current.OnFinished -= CutsceneFinishedHandler;
            _current = null;
        }
    }
}
