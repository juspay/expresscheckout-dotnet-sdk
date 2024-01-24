# Change Log
## [2.0.4] - 2024-01-24
- Removed jose-jwt
- Added custom JWE and JWS
- Removed support for net452 & net46
- Added .net core support
- Added Lazy initialization of JuspayEnvironment
- Interface change for JuspayEnvironment

## [2.0.3] - 2023-11-16
- [CVE-2021-24112](https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2021-24112) removed net5.0 System.Configuration.ConfigurationManager
- Added NetFramework 8.1
- Updated SecurityProtocolType
- Interface Name changes

## [2.0.1] - 2023-10-13
- updated JuspayJWT class to accept single keyId, public and private key in constructor instead of Dictionary