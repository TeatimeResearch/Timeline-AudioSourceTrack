using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace UnityEngine.Timeline {
	public sealed class AudioSourceInitPlayableBehaviour : PlayableBehaviour {
		private IEnumerable<TimelineClip> m_Clips;
		private PlayableDirector m_Director;

		internal PlayableDirector director {
			get {
				return m_Director;
			}
			set {
				m_Director = value;
			}
		}

		internal IEnumerable<TimelineClip> clips {
			get {
				return m_Clips;
			}
			set {
				m_Clips = value;
			}
		}

		public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
			if ( m_Clips == null )
				return;

			int inputPort = 0;
			foreach ( TimelineClip clip in m_Clips ) {
				ScriptPlayable<AudioSourcePlayableBehaviour> scriptPlayable =
					(ScriptPlayable<AudioSourcePlayableBehaviour>)playable.GetInput(inputPort);

				AudioSourcePlayableBehaviour audioSourcePlayableBehaviour = scriptPlayable.GetBehaviour();

				if ( audioSourcePlayableBehaviour != null ) {
					audioSourcePlayableBehaviour.trackAudioSource = (AudioSource)playerData;
				}
				++inputPort;
			}
		}
	}
}
