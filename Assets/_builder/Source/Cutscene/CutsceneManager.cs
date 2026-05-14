using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using nobodyworks.builder.utilities;

namespace nobodyworks.builder.cutscene
{
    public abstract class CutsceneManager : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private PlayableDirector _director;

        #endregion

        public event Action OnFinished;

        public void OnDestroy()
        {
            OnFinished = null;
        }

        public void Play()
        {
            // TODO (PO): In the future, some context should be added, such as a reference to GameManager or smth else
            OnSetup();
            _director.stopped += DirectorStoppedHandler;
            _director.Play();
        }

        public void Stop()
        {
            _director.stopped -= DirectorStoppedHandler;
            _director.Stop();
            OnTeardown();
        }

        protected abstract void OnSetup();
        protected abstract void OnTeardown();

        protected void SetOffset(Offset offset)
        {
            if (_director.playableAsset is not TimelineAsset timelineAsset)
            {
                return;
            }

            foreach (var track in timelineAsset.GetOutputTracks())
            {
                if (track is AnimationTrack animationTrack && track.name.Contains("O"))
                {
                    animationTrack.position = offset.Position;
                    animationTrack.rotation = Quaternion.Euler(offset.Angles);
                }
            }
        }

        private void DirectorStoppedHandler(PlayableDirector _)
        {
            _director.stopped -= DirectorStoppedHandler;
            OnTeardown();
            OnFinished?.Invoke();
        }
    }
}
