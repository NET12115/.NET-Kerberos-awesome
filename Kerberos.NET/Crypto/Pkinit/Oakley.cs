﻿using System;

namespace Kerberos.NET.Crypto
{
    public static class Oakley
    {
        private static byte[] Reverse(ReadOnlyMemory<byte> data)
        {
            var copy = new byte[data.Length];

            data.CopyTo(copy);

            Array.Reverse(copy);

            return copy;
        }

        public static class Group14
        {
            /*
             * https://tools.ietf.org/html/rfc3526#section-3
             * Id: 14
             * Prime: 2^2048 - 2^1984 - 1 + 2^64 * { [2^1918 pi] + 124476 }
             * Generator: 2
             */

            public static readonly ReadOnlyMemory<byte> Prime = new byte[]
            {
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xC9, 0x0F, 0xDA, 0xA2, 0x21, 0x68, 0xC2, 0x34,
                0xC4, 0xC6, 0x62, 0x8B, 0x80, 0xDC, 0x1C, 0xD1, 0x29, 0x02, 0x4E, 0x08, 0x8A, 0x67, 0xCC, 0x74,
                0x02, 0x0B, 0xBE, 0xA6, 0x3B, 0x13, 0x9B, 0x22, 0x51, 0x4A, 0x08, 0x79, 0x8E, 0x34, 0x04, 0xDD,
                0xEF, 0x95, 0x19, 0xB3, 0xCD, 0x3A, 0x43, 0x1B, 0x30, 0x2B, 0x0A, 0x6D, 0xF2, 0x5F, 0x14, 0x37,
                0x4F, 0xE1, 0x35, 0x6D, 0x6D, 0x51, 0xC2, 0x45, 0xE4, 0x85, 0xB5, 0x76, 0x62, 0x5E, 0x7E, 0xC6,
                0xF4, 0x4C, 0x42, 0xE9, 0xA6, 0x37, 0xED, 0x6B, 0x0B, 0xFF, 0x5C, 0xB6, 0xF4, 0x06, 0xB7, 0xED,
                0xEE, 0x38, 0x6B, 0xFB, 0x5A, 0x89, 0x9F, 0xA5, 0xAE, 0x9F, 0x24, 0x11, 0x7C, 0x4B, 0x1F, 0xE6,
                0x49, 0x28, 0x66, 0x51, 0xEC, 0xE4, 0x5B, 0x3D, 0xC2, 0x00, 0x7C, 0xB8, 0xA1, 0x63, 0xBF, 0x05,
                0x98, 0xDA, 0x48, 0x36, 0x1C, 0x55, 0xD3, 0x9A, 0x69, 0x16, 0x3F, 0xA8, 0xFD, 0x24, 0xCF, 0x5F,
                0x83, 0x65, 0x5D, 0x23, 0xDC, 0xA3, 0xAD, 0x96, 0x1C, 0x62, 0xF3, 0x56, 0x20, 0x85, 0x52, 0xBB,
                0x9E, 0xD5, 0x29, 0x07, 0x70, 0x96, 0x96, 0x6D, 0x67, 0x0C, 0x35, 0x4E, 0x4A, 0xBC, 0x98, 0x04,
                0xF1, 0x74, 0x6C, 0x08, 0xCA, 0x18, 0x21, 0x7C, 0x32, 0x90, 0x5E, 0x46, 0x2E, 0x36, 0xCE, 0x3B,
                0xE3, 0x9E, 0x77, 0x2C, 0x18, 0x0E, 0x86, 0x03, 0x9B, 0x27, 0x83, 0xA2, 0xEC, 0x07, 0xA2, 0x8F,
                0xB5, 0xC5, 0x5D, 0xF0, 0x6F, 0x4C, 0x52, 0xC9, 0xDE, 0x2B, 0xCB, 0xF6, 0x95, 0x58, 0x17, 0x18,
                0x39, 0x95, 0x49, 0x7C, 0xEA, 0x95, 0x6A, 0xE5, 0x15, 0xD2, 0x26, 0x18, 0x98, 0xFA, 0x05, 0x10,
                0x15, 0x72, 0x8E, 0x5A, 0x8A, 0xAC, 0xAA, 0x68, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF
            };

