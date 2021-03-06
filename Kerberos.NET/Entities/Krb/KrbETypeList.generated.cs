// -----------------------------------------------------------------------
// Licensed to The .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// -----------------------------------------------------------------------

// This is a generated file.
// The generation template has been modified from .NET Runtime implementation

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.Asn1;
using Kerberos.NET.Crypto;
using Kerberos.NET.Asn1;

namespace Kerberos.NET.Entities
{
    public partial class KrbETypeList
    {
        /*
          EtypeList ::= SEQUENCE OF Int32
                        - - Specifies the enctypes supported by the client.
                        - - This enctype list is in decreasing preference order
                        - - (favorite choice first).
                        - - Int32 is defined in [RFC4120].
         */
    
        public EncryptionType[] List { get; set; }
  
#if DEBUG
        static KrbETypeList()
        {
            var usedTags = new System.Collections.Generic.Dictionary<Asn1Tag, string>();
            Action<Asn1Tag, string> ensureUniqueTag = (tag, fieldName) =>
            {
                if (usedTags.TryGetValue(tag, out string existing))
                {
                    throw new InvalidOperationException($"Tag '{tag}' is in use by both '{existing}' and '{fieldName}'");
                }

                usedTags.Add(tag, fieldName);
            };
            
            ensureUniqueTag(Asn1Tag.Sequence, "List");
        }
#endif
        // Encoding methods
        public ReadOnlyMemory<byte> Encode()
        {
            var writer = new AsnWriter(AsnEncodingRules.DER);

            Encode(writer);

            return writer.EncodeAsMemory();
        }

        internal void Encode(AsnWriter writer)
        {
            bool wroteValue = false; 
            
            if (List != null)
            {
                if (wroteValue)
                {
                    throw new CryptographicException();
                }
                
                writer.PushSequence();
            
                for (int i = 0; i < List.Length; i++)
                {
                    writer.WriteInteger((long)List[i]); 
                }

                writer.PopSequence();

                wroteValue = true;
            }

            if (!wroteValue)
            {
                throw new CryptographicException();
            }
        }
                
        internal ReadOnlyMemory<byte> EncodeApplication(Asn1Tag tag)
        {
            using (var writer = new AsnWriter(AsnEncodingRules.DER))
            {
                writer.PushSequence(tag);
                
                this.Encode(writer);

                writer.PopSequence(tag);

                return writer.EncodeAsMemory();
            }
        }
        
        public static KrbETypeList Decode(ReadOnlyMemory<byte> data)
        {
            return Decode(data, AsnEncodingRules.DER);
        }

        internal static KrbETypeList Decode(ReadOnlyMemory<byte> encoded, AsnEncodingRules ruleSet)
        {
            AsnReader reader = new AsnReader(encoded, ruleSet);
            
            Decode(reader, out KrbETypeList decoded);
            reader.ThrowIfNotEmpty();
            return decoded;
        }

        internal static void Decode<T>(AsnReader reader, out T decoded)
          where T: KrbETypeList, new()
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            decoded = new T();
            
            Asn1Tag tag = reader.PeekTag();
            AsnReader collectionReader;
            
            if (tag.HasSameClassAndValue(Asn1Tag.Sequence))
            {
                // Decode SEQUENCE OF for List
                {
                    collectionReader = reader.ReadSequence();
                    var tmpList = new List<EncryptionType>();
                    EncryptionType tmpItem;

                    while (collectionReader.HasData)
                    {

                        if (!collectionReader.TryReadInt32(out EncryptionType tmp))
                        {
                            collectionReader.ThrowIfNotEmpty();
                        }
            
            tmpItem = tmp; 
                        tmpList.Add(tmpItem);
                    }

                    decoded.List = tmpList.ToArray();
                }
            }
            else
            {
                throw new CryptographicException();
            }
        }
    }
}
