﻿/******************************************************************************
 * MediaInfo.NET - A fast, easy-to-use .NET wrapper for MediaInfo.
 * Use at your own risk, under the same license as MediaInfo itself.
 * Copyright (C) 2012 Charles N. Burns
 * 
 * Official source code: https://code.google.com/p/mediainfo-dot-net/
 * 
 ******************************************************************************
 *
 * AudioStream.cs
 * 
 * Presents information and functionality specific to an audio stream.
 * 
 ******************************************************************************
 */

using System;
using MediaInfoLib;
using System.Collections.Generic;

namespace MediaInfoDotNet.Models
{
	///<summary>Represents a single audio stream.</summary>
	public sealed class AudioStream : Media
	{
		readonly MultiStreamCommon streamCommon;

		///<summary>AudioStream constructor.</summary>
		///<param name="mediaInfo">A MediaInfo object.</param>
		///<param name="id">The MediaInfo ID for this audio stream.</param>
		public AudioStream(MediaInfo mediaInfo, int id)
			: base(mediaInfo, id) {
			kind = StreamKind.Audio;
			streamCommon = new MultiStreamCommon(mediaInfo, kind, id);
		}

		#region AllStreamsCommon
		///<summary>The format or container of this file or stream.</summary>
		public string format { get { return streamCommon.format; } }

		///<summary>The title of this stream.</summary>
		public string title { get { return streamCommon.title; } }

		///<summary>This stream's globally unique ID (GUID).</summary>
		public string uniqueId { get { return streamCommon.uniqueId; } }
		#endregion

		#region GeneralVideoAudioTextImageMenuCommon
		///<summary>Codec ID available from some codecs.</summary>
		///<example>AAC audio:A_AAC, h.264 video:V_MPEG4/ISO/AVC</example>
		public string codecId { get { return streamCommon.codecId; } }

		///<summary>Common name of the codec.</summary>
		public string codecCommonName { get { return streamCommon.codecCommonName; } }
		#endregion

		#region GeneralVideoAudioTextMenu
		///<summary>Stream delay (e.g. to sync audio/video) in ms.</summary>
		public int delay { get { return streamCommon.delay; } }

		///<summary>Duration of the stream in milliseconds.</summary>
		public int duration { get { return streamCommon.duration; } }
		#endregion

		#region GeneralVideoAudioTextImageCommon
		///<summary>Date and time stream encoding completed.</summary>
		public DateTime encodedDate { get { return streamCommon.encodedDate; } }

		///<summary>Software used to encode this stream.</summary>
		public string encodedLibrary { get { return streamCommon.encoderLibrary; } }

		///<summary>Media type of stream, formerly called MIME type.</summary>
		public string internetMediaType { get { return streamCommon.internetMediaType; } }

		///<summary>Size in bytes.</summary>
		public long size { get { return streamCommon.size; } }

		///<summary>Encoder settings used for encoding this stream.
		///String format: name=value / name=value / ...</summary>
		public string encoderSettingsRaw { get { return streamCommon.encoderSettingsRaw; } }

		///<summary>Encoder settings used for encoding this stream.</summary>
		public IDictionary<string, string> encoderSettings { get { return streamCommon.encoderSettings; } }
		#endregion

		#region VideoAudioTextCommon
		///<summary>The bit rate of this stream, in bits per second</summary>
		public int bitRate { get { return streamCommon.bitRate; } }

		///<summary>The maximum bitrate of this stream in BPS.</summary>
		public int bitRateMaximum { get { return streamCommon.bitRateMaximum; } }

		///<summary>The minimum bitrate of this stream in BPS.</summary>
		public int bitRateMinimum { get { return streamCommon.bitRateMinimum; } }

		///<summary>The maximum allowed bitrate, in BPS, with the encoder
		/// settings used. Some encoders report the average BPS.</summary>
		public int bitRateNominal { get { return streamCommon.bitRateNominal; } }

		///<summary>Mode (CBR, VBR) used for bit allocation.</summary>
		public string bitRateMode { get { return streamCommon.bitRateMode; } }

		///<summary>How the stream is muxed into the container.</summary>
		public string muxingMode { get { return streamCommon.muxingMode; } }

		///<summary>The total number of frames (e.g. video frames).</summary>
		public int frameCount { get { return streamCommon.frameCount; } }

		///<summary>Frame rate of the stream in frames per second.</summary>
		public float frameRate { get { return streamCommon.frameRate; } }
		#endregion

		#region VideoAudioTextImageCommon
		///<summary>Compression mode (lossy or lossless).</summary>
		public string compressionMode { get { return streamCommon.compressionMode; } }

		///<summary>Ratio of current size to uncompressed size.</summary>
		public string compressionRatio { get { return streamCommon.compressionRatio; } }

		///<example>Stream bit depth (16, 24, 32...)</example>
		public int bitDepth { get { return streamCommon.bitDepth; } }
		#endregion

		#region VideoAudioTextImageMenuCommon
		///<summary>2-letter (if available) or 3-letter ISO code.</summary>
		public string language { get { return streamCommon.language; } }
		#endregion

		#region Audio
		int _channels = int.MinValue;
		///<summary>Number of audio channels, e.g. 6 for 5.1 audio.</summary>
		public int channels {
			get {
				if(_channels == int.MinValue)
					_channels = miGetInt("Channel(s)");
				return _channels;
			}
		}

		int _sampleRate = int.MinValue;
		///<summary>Audio sample rate, e.g. 44100 for CD audio.</summary>
		public int sampleRate {
			get {
				if(_sampleRate == int.MinValue)
					_sampleRate = miGetInt("SamplingRate");
				return _sampleRate;
			}
		}
		#endregion
	}
}
