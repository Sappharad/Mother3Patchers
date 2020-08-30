using System;
using System.IO;

namespace LibUPS
{
	public class LibUPS
	{
		public static void ApplyUPS(string inputPath, string outputPath, string patchPath){
			if (!File.Exists (inputPath)) {
                throw new FileNotFoundException("Impossible d’accéder au fichier d’entrée.");
			}
			if (!File.Exists (patchPath)) {
                throw new FileNotFoundException("Le fichier de patch n’a pas été trouvé. Il devrait être inclus dans le dossier d’installation.");
			}
			byte[] cache = new byte[4];
			FileStream inFile = new FileStream (inputPath, FileMode.Open, FileAccess.Read);
			FileStream patchFile = new FileStream (patchPath, FileMode.Open, FileAccess.Read);
			FileStream outFile = new FileStream (outputPath, FileMode.Create, FileAccess.ReadWrite);
			if (patchFile.Length < 16) {
				inFile.Close ();
				patchFile.Close ();
				outFile.Close ();
				throw new Exception ("Patch file is too small!");
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
                throw new Exception("Le fichier d’entrée est incorrect.");
			}

			patchFile.Seek (0, SeekOrigin.Begin);
			patchFile.Read (cache, 0, 4);
			if(cache[0] != 'U' && cache[1] != 'P' && cache[2] != 'S' && cache[3] != '1'){
				inFile.Close ();
				patchFile.Close ();
				outFile.Close ();
                throw new InvalidDataException("Une erreur est survenue lors de l’application du patch.");
			}

			ulong size_input = decptr (patchFile);
			ulong size_output = decptr (patchFile);
            uint inputCrc = CalculateFileCRC32(inFile, (long)size_input);
            bool reversePatch = (crc_out == inputCrc);
			if(crc_in != inputCrc && crc_out != inputCrc){ //Allow unpatching by accepting the output file too
				inFile.Close ();
				patchFile.Close ();
				outFile.Close ();
                throw new Exception("Le fichier d’entrée est incorrect.");
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

			if((!reversePatch && crc_out != CalculateFileCRC32(outFile, (long)size_output)) ||
                (reversePatch && crc_in != CalculateFileCRC32(outFile, (long)size_output)))
            {
				inFile.Close ();
				patchFile.Close ();
				outFile.Close ();
                throw new Exception("Une erreur est survenue lors de l’application du patch.");
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
