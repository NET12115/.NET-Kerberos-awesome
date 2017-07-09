﻿using Syfuhs.Security.Kerberos.Crypto;
using Syfuhs.Security.Kerberos.Entities;
using System;
using System.Security;

namespace Syfuhs.Security.Kerberos.Aes
{
    public abstract class AESDecryptor : KerberosEncryptor
    {
        public AESDecryptor(IEncryptor encryptor, IHasher hasher) 
            : base(encryptor, hasher)
        {
        }

        public override int ChecksumSize { get { return 96 / 8; } }

        protected virtual byte[] DecryptWith(byte[] workBuffer, int[] workLens, byte[] key, byte[] iv, KeyUsage usage)
        {
            var confounderLen = workLens[0];
            var checksumLen = workLens[1];
            var dataLen = workLens[2];

            byte[] Ki;
            byte[] Ke;

            var constant = new byte[5];

            ConvertBytes((int)usage, constant, 0);

            constant[4] = 170;

            var encryptor = (AESEncryptor)Encryptor;

            Ke = encryptor.DK(key, constant);

            constant[4] = 85;

            Ki = encryptor.DK(key, constant);

            var tmpEnc = new byte[confounderLen + dataLen];
            Buffer.BlockCopy(workBuffer, 0, tmpEnc, 0, (confounderLen + dataLen));

            var checksum = new byte[checksumLen];

            Buffer.BlockCopy(workBuffer, confounderLen + dataLen, checksum, 0, checksumLen);

            encryptor.Decrypt(Ke, iv, tmpEnc);

            var newChecksum = MakeChecksum(Ki, tmpEnc, checksumLen);

            if (!SlowCompare(checksum, newChecksum))
            {
                throw new SecurityException("Invalid checksum");
            }

                            var data = new byte[dataLen];

            Buffer.BlockCopy(tmpEnc, confounderLen, data, 0, dataLen);

            return data;
        }

        public override byte[] Decrypt(byte[] cipher, KerberosKey key, KeyUsage usage)
        {
            var iv = new byte[Encryptor.BlockSize];
            return Decrypt(cipher, key.GetKey(Encryptor), iv, usage);
        }

        private byte[] Decrypt(byte[] cipher, byte[] key, byte[] iv, KeyUsage usage)
        {
            var totalLen = cipher.Length;
            var confounderLen = Encryptor.BlockSize;
            var checksumLen = ChecksumSize;
            var dataLen = totalLen - (confounderLen + checksumLen);

            var lengths = new int[] { confounderLen, checksumLen, dataLen };

            return DecryptWith(cipher, lengths, key, iv, usage);
        }

        private static void ConvertBytes(int val, byte[] bytes, int offset)
        {
            bytes[offset + 0] = (byte)((val >> 24) & 0xff);
            bytes[offset + 1] = (byte)((val >> 16) & 0xff);
            bytes[offset + 2] = (byte)((val >> 8) & 0xff);
            bytes[offset + 3] = (byte)((val) & 0xff);
        }
    }
}
