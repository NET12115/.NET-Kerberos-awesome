// -----------------------------------------------------------------------
// Licensed to The .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Security.Cryptography.Asn1;

namespace Kerberos.NET.Asn1
{
    internal static class Asn1Extension
    {
        public static bool HasValue<T>(T thing)
            where T : class
        {
            return thing != null;
        }

        public static bool HasValue<T>(T? thing)
            where T : struct
        {
            return thing.HasValue;
        }

        public static bool HasValue(Enum thing)
        {
            return thing != null;
        }

        internal static ReadOnlyMemory<byte> DepadLeft(this ReadOnlyMemory<byte> data)
        {
            var result = data;

            for (var i = 0; i < data.Length; i++)
            {
                if (data.Span[i] == 0)
                {
                    result = result.Slice(i + 1);
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        internal static ReadOnlyMemory<byte> PadRight(this ReadOnlyMemory<byte> data, int length)
        {
            if (data.Length == length)
            {
                return data;
            }

            var copy = new Memory<byte>(new byte[length]);

            data.CopyTo(copy.Slice(length - data.Length));

            return copy;
        }

        internal static void WriteKeyParameterInteger(this AsnWriter writer, ReadOnlySpan<byte> integer)
        {
            Debug.Assert(!integer.IsEmpty);

            if (integer[0] == 0)
            {
                int newStart = 1;

                while (newStart < integer.Length)
                {
                    if (integer[newStart] >= 0x80)
                    {
                        newStart--;
                        break;
                    }

                    if (integer[newStart] != 0)
                    {
                        break;
                    }

                    newStart++;
                }

                if (newStart == integer.Length)
                {
                    newStart--;
                }

                integer = integer.Slice(newStart);
            }

            writer.WriteIntegerUnsigned(integer);
        }
    }
}
