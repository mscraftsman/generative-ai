namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Options for audio generation.
    /// </summary>
    public class AudioOptions
    {
        /// <summary>
        /// Optional. The format of the audio response.
        /// </summary>
        /// <remarks>
        /// Can be either:
        /// - "wav": Format the response as a WAV file.
        /// - "mp3": Format the response as an MP3 file.
        /// - "flac": Format the response as a FLAC file.
        /// - "opus": Format the response as an OPUS file.
        /// - "pcm16": Format the response as a PCM16 file.
        /// </remarks>
        public string? Format { get; set; }
        /// <summary>
        /// Optional. The voice to use for the audio response.
        /// </summary>
        public string? Voice { get; set; }
    }
}