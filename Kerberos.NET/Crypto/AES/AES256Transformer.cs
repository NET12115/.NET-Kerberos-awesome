// -----------------------------------------------------------------------
// Licensed to The .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// -----------------------------------------------------------------------

#pragma warning disable S101 // Types should be named in camel case

namespace Kerberos.NET.Crypto
{
    internal class AES256Transformer : AESTransformer
    {
        private const int Size = 32;

        public AES256Transformer()
            : base(Size)
        {
        }

        public override ChecksumType ChecksumType => ChecksumType.HMAC_SHA1_96_AES256;

        public override EncryptionType EncryptionType => EncryptionType.AES256_CTS_HMAC_SHA1_96;
    }
}
