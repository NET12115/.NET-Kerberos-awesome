// -----------------------------------------------------------------------
// Licensed to The .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kerberos.NET;
using Kerberos.NET.Crypto;
using Kerberos.NET.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Kerberos.NET
{
    [TestClass]
    public class DelegationTests : BaseTest
    {
        [TestMethod]
        public void DelegationEncoding_NoTicket()
        {
            var delegInfo = new DelegationInfo() { };

            var encoded = delegInfo.Encode();

            Assert.IsNotNull(encoded);
        }

        [TestMethod]
        public void DelegationEncoding_Roundtrip()
        {
            var delegInfo = new DelegationInfo()
            {
                DelegationTicket = new KrbCred
                {
                    Tickets = new[]
                    {
                        new KrbTicket
                        {
                            EncryptedPart = new KrbEncryptedData { Cipher = new byte[16], EType = EncryptionType.AES128_CTS_HMAC_SHA1_96 },
                            Realm = "blah.test.com",
                            SName = KrbPrincipalName.FromString("blah@test.com"),
                            TicketNumber = 245
                        }
                    },
                    EncryptedPart = new KrbEncryptedData { Cipher = new byte[16], EType = EncryptionType.AES128_CTS_HMAC_SHA1_96 }
                }
            };

            var encoded = delegInfo.Encode();

            Assert.IsNotNull(encoded);

            var decoded = new DelegationInfo().Decode(encoded);

            Assert.IsNotNull(decoded);
            Assert.IsNotNull(decoded.DelegationTicket);
            Assert.IsNotNull(decoded.DelegationTicket.Tickets);

            Assert.AreEqual(1, decoded.DelegationTicket.Tickets.Length);

            Assert.IsNotNull(decoded.DelegationTicket.EncryptedPart);
        }

        [TestMethod]
        public async Task DelegationRetrieval()
        {
            var validator = new KerberosValidator(new KerberosKey("P@ssw0rd!")) { ValidateAfterDecrypt = DefaultActions };

            var data = await validator.Validate(Convert.FromBase64String(TicketContainingDelegation));

            Assert.IsNotNull(data);

            var cred = data.DelegationTicket;

            Assert.IsNotNull(cred);

            Assert.AreEqual(1, cred.TicketInfo.Length);

            var ticket = cred.TicketInfo.First();

            Assert.AreEqual("Administrator", ticket.PName.Name.First());
            Assert.AreEqual("krbtgt/CORP.IDENTITYINTERVENTION.COM", ticket.SName.FullyQualifiedName);

            Assert.IsNotNull(ticket.Key);
            Assert.IsNotNull(ticket.Key.KeyValue);
        }

        [TestMethod]
        public void CredPartRoundtrip()
        {
            KrbEncKrbCredPart part = new KrbEncKrbCredPart
            {
                Nonce = 123,
                RAddress = new KrbHostAddress
                {
                    Address = Encoding.ASCII.GetBytes("blaaaaaaaah"),
                    AddressType = AddressType.NetBios
                },
                SAddress = new KrbHostAddress
                {
                    Address = Encoding.ASCII.GetBytes("server"),
                    AddressType = AddressType.NetBios
                },
                Timestamp = DateTimeOffset.UtcNow,
                USec = 123,
                TicketInfo = new[]
                {
                    new KrbCredInfo
                    {
                        AuthorizationData = new KrbAuthorizationData[]
                        {
                            new KrbAuthorizationData
                            {
                                Data = Array.Empty<byte>(),
                                Type = AuthorizationDataType.AdAndOr
                            }
                        },
                        AuthTime = DateTimeOffset.UtcNow,
                        EndTime = DateTimeOffset.UtcNow,
                        RenewTill = DateTimeOffset.UtcNow,
                        Flags = TicketFlags.Anonymous,
                        Key = KrbEncryptionKey.Generate(EncryptionType.AES128_CTS_HMAC_SHA1_96),
                        PName = new KrbPrincipalName
                        {
                            Name = new[] { "pname" },
                            Type = PrincipalNameType.NT_ENTERPRISE
                        },
                        Realm = "realm.com",
                        SName = new KrbPrincipalName
                        {
                            Name = new[] { "server" },
                            Type = PrincipalNameType.NT_ENTERPRISE
                        },
                        SRealm = "srealm.com",
                        StartTime = DateTimeOffset.UtcNow
                    }
                }
            };

            var encoded = part.EncodeApplication();

            var decoded = KrbEncKrbCredPart.DecodeApplication(encoded);

            Assert.IsNotNull(decoded);

            Assert.AreEqual(part.Nonce, decoded.Nonce);
            Assert.AreEqual(1, part.TicketInfo.Length);
        }
    }
}