using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace UnityEngine.Timeline {
	[Serializable]
	[TrackClipType(typeof(AudioSourcePlayableAsset))]
	[TrackColor(1f, .5f, 0f)]
	[TrackBindingType(typeof(AudioSource))]
	public class AudioSourcePlayableTrack : TrackAsset {
		public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount) {
			PlayableDirector playableDirector = go.GetComponent<PlayableDirector>();

			ScriptPlayable<AudioSourceInitPlayableBehaviour> playable =
				ScriptPlayable<AudioSourceInitPlayableBehaviour>.Create(graph, inputCount);

			AudioSourceInitPlayableBehaviour audioSourceInitPlayableBehaviour =
				playable.GetBehaviour();

			if ( audioSourceInitPlayableBehaviour != null ) {
				audioSourceInitPlayableBehaviour.director = playableDirector;
				audioSourceInitPlayableBehaviour.clips = GetClips();
			}

			return playable;
		}
	}
}
