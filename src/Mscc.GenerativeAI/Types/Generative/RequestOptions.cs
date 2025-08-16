#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
#endif

namespace Mscc.GenerativeAI
{
    public class RequestOptions
    {
        public Retry Retry;
        public TimeSpan Timeout;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestOptions"/> class
        /// </summary>
        public RequestOptions()
        {
            Retry = new Retry();
            Timeout = new TimeSpan().Add(TimeSpan.FromSeconds(90));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestOptions"/> class
        /// </summary>
        /// <param name="retry">Refer to [retry docs](https://googleapis.dev/python/google-api-core/latest/retry.html) for details.</param>
        /// <param name="timeout">In seconds (or provide a [TimeToDeadlineTimeout](https://googleapis.dev/python/google-api-core/latest/timeout.html) object).</param>
        public RequestOptions(Retry? retry, TimeSpan? timeout) : this()
        {
            Retry = retry ?? Retry;
            Timeout = timeout ?? Timeout;
        }
    }

    public class Retry
    {
        public int Initial { get; set; }
        public int Multiplies { get; set; }
        public int Maximum { get; set; }
        public int Timeout { get; set; }
    }
}