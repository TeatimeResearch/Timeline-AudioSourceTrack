using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;

namespace UnityEngine.Timeline {
	public class AudioSourcePlayableBehaviour : PlayableBehaviour {
		public AudioSource trackAudioSource; // not visible in inspector
		public AudioSource clipAudioSource;
		public AudioSource audioSource {
			get => clipAudioSource != null ? clipAudioSource : trackAudioSource;
		}
		public AudioClip audioClip;
		public bool mute = false;

		public float volume = 1f;
		public float pitch = 1f;

		public bool loop = true;
		public double startTime = 0.0;

		private bool playedOnce = false;
		private bool prepared = false;

		public void PrepareAudio() {
			if ( prepared ) return;
			if ( (trackAudioSource == null && audioSource == null) || audioClip == null ) return;

			if ( audioSource.clip != audioClip ) StopAudio();

			audioSource.clip = audioClip;
			audioSource.playOnAwake = false;
			audioSource.loop = loop;
			audioSource.volume = volume;
			audioSource.pitch = pitch;

			audioSource.mute = mute;

			audioSource.time = (float)startTime;
			prepared = true;
		}

		public override void PrepareFrame(Playable playable, FrameData info) {
			if ( !Application.isPlaying ) return;
			if ( audioSource == null || audioClip == null ) return;

			// audioSource.timeReference = Application.isPlaying ? VideoTimeReference.ExternalTime : VideoTimeReference.Freerun;
			//
			// if ( audioSource.isPlaying && Application.isPlaying )
			// 	audioSource.externalReferenceTime = playable.GetTime();
			// else if ( !Application.isPlaying )
			// 	SyncVideoToPlayable(playable);
		}

		public override void OnBehaviourPlay(Playable playable, FrameData info) {
			if ( audioSource == null ) return;

			if ( !playedOnce ) {
				PlayAudio();
				SyncAudioToPlayable(playable);
			}
		}

		public override void OnBehaviourPause(Playable playable, FrameData info) {
			if ( audioSource == null ) return;

			if ( Application.isPlaying ) {
				PauseAudio();
			} else {
				StopAudio();
			}
		}

		public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
			if ( !Application.isPlaying ) return;

			if ( audioSource == null || audioSource.clip == null ) return;

			audioSource.volume = info.weight * volume;
		}

		public override void OnGraphStart(Playable playable) {
			if ( !Application.isPlaying ) return;
			playedOnce = false;
		}

		public override void OnGraphStop(Playable playable) { }

		public override void OnPlayableDestroy(Playable playable) {
			if ( !Application.isPlaying ) return;
			StopAudio();
		}

		public void PlayAudio() {
			if ( !Application.isPlaying ) return;
			if ( audioSource == null ) return;

			PrepareAudio();

			audioSource.Play();
			playedOnce = true;
		}

		public void PauseAudio() {
			if ( audioSource == null ) return;

			audioSource.Pause();
			prepared = false;
		}

		public void StopAudio() {
			if ( audioSource == null ) return;

			playedOnce = false;
			audioSource.Stop();
			prepared = false;
		}

		private void SyncAudioToPlayable(Playable playable) {
			if ( audioSource == null || audioSource.clip == null ) return;

			audioSource.time = Mathf.Min((float)((startTime + playable.GetTime()) * pitch), audioSource.clip.length);
		}
	}
}