            public static readonly ReadOnlyMemory<byte> PrimeLittleEndian = Reverse(Prime);

            public static readonly ReadOnlyMemory<byte> Generator = new byte[]
            {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02
            };

            public static readonly ReadOnlyMemory<byte> GeneratorLittleEndian = Reverse(Generator);

            public static readonly ReadOnlyMemory<byte> Factor = new byte[]
            {
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x7f, 0x34, 0x55, 0x56, 0x45, 0x2d, 0x47, 0xb9, 0x0a,
                0x88, 0x02, 0x7d, 0x4c, 0x0c, 0x13, 0xe9, 0x8a, 0x72, 0xb5, 0x4a, 0x75, 0xbe, 0xa4, 0xca, 0x1c,
                0x8c, 0x0b, 0xac, 0x4a, 0xfb, 0xe5, 0x15, 0xef, 0x64, 0x29, 0xa6, 0x37, 0xf8, 0xae, 0xe2, 0xda,
                0x47, 0xd1, 0x03, 0x76, 0xd1, 0xc1, 0x93, 0xcd, 0x01, 0x43, 0x07, 0x0c, 0x96, 0x3b, 0xcf, 0xf1,
                0x1d, 0x67, 0x1b, 0x17, 0x23, 0x2f, 0x48, 0x19, 0xbe, 0x10, 0x0c, 0x65, 0x04, 0x36, 0xba, 0x78,
                0x02, 0x4c, 0x5e, 0x25, 0xa7, 0x1a, 0x86, 0xb3, 0x36, 0x4b, 0x4b, 0xb8, 0x83, 0x94, 0x6a, 0xcf,
                0x5d, 0xa9, 0x42, 0x10, 0xab, 0x79, 0x31, 0x0e, 0xcb, 0xd6, 0x51, 0xee, 0x91, 0xae, 0xb2, 0xc1,
                0xaf, 0x67, 0x92, 0x7e, 0xd4, 0x1f, 0x8b, 0x34, 0xcd, 0xe9, 0x2a, 0x0e, 0x1b, 0x24, 0x6d, 0xcc,
                0x82, 0xdf, 0xb1, 0x50, 0x5c, 0x3e, 0x00, 0xe1, 0x9e, 0x2d, 0x72, 0xf6, 0x28, 0x33, 0x94, 0x24,
                0xf3, 0x8f, 0x25, 0xbe, 0x08, 0x92, 0x4f, 0xd7, 0xd2, 0xcf, 0x44, 0xad, 0xfd, 0x35, 0x1c, 0xf7,
                0xf6, 0x5b, 0x03, 0x7a, 0x5b, 0xae, 0xff, 0x85, 0xb5, 0xf6, 0x1b, 0xd3, 0x74, 0x21, 0x26, 0x7a,
                0x63, 0x3f, 0x2f, 0x31, 0xbb, 0xda, 0x42, 0xf2, 0x22, 0xe1, 0xa8, 0xb6, 0xb6, 0x9a, 0xf0, 0xa7,
                0x1b, 0x8a, 0x2f, 0xf9, 0x36, 0x85, 0x15, 0x98, 0x8d, 0x21, 0x9d, 0xe6, 0xd9, 0x8c, 0xca, 0xf7,
                0x6e, 0x02, 0x1a, 0xc7, 0x3c, 0x04, 0xa5, 0x28, 0x91, 0xcd, 0x89, 0x1d, 0x53, 0xdf, 0x05, 0x01,
                0x3a, 0xe6, 0x33, 0x45, 0x04, 0x27, 0x81, 0x94, 0x68, 0x0e, 0x6e, 0xc0, 0x45, 0x31, 0x63, 0x62,
                0x1a, 0x61, 0xb4, 0x10, 0x51, 0xed, 0x87, 0xe4, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x7f
            };

