﻿using Kerberos.NET;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.IO;
using System.Security;
using System.Threading.Tasks;

namespace Tests.Kerberos.NET
{
    [TestClass]
    public class ValidatorTests
    {
        [TestMethod]
        public async Task TestKerberosValidator()
        {
            var data = File.ReadAllBytes("data\\rc4-kerberos-data");
            var key = File.ReadAllBytes("data\\rc4-key-data");

            var validator = new KerberosValidator(key) { ValidateAfterDecrypt = ValidationActions.Replay };

            var result = await validator.Validate(data);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task TestKerberosValidatorNone()
        {
            var data = File.ReadAllBytes("data\\rc4-kerberos-data");
            var key = File.ReadAllBytes("data\\rc4-key-data");

            var validator = new KerberosValidator(key) { ValidateAfterDecrypt = ValidationActions.None };

            var result = await validator.Validate(data);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task TestKerberosValidatorTimeOffset()
        {
            var data = File.ReadAllBytes("data\\rc4-kerberos-data");
            var key = File.ReadAllBytes("data\\rc4-key-data");

            var validator = new KerberosValidator(key);

            validator.Now = () => DateTimeOffset.Parse("1/9/2009 5:20:00 PM +00:00", CultureInfo.InvariantCulture);

            var result = await validator.Validate(data);

            Assert.IsNotNull(result);
        }

        [TestMethod, ExpectedException(typeof(SecurityException))]
        public async Task TestKerberosValidatorBadKey()
        {
            var data = File.ReadAllBytes("data\\aes128-kerberos-data");
            var key = File.ReadAllBytes("data\\rc4-key-data");

            var validator = new KerberosValidator(key);

            await validator.Validate(data);
        }

        [TestMethod, ExpectedException(typeof(KerberosValidationException))]
        public async Task TestKerberosValidatorExpiredTicket()
        {
            var data = File.ReadAllBytes("data\\rc4-kerberos-data");
            var key = File.ReadAllBytes("data\\rc4-key-data");

            var validator = new KerberosValidator(key);

            await validator.Validate(data);
        }
    }
}
