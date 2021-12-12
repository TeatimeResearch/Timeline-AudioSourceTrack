using System;
using UnityEngine;
using UnityEngine.Playables;

namespace UnityEngine.Timeline {
	[Serializable]
	public class AudioSourcePlayableAsset : PlayableAsset {
		[Tooltip("Optional: Use this AudioSource instead of track default")]
		public ExposedReference<AudioSource> clipAudioSource;

		[SerializeField, NotKeyable]
		public AudioClip audioClip;

		[SerializeField, NotKeyable]
		public bool loop = false;
		[Range(0.001f, 3f)]
		[SerializeField, NotKeyable]
		public float pitch = 1f;

		[SerializeField, NotKeyable]
		public double startTime = 0.0;
		[Space]
		[Range(0f, 1f)]
		[SerializeField, NotKeyable]
		public float volume = 1f;

		[SerializeField, NotKeyable]
		public bool mute = false;

		public override Playable CreatePlayable(PlayableGraph graph, GameObject go) {
			ScriptPlayable<AudioSourcePlayableBehaviour> playable =
				ScriptPlayable<AudioSourcePlayableBehaviour>.Create(graph);

			AudioSourcePlayableBehaviour playableBehaviour = playable.GetBehaviour();

			playableBehaviour.clipAudioSource = clipAudioSource.Resolve(graph.GetResolver());
			playableBehaviour.audioClip = audioClip;
			playableBehaviour.mute = mute;
			playableBehaviour.loop = loop;
			playableBehaviour.volume = volume;
			playableBehaviour.pitch = pitch;
			playableBehaviour.startTime = startTime;

			return playable;
		}

		public override double duration { // this is used as the default duration of the clip when created
			get {
				return audioClip == null || audioClip.length == 0 ? 30 : (audioClip.length - startTime) * pitch;
			}
		}
	}
}