            public static readonly ReadOnlyMemory<byte> FactorLittleEndian = Reverse(Factor);
        }

        public static class Group2
        {
            public static readonly ReadOnlyMemory<byte> Prime = new byte[]
            {
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xC9, 0x0F, 0xDA, 0xA2, 0x21, 0x68, 0xC2, 0x34,
                0xC4, 0xC6, 0x62, 0x8B, 0x80, 0xDC, 0x1C, 0xD1, 0x29, 0x02, 0x4E, 0x08, 0x8A, 0x67, 0xCC, 0x74,
                0x02, 0x0B, 0xBE, 0xA6, 0x3B, 0x13, 0x9B, 0x22, 0x51, 0x4A, 0x08, 0x79, 0x8E, 0x34, 0x04, 0xDD,
                0xEF, 0x95, 0x19, 0xB3, 0xCD, 0x3A, 0x43, 0x1B, 0x30, 0x2B, 0x0A, 0x6D, 0xF2, 0x5F, 0x14, 0x37,
                0x4F, 0xE1, 0x35, 0x6D, 0x6D, 0x51, 0xC2, 0x45, 0xE4, 0x85, 0xB5, 0x76, 0x62, 0x5E, 0x7E, 0xC6,
                0xF4, 0x4C, 0x42, 0xE9, 0xA6, 0x37, 0xED, 0x6B, 0x0B, 0xFF, 0x5C, 0xB6, 0xF4, 0x06, 0xB7, 0xED,
                0xEE, 0x38, 0x6B, 0xFB, 0x5A, 0x89, 0x9F, 0xA5, 0xAE, 0x9F, 0x24, 0x11, 0x7C, 0x4B, 0x1F, 0xE6,
                0x49, 0x28, 0x66, 0x51, 0xEC, 0xE6, 0x53, 0x81, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF
            };

            public static readonly ReadOnlyMemory<byte> PrimeLittleEndian = Reverse(Prime);

            public static readonly ReadOnlyMemory<byte> Generator = new byte[]
            {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02
            };

            public static readonly ReadOnlyMemory<byte> GeneratorLittleEndian = Reverse(Generator);

            public static readonly ReadOnlyMemory<byte> Factor = new byte[]
            {
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xc0, 0x29, 0x73, 0xf6, 0x28, 0x33, 0x94, 0x24,
                0xf3, 0x8f, 0x25, 0xbe, 0x08, 0x92, 0x4f, 0xd7, 0xd2, 0xcf, 0x44, 0xad, 0xfd, 0x35, 0x1c, 0xf7,
                0xf6, 0x5b, 0x03, 0x7a, 0x5b, 0xae, 0xff, 0x85, 0xb5, 0xf6, 0x1b, 0xd3, 0x74, 0x21, 0x26, 0x7a,
                0x63, 0x3f, 0x2f, 0x31, 0xbb, 0xda, 0x42, 0xf2, 0x22, 0xe1, 0xa8, 0xb6, 0xb6, 0x9a, 0xf0, 0xa7,
                0x1b, 0x8a, 0x2f, 0xf9, 0x36, 0x85, 0x15, 0x98, 0x8d, 0x21, 0x9d, 0xe6, 0xd9, 0x8c, 0xca, 0xf7,
                0x6e, 0x02, 0x1a, 0xc7, 0x3c, 0x04, 0xa5, 0x28, 0x91, 0xcd, 0x89, 0x1d, 0x53, 0xdf, 0x05, 0x01,
                0x3a, 0xe6, 0x33, 0x45, 0x04, 0x27, 0x81, 0x94, 0x68, 0x0e, 0x6e, 0xc0, 0x45, 0x31, 0x63, 0x62,
                0x1a, 0x61, 0xb4, 0x10, 0x51, 0xed, 0x87, 0xe4, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x7f
            };

            public static readonly ReadOnlyMemory<byte> FactorLittleEndian = Reverse(Factor);
        }
    }
}
