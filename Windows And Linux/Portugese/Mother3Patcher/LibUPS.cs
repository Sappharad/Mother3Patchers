using System;
using System.IO;

namespace LibUPS
{
	public class LibUPS
	{
		public static void ApplyUPS(string inputPath, string outputPath, string patchPath){
			if (!File.Exists (inputPath)) {
                throw new FileNotFoundException("Arquivo de entrada n�o encontrado!");
			}
			if (!File.Exists (patchPath)) {
                throw new FileNotFoundException("Arquivo de patch n�o encontrado!");
			}
			byte[] cache = new byte[4];
			FileStream inFile = new FileStream (inputPath, FileMode.Open, FileAccess.Read);
			FileStream patchFile = new FileStream (patchPath, FileMode.Open, FileAccess.Read);
			FileStream outFile = new FileStream (outputPath, FileMode.Create, FileAccess.ReadWrite);
			if (patchFile.Length < 16) {
				inFile.Close ();
				patchFile.Close ();
				outFile.Close ();
                throw new Exception("Arquivo de corre��o � muito pequeno!");
			}
			patchFile.Seek (patchFile.Length-12, SeekOrigin.Begin);
			uint crc_in = Read32Bit (patchFile);
			uint crc_out = Read32Bit (patchFile);
			uint crc_patch = Read32Bit (patchFile);

			//Calculate patch CRC
			if(crc_patch != CalculateFileCRC32(patchFile, patchFile.Length-4)){
				inFile.Close ();
				patchFile.Close ();
				outFile.Close ();
                throw new Exception("Patch � corrupto!");
			}

			patchFile.Seek (0, SeekOrigin.Begin);
			patchFile.Read (cache, 0, 4);
			if(cache[0] != 'U' && cache[1] != 'P' && cache[2] != 'S' && cache[3] != '1'){
				inFile.Close ();
				patchFile.Close ();
				outFile.Close ();
                throw new InvalidDataException("N�o � um arquivo UPS!");
			}

			ulong size_input = decptr (patchFile);
			ulong size_output = decptr (patchFile);
			if(crc_in != CalculateFileCRC32(inFile, (long)size_input)){
				inFile.Close ();
				patchFile.Close ();
				outFile.Close ();
                throw new Exception("ROM de entrada n�o � o arquivo correto!");
			}

			inFile.Seek (0, SeekOrigin.Begin);
			for (ulong i = 0; i < size_input && i < size_output; i++)
				outFile.WriteByte ((byte)inFile.ReadByte ()); //Todo: This is probably slow and we should read 4k chunks
			for(ulong i = size_input; i < size_output; i++) outFile.WriteByte(0x00);
			//We just copied input file to output file

			inFile.Seek (0, SeekOrigin.Begin);
			outFile.Seek (0, SeekOrigin.Begin);

			ulong relative = 0;
			while(patchFile.Position < patchFile.Length - 12) {
				relative += decptr(patchFile);
				if(relative >= size_output) continue;

				inFile.Seek ((long)relative, SeekOrigin.Begin);
				outFile.Seek ((long)relative, SeekOrigin.Begin);
				for(ulong i = relative; i < size_output; i++) {
					byte x = (byte)patchFile.ReadByte();
					relative++;
					if(x==0) break;
					if(i < size_output) {
						byte y = (i < size_input) ? (byte)inFile.ReadByte() : (byte)0x00;
						outFile.WriteByte ((byte)(x ^ y));
					}
				}
			}

			if(crc_out != CalculateFileCRC32(outFile, (long)size_output)){
				inFile.Close ();
				patchFile.Close ();
				outFile.Close ();
                throw new Exception("O arquivo de sa�da n�o est� correto!");
			}

			inFile.Close ();
			outFile.Close ();
			patchFile.Close ();
		}

		public static uint CalculateFileCRC32(FileStream fs, long length){
			byte[] cache = new byte[4096]; //Up to 4KB at a time
			long remains = length;
			fs.Seek (0, SeekOrigin.Begin);
			CRC32 crc = new CRC32 ();
			crc.Initialize ();
			int bytesRead = 0;
			while ((bytesRead = fs.Read(cache, 0, cache.Length)) > 0) {
				remains -= bytesRead;
				if (remains <= 0) {
					bytesRead = (bytesRead + (int)remains);
				}
				crc.HashCore (cache, 0, bytesRead);
			}
			byte[] hash = crc.HashFinal();
			return (uint)(hash[3] + (hash[2] << 8) +(hash[1] << 16) + (hash[0] << 24));
		}

		public static uint Read32Bit(FileStream fs, bool bigEndian=false){
			byte[] block = new byte[4];
			fs.Read (block, 0, 4);

			if (bigEndian) {
				return (uint)(block [3] + (block [2] << 8) + (block [1] << 16) + (block [0] << 24));
			}
			return (uint)(block [0] + (block [1] << 8) + (block [2] << 16) + (block [3] << 24));
		}

		public static ulong decptr(FileStream fs) {
			ulong offset = 0, shift = 1;
			while(true) {
				byte x = (byte)fs.ReadByte();
				offset += ((ulong)(x & 0x7f) * shift);
				if((x & 0x80) > 0) break;
				shift <<= 7;
				offset += shift;
			}
			return offset;
		}
	}
}
