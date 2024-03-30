/*
 *  Copyright (c) 2018 Stanislav Denisov
 *
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *
 *  The above copyright notice and this permission notice shall be included in all
 *  copies or substantial portions of the Software.
 *
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *  SOFTWARE.
 */

/*
 * ArrayPoolEventSource.cs
 *
 * This file contains the implementation of the ArrayPoolEventSource class, which is used to log events related to the allocation, renting, and returning of buffers in an array pool.
 *
 * The ArrayPoolEventSource class has methods for logging when a buffer is allocated, rented, and returned. It also has an enumeration, BufferAllocatedReason, which indicates the reason why a buffer was allocated.
 *
 * The Log class is a helper class that formats and outputs the log messages. Depending on the platform, it either outputs to the console or uses Unity's Debug.Log method.
 *
 * Usage:
 *
 * ArrayPoolEventSource.EventLog.BufferAllocated(bufferId, bufferSize, poolId, bucketId, reason);
 * ArrayPoolEventSource.EventLog.BufferRented(bufferId, bufferSize, poolId, bucketId);
 * ArrayPoolEventSource.EventLog.BufferReturned(bufferId, bufferSize, poolId);
 */

using System;

#if ENABLE_MONO || ENABLE_IL2CPP
	using UnityEngine;
#endif

namespace NetStack.Buffers {
	internal sealed class ArrayPoolEventSource {
		#if NETSTACK_BUFFERS_LOG
			internal static readonly ArrayPoolEventSource EventLog = new ArrayPoolEventSource();

			internal enum BufferAllocatedReason : int {
				Pooled,
				OverMaximumSize,
				PoolExhausted
			}

			internal void BufferAllocated(int bufferId, int bufferSize, int poolId, int bucketId, BufferAllocatedReason reason) {
				var message = "Buffer allocated (Buffer ID: " + bufferId + ", Buffer size: " + bufferSize + ", Pool ID: " + poolId + ", Bucket ID: " + bucketId + ", Reason: " + reason + ")";

				if (reason == BufferAllocatedReason.Pooled)
					Log.Info("Buffers", message);
				else
					Log.Warning("Buffers", message);
			}

			internal void BufferRented(int bufferId, int bufferSize, int poolId, int bucketId) {
				Log.Info("Buffers", "Buffer rented (Buffer ID: " + bufferId + ", Buffer size: " + bufferSize + ", Pool ID: " + poolId + ", Bucket ID: " + bucketId + ")");
			}

			internal void BufferReturned(int bufferId, int bufferSize, int poolId) {
				Log.Info("Buffers", "Buffer returned (Buffer ID: " + bufferId + ", Buffer size: " + bufferSize + ", Pool ID: " + poolId + ")");
			}
		#endif
	}

	internal static class Log {
		private static string Output(string module, string message) {
			return DateTime.Now.ToString("[HH:mm:ss]") + " [NetStack." + module + "] " + message;
		}

		public static void Info(string module, string message) {
			#if ENABLE_MONO || ENABLE_IL2CPP
				Debug.Log(Output(module, message));
			#else
				Console.WriteLine(Output(module, message));
			#endif
		}

		public static void Warning(string module, string message) {
			#if ENABLE_MONO || ENABLE_IL2CPP
				Debug.LogWarning(Output(module, message));
			#else
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine(Output(module, message));
				Console.ResetColor();
			#endif
		}
	}
}