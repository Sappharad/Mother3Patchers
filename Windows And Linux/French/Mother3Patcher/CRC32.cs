using System;
using System.Security.Cryptography;

namespace LibUPS
{
	public class CRC32
	{
		// Shared, pre-computed lookup table for efficiency
		private static readonly uint[] _crc32Table;

		/// <summary>
		/// Initializes the shared lookup table.
		/// </summary>
		static CRC32()
		{
			// Allocate table
			_crc32Table = new uint[256];

			// For each byte
			for (uint n = 0; n < 256; n++)
			{
				// For each bit
				uint c = n;
				for (int k = 0; k < 8; k++)
				{
					// Compute value
					if (0 != (c & 1))
					{
						c = 0xedb88320 ^ (c >> 1);
					}
					else
					{
						c = c >> 1;
					}
				}

				// Store result in table
				_crc32Table[n] = c;
			}
		}

		// Current hash value
		private uint _crc32Value;

		// True if HashCore has been called
		private bool _hashCoreCalled;

		// True if HashFinal has been called
		private bool _hashFinalCalled;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public CRC32()
		{
			InitializeVariables();
		}

		/// <summary>
		/// Initializes internal state.
		/// </summary>
		public void Initialize()
		{
			InitializeVariables();
		}

		/// <summary>
		/// Initializes variables.
		/// </summary>
		private void InitializeVariables()
		{
			_crc32Value = uint.MaxValue;
			_hashCoreCalled = false;
			_hashFinalCalled = false;
		}

		/// <summary>
		/// Updates the hash code for the provided data.
		/// </summary>
		/// <param name="array">Data.</param>
		/// <param name="ibStart">Start position.</param>
		/// <param name="cbSize">Number of bytes.</param>
		public void HashCore(byte[] array, int ibStart, int cbSize)
		{
			if (null == array)
			{
				throw new ArgumentNullException("array");
			}

			/*if (_hashFinalCalled)
			{
				throw new CryptographicException(
					"Hash not valid for use in specified state.");
			}
			_hashCoreCalled = true;*/

			for (int i = ibStart; i < ibStart + cbSize; i++)
			{
				byte index = (byte)(_crc32Value ^ array[i]);
				_crc32Value = _crc32Table[index] ^ ((_crc32Value >> 8) & 0xffffff);
			}
		}

		/// <summary>
		/// Finalizes the hash code and returns it.
		/// </summary>
		/// <returns></returns>
		public byte[] HashFinal()
		{
			_hashFinalCalled = true;
			return Hash;
		}

		/// <summary>
		/// Returns the hash as an array of bytes.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
		                                                 "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification =
		                                                 "Matching .NET behavior by throwing here.")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
		                                                 "CA2201:DoNotRaiseReservedExceptionTypes", Justification =
		                                                 "Matching .NET behavior by throwing NullReferenceException.")]
		public byte[] Hash
		{
			get
			{
				/*if (!_hashCoreCalled)
				{
					throw new NullReferenceException();
				}*/
				if (!_hashFinalCalled)
				{
					// Note: Not CryptographicUnexpectedOperationException because
					// that can't be instantiated on Silverlight 4
					throw new CryptographicException(
						"Hash must be finalized before the hash value is retrieved.");
				}

				// Convert complement of hash code to byte array
				byte[] bytes = BitConverter.GetBytes(~_crc32Value);

				// Reverse for proper endianness, and return
				Array.Reverse(bytes);
				return bytes;
			}
		}

		// Return size of hash in bits.
		public int HashSize
		{
			get
			{
				return 4 * 8;
			}
		}
	}
}

