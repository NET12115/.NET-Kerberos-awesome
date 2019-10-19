﻿namespace Kerberos.NET.Entities
{
    public enum KerberosErrorCode
    {
        KDC_ERR_NONE = 0,
        KDC_ERR_NAME_EXP = 1,
        KDC_ERR_SERVICE_EXP = 2,
        KDC_ERR_BAD_PVNO = 3,
        KDC_ERR_C_OLD_MAST_KVNO = 4,
        KDC_ERR_S_OLD_MAST_KVNO = 5,
        KDC_ERR_C_PRINCIPAL_UNKNOWN = 6,
        KDC_ERR_S_PRINCIPAL_UNKNOWN = 7,
        KDC_ERR_PRINCIPAL_NOT_UNIQUE = 8,
        KDC_ERR_NULL_KEY = 9,
        KDC_ERR_CANNOT_POSTDATE = 10,
        KDC_ERR_NEVER_VALID = 11,
        KDC_ERR_POLICY = 12,
        KDC_ERR_BADOPTION = 13,
        KDC_ERR_ETYPE_NOSUPP = 14,
        KDC_ERR_SUMTYPE_NOSUPP = 15,
        KDC_ERR_PADATA_TYPE_NOSUPP = 16,
        KDC_ERR_TRTYPE_NOSUPP = 17,
        KDC_ERR_CLIENT_REVOKED = 18,
        KDC_ERR_SERVICE_REVOKED = 19,
        KDC_ERR_TGT_REVOKED = 20,
        KDC_ERR_CLIENT_NOTYET = 21,
        KDC_ERR_SERVICE_NOTYET = 22,
        KDC_ERR_KEY_EXPIRED = 23,
        KDC_ERR_PREAUTH_FAILED = 24,
        KDC_ERR_PREAUTH_REQUIRED = 25,
        KDC_ERR_SERVER_NOMATCH = 26,
        KDC_ERR_MUST_USE_USER2USER = 27,
        KDC_ERR_PATH_NOT_ACCEPTED = 28,
        KDC_ERR_SVC_UNAVAILABLE = 29,
        KRB_AP_ERR_BAD_INTEGRITY = 31,
        KRB_AP_ERR_TKT_EXPIRED = 32,
        KRB_AP_ERR_TKT_NYV = 33,
        KRB_AP_ERR_REPEAT = 34,
        KRB_AP_ERR_NOT_US = 35,
        KRB_AP_ERR_BADMATCH = 36,
        KRB_AP_ERR_SKEW = 37,
        KRB_AP_ERR_BADADDR = 38,
        KRB_AP_ERR_BADVERSION = 39,
        KRB_AP_ERR_MSG_TYPE = 40,
        KRB_AP_ERR_MODIFIED = 41,
        KRB_AP_ERR_BADORDER = 42,
        KRB_AP_ERR_BADKEYVER = 44,
        KRB_AP_ERR_NOKEY = 45,
        KRB_AP_ERR_MUT_FAIL = 46,
        KRB_AP_ERR_BADDIRECTION = 47,
        KRB_AP_ERR_METHOD = 48,
        KRB_AP_ERR_BADSEQ = 49,
        KRB_AP_ERR_INAPP_CKSUM = 50,
        KRB_AP_PATH_NOT_ACCEPTED = 51,
        KRB_ERR_RESPONSE_TOO_BIG = 52,
        KRB_ERR_GENERIC = 60,
        KRB_ERR_FIELD_TOOLONG = 61,
        KDC_ERR_CLIENT_NOT_TRUSTED = 62,
        KDC_ERR_KDC_NOT_TRUSTED = 63,
        KDC_ERR_INVALID_SIG = 64,
        KDC_ERR_KEY_TOO_WEAK = 65,
        KDC_ERR_CERTIFICATE_MISMATCH = 66,
        KRB_AP_ERR_NO_TGT = 67,
        KDC_ERR_WRONG_REALM = 68,
        KRB_AP_ERR_USER_TO_USER_REQUIRED = 69,
        KDC_ERR_CANT_VERIFY_CERTIFICATE = 70,
        KDC_ERR_INVALID_CERTIFICATE = 71,
        KDC_ERR_REVOKED_CERTIFICATE = 72,
        KDC_ERR_REVOCATION_STATUS_UNKNOWN = 73,
        KDC_ERR_REVOCATION_STATUS_UNAVAILABLE = 74,
        KDC_ERR_CLIENT_NAME_MISMATCH = 75,
        KDC_ERR_KDC_NAME_MISMATCH = 76
    }
}
